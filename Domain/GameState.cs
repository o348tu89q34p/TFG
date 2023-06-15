namespace Domain;

public class GameState<T, U> where T : Scale, new() where U : Scale, new()
{
    private Rules<T, U> Rules { get; }
    private SinglePlayer<T, U>[] Players { get; }
    private Stack<ICard<T, U>> Stock;
    private Stack<ICard<T, U>> Discard;
    private List<IMeld<T, U>> Melds;
    private int Turn { get; set; }
    // private int Round { get; set; }
    private int Human { get; set; }
    private Brain<T, U> Ai { get; }

    public GameState(Rules<T, U> rules) {
        // rules have been validated on creation
        this.Rules = rules;
        this.Stock = ICard<T, U>.BuildStock(rules.NumDecks, rules.NumWc);
        this.Discard = new Stack<ICard<T, U>>();
        this.Melds = new List<IMeld<T, U>>();
        this.Turn = 0;
        // this.Round = 0;

        // Create the players.
        this.Players = new SinglePlayer<T, U>[this.Rules.NumPlayers];

        // Change this when randomizing the turn.
        this.Human = 0;
        this.Players[0] = new SinglePlayer<T, U>(this.Rules.PlayerR, "Human");
        int nCards = this.Rules.PlayerR.NumCards;
        for (int i = 1; i < this.Rules.NumPlayers; i++) {
            this.Players[i] = new SinglePlayer<T, U>(this.Rules.PlayerR, "bot_" + i);
        }

        // Deal cards.
        /*
        // This deals nCards to each player.
        for (int i = 0; i < Rules.NumPlayers; i++) {
        for (int j = 0; j < nCards; j++) {
        this.Players[i].Add(this.Stock.Pop());
        }
        }
        */

        // This deals nCards times one card per player.
        for (int i = 0; i < nCards; i++) {
            for (int j = 0; j < Rules.NumPlayers; j++) {
                this.Players[j].Add(this.Stock.Pop());
            }
        }

        this.SufflePlayers();

        if (!this.Rules.PlayerR.NeedsOut) {
            this.Discard.Push(this.Stock.Pop());
        }

        this.Ai = new Brain<T, U>(rules);
    }

    private void SufflePlayers() {
        int n = this.Players.Length;
        int r;
        Random rnd = new Random();
        SinglePlayer<T, U> aux;

        for (int i = 0; i < n; i++) {
            r = rnd.Next(i, n);
            aux = this.Players[i];
            this.Players[i] = this.Players[r];
            this.Players[r] = aux;
            if (this.Players[i].GetProfile().Name.Equals("Human")) {
                this.Human = i;
            }
        }
    }

    public void FlipDiscard() {
        if (this.Stock.Count <= 0) {
            ICard<T, U> top = this.Discard.Pop();
            while (this.Discard.Count > 0) {
                this.Stock.Push(this.Discard.Pop());
            }
            this.Discard.Push(top);
        }
    }

    // Maybe this can just return coords that it asks to the player.
    public List<ICard<T, U>> HumanHand() {
        return this.Players[this.Human].GetCards();
    }

    public List<PlayerProfile> GetOpponents() {
        int nOpponents = this.Players.Length - 1;
        List<PlayerProfile> res = new List<PlayerProfile>(nOpponents);

        int pointer = this.Human + 1;
        int count = 0;
        while (count < nOpponents) {
            res.Add(this.Players[(pointer + this.NumPlayers())%this.NumPlayers()].GetProfile());
            pointer++;
            if (pointer >= this.NumPlayers()) {
                pointer = 0;
            }
            count++;
        }

        return res;
    }

    public (bool, ICard<T, U>?) PilesStatus() {
        ICard<T, U>? topDiscard = null;
        if (this.Discard.Count > 0) {
            topDiscard = this.Discard.Peek();
        }

        return (this.Stock.Count > 0, topDiscard);
    }

    public PlayerProfile CurrentProfile() {
        return this.Players[this.CurrentPlayerPos()].GetProfile();
    }

    public SinglePlayer<T, U> CurrentPlayer() {
        return this.Players[this.CurrentPlayerPos()];
    }

    public int CurrentPlayerPos() {
        return this.Turn%this.Players.Length;
    }

    public bool IsHumanTurn() {
        return this.CurrentPlayerPos() == this.Human;
    }

    public string TurnStatus() {
        return this.CurrentPlayer().GetProfile().Name + "'s turn.";
    }

    public ResultMove<ICard<T, U>> BotPick() {
        this.CurrentPlayer().UpdateMove(this.Ai.MakePick(this.CurrentPlayer()));
        return this.CurrentPlayer().MakePlay(this.Rules, this.Stock, this.Discard, this.Melds);
    }

    public ResultMove<ICard<T, U>>? BotPlay() {
        ResultMove<int>? play = this.Ai.MakePlay(this.CurrentPlayer());

        if (play == null) {
            return null;
        }

        this.CurrentPlayer().UpdateMove(play);
        return this.CurrentPlayer().MakePlay(this.Rules, this.Stock, this.Discard, this.Melds);
    }

    public ResultMove<ICard<T, U>> BotDiscard() {
        this.CurrentPlayer().UpdateMove(this.Ai.MakeDiscard(this.CurrentPlayer()));
        return this.CurrentPlayer().MakePlay(this.Rules, this.Stock, this.Discard, this.Melds);
    }

    public int NumPlayers() {
        return this.Players.Length;
    }

    public void NextTurn() {
        this.Turn = (this.Turn + 1)%this.Players.Length;
    }

    public int HumanPos() {
        return this.Human;
    }

    public (string?, ResultMove<ICard<T, U>>?) HumanPlay(ResultMove<int> move) {
        try {
            this.CurrentPlayer().UpdateMove(move);
            return (null, this.CurrentPlayer().MakePlay(this.Rules, this.Stock, this.Discard, this.Melds));
        } catch (InvalidOperationException e) {
            return (e.Message, null);
        }
    }

    public bool CurrentHasWon() {
        return this.CurrentPlayer().HasWon();
    }

    public void HumanSortRun() {
        this.Players[this.Human].SortForRun();
    }

    public void HumanSortSet() {
        this.Players[this.Human].SortForSet();
    }

    public List<List<ICard<T, U>>> CurrentMelds() {
        List<List<ICard<T, U>>> lst = new List<List<ICard<T, U>>>(this.Melds.Count);
        foreach (IMeld<T, U> meld in this.Melds) {
            lst.Add(meld.GetCards());
        }

        return lst;
    }

    public List<List<ICard<T, U>>> PlayerHands() {
        List<List<ICard<T, U>>> lst = new List<List<ICard<T, U>>>(this.Players.Length);
        for (int i = 0; i < this.Players.Length; i++) {
            lst.Add(this.Players[i].GetCards());
        }

        return lst;
    }

    public List<PlayerProfile> PlayerProfiles() {
        List<PlayerProfile> lst = new List<PlayerProfile>(this.Players.Length);
        for (int i = 0; i < this.Players.Length; i++) {
            lst.Add(this.Players[i].GetProfile());
        }

        return lst;
    }
}
