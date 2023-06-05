namespace Domain;

public class SinglePlayer<T, U> where T : Scale, new() where U : Scale, new()
{
    private PlayerRules Rules { get; }
    private ArrayHand<T, U> Hand { get; }
    private string Name { get; }
    public bool CameOut { get; }
    public bool HasPicked { get; set; }
    private ICard<T, U>? Picked { get; set; }
    private ResultMove<int> Move { get; set; }

    public SinglePlayer(PlayerRules rules, string name) {
        this.Rules = rules;
        this.Hand = new ArrayHand<T, U>(rules.NumCards + 1);
        this.Name = name;
        this.CameOut = false;
        this.HasPicked = false;
        this.Picked = null;
        this.Move = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
    }

    private ResultMove<ICard<T, U>> MakePickStock(Stack<ICard<T, U>> stock) {
        if (this.HasPicked) {
            throw new InvalidOperationException("You already picked this turn.");
        }
        if (this.Hand.IsFull()) {
            throw new Exception("Hand full when picking.");
        }

        var cards = new List<ICard<T, U>>();
        cards.Add(stock.Peek());

        this.Hand.Append(stock.Pop());
        this.HasPicked = true;

        return new ResultMove<ICard<T, U>>(this.Move.Move, cards, null, null);
    }

    private ResultMove<ICard<T, U>> MakePickDiscard(Stack<ICard<T, U>> discard) {
        if (this.HasPicked) {
            throw new InvalidOperationException("You already picked this turn.");
        }
        if (this.Rules.NeedsOut && !this.CameOut) {
            throw new InvalidOperationException("You must come out in order to pick from the discard pile.");
        }
        if (this.Hand.IsFull()) {
            throw new Exception("Hand full when picking.");
        }
        if (discard.Count == 0) {
            throw new Exception("Cannot pick from an empty discard pile.");
        }

        var cards = new List<ICard<T, U>>();
        cards.Add(discard.Peek());

        ICard<T, U> card = discard.Pop();
        this.HasPicked = true;
        this.Picked = card;
        this.Hand.Append(card);

        return new ResultMove<ICard<T, U>>(this.Move.Move, cards, null, null);
    }

    private ResultMove<ICard<T, U>> MakeRun(Rules<T, U> rules, List<IMeld<T, U>> melds) {
        if (!this.HasPicked) {
            throw new InvalidOperationException("You must pick a card before doing any other action.");
        }

        var twoLists = this.GetCards(this.Move.CardsMoved);
        ArrayHand<T, U> cards = twoLists.Item1;
        List<ICard<T, U>> retCards = twoLists.Item2;

        try {
            var mr = new MeldRun<T, U>(cards, rules.MeldR);
            if (rules.EndDiscard && cards.Size() == this.Hand.Size()) {
                throw new InvalidOperationException("The last card must be discaded.");
            }
            melds.Add(mr);
        } catch {
            throw new InvalidOperationException("The cards given don't form a valid run.");
        }

        this.Dispose(this.Move.CardsMoved);

        return new ResultMove<ICard<T, U>>(this.Move.Move, retCards, null, null);
    }

    private ResultMove<ICard<T, U>> MakeSet(Rules<T, U> rules, List<IMeld<T, U>> melds) {
        if (!this.HasPicked) {
            throw new InvalidOperationException("You must pick a card before doing any other action.");
        }

        var twoLists = this.GetCards(this.Move.CardsMoved);
        ArrayHand<T, U> cards = twoLists.Item1;
        List<ICard<T, U>> retCards = twoLists.Item2;

        try {
            var mr = new MeldSet<T, U>(cards, rules.MeldR);
            if (rules.EndDiscard && cards.Size() == this.Hand.Size()) {
                throw new InvalidOperationException("The last card must be discaded.");
            }
            melds.Add(mr);
        } catch (Exception e) {
            for (int i = 0; i < cards.Size(); i++) {
                cards.GetAt(i).Print();
            }
            //throw new InvalidOperationException("The cards given don't form a valid set.");
            throw new InvalidOperationException(e.Message);
        }

        this.Dispose(this.Move.CardsMoved);

        return new ResultMove<ICard<T, U>>(this.Move.Move, retCards, null, null);
    }

    private ResultMove<ICard<T, U>> MakeShed(Stack<ICard<T, U>> discard) {
        int pos = this.Move.CardsMoved.ElementAt(0);
        ICard<T, U> card = this.Hand.GetAt(pos);

        if (this.Picked != null && this.Picked == card) {
            throw new InvalidOperationException("Cannot discard the picked card if it's from the discard pile.");
        }

        discard.Push(card);
        this.Hand.RemoveAt(pos);
        this.HasPicked = false;
        this.Picked = null;

        List<ICard<T, U>> res = new List<ICard<T, U>>();
        res.Add(card);

        return new ResultMove<ICard<T, U>>(this.Move.Move, res, null, null);
    }

    public ResultMove<ICard<T, U>> MakePlay(Rules<T, U> rules, Stack<ICard<T, U>> stock, Stack<ICard<T, U>> discard, List<IMeld<T, U>> melds) {
        switch (this.Move.Move) {
            case MoveKind.STOCK:
                return this.MakePickStock(stock);
            case MoveKind.DISCARD:
                return this.MakePickDiscard(discard);
            case MoveKind.RUN:
                return this.MakeRun(rules, melds);
            case MoveKind.SET:
                return this.MakeSet(rules, melds);
            case MoveKind.SHED:
                return this.MakeShed(discard);
            default:
                throw new Exception("Specified a play that is not defined.");
        }
    }

    public void UpdateMove(ResultMove<int> playerMove) {
        this.Move = playerMove;
    }

    public void Add(ICard<T, U> card) {
        if (this.Hand.IsFull()) {
            throw new Exception("Missmatch between the hand size and cards dealt.");
        }

        this.Hand.Append(card);
    }

    public PlayerProfile GetProfile() {
        return new PlayerProfile(this.Name, this.Hand.Size());
    }

    public ArrayHand<T, U> GetHand() {
        return this.Hand;
    }

    public List<ICard<T, U>> GetCards() {
        return this.Hand.Hand;
    }

    public bool HasComeOut() {
        return this.CameOut;
    }

    private (ArrayHand<T, U>, List<ICard<T, U>>) GetCards(List<int> positions) {
        var arr = new ArrayHand<T, U>(positions.Count);
        var crd = new List<ICard<T, U>>(positions.Count);
        for (int i = 0; i < positions.Count; i++) {
            arr.Append(this.Hand.GetAt(positions[i]));
            crd.Add(this.Hand.GetAt(positions[i]));
        }

        return (arr, crd);
    }

    private void Dispose(List<int> positions) {
        positions.Sort();

        for (int i = 0; i < positions.Count; i++) {
            this.Hand.RemoveAt(positions[i] - i);
        }
    }

    public bool HasWon() {
        return this.Hand.Size() == 0;
    }

    public void SortForRun() {
        this.Hand.SortRuns();
    }

    public void SortForSet() {
        this.Hand.SortSets();
    }
}
