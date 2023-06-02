namespace Domain;
public class DummyPlayer<T, U> : IPlayer<T, U>
    where T : Scale, new() where U : Scale, new()
    {
        private PlayerRules Rules { get; }
        private ArrayHand<T, U> Hand { get; }
        private string Name { get; }
        public bool CameOut { get; }
        public bool HasPicked { get; set; }
        private ICard<T, U>? Picked { get; set; }
        private int TurnState { get; set; }

        public DummyPlayer(PlayerRules rules, string name) {
            this.Rules = rules;
            this.Hand = new ArrayHand<T, U>(rules.NumCards + 1);
            this.Name = name;
            this.CameOut = false;
            this.HasPicked = false;
            this.Picked = null;
            this.TurnState = 0;
        }

        public PlayerAction ChooseAction() {
            switch (this.TurnState) {
                case 0:
                    return PlayerAction.PickStock;
                case 1:
                    return PlayerAction.Discard;
                default:
                    throw new Exception("Unimplemented dummy player state.");
            }
        }

        public void DoPickStock(Stack<ICard<T, U>> stock) {
            if (this.HasPicked) {
                return;
            }

            this.Hand.Append(stock.Pop());
            this.HasPicked = true;
            this.TurnState = 1;
        }

        public void DoPickDiscard(Stack<ICard<T, U>> discard) {
            if (this.HasPicked || (this.Rules.NeedsOut && !this.CameOut)) {
                return;
            }

            ICard<T, U> card = discard.Pop();
            this.Picked = card;
            this.Hand.Append(card);
            this.TurnState = 1;
        }

        public void DoMeldRun(Rules<T, U> rules, List<IMeld<T, U>> melds) {
        }

        public void DoMeldSet(Rules<T, U> rules, List<IMeld<T, U>> melds) {
        }

        public void DoLayOff(List<IMeld<T, U>> melds) {
        }

        public void DoReplace(List<IMeld<T, U>> melds) {
        }

        public ICard<T, U> DoShed(Stack<ICard<T, U>> discard) {
            discard.Push(this.Hand.GetAt(0));
            ICard<T, U> c = this.Hand.GetAt(0);
            this.Hand.RemoveAt(0);
            this.HasPicked = false;
            this.Picked = null;
            this.TurnState = 0;

            return c;
        }

        public bool HasWon() {
            return this.Hand.IsEmpty();
        }

        public void Add(ICard<T, U> card) {
            if (this.Hand.IsFull()) {
                throw new Exception("Missmatch between the hand size and cards dealt.");
            }

            this.Hand.Append(card);
        }

        public string GetName() {
            return this.Name;
        }

        public PlayerProfile GetProfile() {
            return new PlayerProfile(this.Name, this.Hand.Size());
        }

        public void ShowHand() {
            this.Hand.Print();
        }

        public bool HasComeOut() {
            return this.CameOut;
        }

        public ArrayHand<T, U> GetHand() {
            return this.Hand;
        }
    }
