namespace Domain;

public enum Phase {
    OPPONENT,
    HUMAN_INIT,
    HUMAN_PLAY
}

public class GameState<T, U>
    where T : Scale, new()
    where U : Scale, new()
    {
        private Rules<T, U> Rules { get; }
        private IPlayer<T, U>[] Players { get; }
        private Stack<ICard<T, U>> Stock;
        private Stack<ICard<T, U>> Discard;
        private List<IMeld<T, U>> Melds;
        private int Turn { get; set; }
        // private int Round { get; set; }
        private int Human { get; set; }
        public Phase GamePhase { get; private set; }

        public GameState(Rules<T, U> rules) {
            // rules have been validated on creation
            this.Rules = rules;
            this.Stock = ICard<T, U>.BuildStock(rules.NumDecks, rules.NumWc);
            this.Discard = new Stack<ICard<T, U>>();
            this.Melds = new List<IMeld<T, U>>();
            this.Turn = 0;
            // this.Round = 0;

            // Create the players.
            this.Players = new IPlayer<T, U>[this.Rules.NumPlayers];

            // Change this when randomizing the turn.
            this.Human = 0;
            this.Players[0] = new HumanPlayer<T, U>(this.Rules.PlayerR, "Human");
            int nCards = this.Rules.PlayerR.NumCards;
            for (int i = 1; i < this.Rules.NumPlayers; i++) {
                this.Players[i] = new DummyPlayer<T, U>(this.Rules.PlayerR, "bot_" + i);
            }

            /*
            // Deal cards.
            // This deals nCards to each player.
            for (int i = 0; i < Rules.NumPlayers; i++) {
                for (int j = 0; j < nCards; j++) {
                    this.Players[i].Add(this.Stock.Pop());
                }
            }
            */

            this.SufflePlayers();

            // This deals nCards times one card per player.
            for (int i = 0; i < nCards; i++) {
                for (int j = 0; j < Rules.NumPlayers; j++) {
                    this.Players[j].Add(this.Stock.Pop());
                }
            }

            if (!this.Rules.PlayerR.NeedsOut) {
                this.Discard.Push(this.Stock.Pop());
            }

            if (this.Players[0] is HumanPlayer<T, U>) {
                this.GamePhase = Phase.HUMAN_INIT;
            } else {
                this.GamePhase = Phase.OPPONENT;
            }
        }

        private void SufflePlayers() {
            int n = this.Players.Length;
            int r;
            Random rnd = new Random();
            IPlayer<T, U> aux;

            for (int i = 0; i < n; i++) {
                r = rnd.Next(i, n);
                aux = this.Players[i];
                this.Players[i] = this.Players[r];
                this.Players[r] = aux;
                if (this.Players[i] is HumanPlayer<T, U>) {
                    this.Human = i;
                }
            }
        }

        private void PrintMelds() {
            Console.WriteLine("Melds:");
            for (int i = 0; i < this.Melds.Count; i++) {
                Console.WriteLine($"- {this.Melds.ElementAt(i).Type()} {i}");
                this.Melds.ElementAt(i).Print();
            }
        }

        private void PrintPiles() {
            Console.WriteLine("Piles:");
            Console.WriteLine($"Stock: ({this.Stock.Count})");

            Console.Write($"Discard: ");
            if (this.Discard.Count == 0) {
                Console.WriteLine("--empty--");
            } else {
                this.Discard.Peek().Print();
            }
        }

        private void FilpDiscard() {
            if (this.Stock.Count == 0) {
                foreach (ICard<T, U> c in this.Discard) {
                    this.Stock.Push(c);
                }
            }
        }

        public void Update() {
            this.FilpDiscard();

            IPlayer<T, U> p = this.Players[this.Turn%this.Players.Length];
            PlayerAction pa = p.ChooseAction();

            if (pa == PlayerAction.PickStock) {
                p.DoPickStock(this.Stock);
            } else if (pa == PlayerAction.PickDiscard) {
                p.DoPickDiscard(this.Discard);
            } else if (pa == PlayerAction.MeldRun) {
                Console.WriteLine("Choose to meld a run.");
                p.DoMeldRun(this.Rules, this.Melds);
            } else if (pa == PlayerAction.MeldSet) {
                Console.WriteLine("Choose to meld a set.");
                p.DoMeldSet(this.Rules, this.Melds);
            } else if (pa == PlayerAction.LayOff) {
                Console.WriteLine("Choos the cards to lay off.");
                p.DoLayOff(this.Melds);
            } else if (pa == PlayerAction.Replace) {
                p.DoReplace(this.Melds);
            }
            else if (pa == PlayerAction.Discard) {
                p.DoShed(this.Discard);
                this.Turn++;
            } else {
                Console.WriteLine("Unknown action");
            }

            Console.WriteLine($"{this.Players[this.Turn%4]} won the game.");
        }

        // Maybe this can just return coords that it asks to the player.
        public ArrayHand<T, U> HumanHand() {
            return ((HumanPlayer<T, U>)this.Players[this.Human]).GetHand();
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

        public bool StockEmpty() {
            return this.Stock.Count == 0;
        }

        public ICard<T, U>? TopDiscard() {
            if (this.Discard.Count == 0) {
                return null;
            } else {
                return this.Discard.Peek();
            }
        }

        public PlayerProfile CurrentProfile() {
            return this.Players[this.CurrentPlayer()].GetProfile();
        }

        public void NextPhase() {
            if (this.Turn%this.Players.Length == this.Human) {
                switch (this.GamePhase) {
                    case Phase.OPPONENT:
                        this.GamePhase = Phase.HUMAN_INIT;
                        break;
                    case Phase.HUMAN_INIT:
                        this.GamePhase = Phase.HUMAN_PLAY;
                        break;
                    case Phase.HUMAN_PLAY:
                        this.GamePhase = Phase.OPPONENT;
                        break;
                    default:
                        throw new Exception("Undefined behaviour for game phase.");
                }
            }
        }

        public string GetPhaseStatus() {
            if (this.IsHumanTurn()) {
                return "It's human's turn";
            } else {
                return this.CurrentProfile().Name + "'s turn";
            }
        }

        public int CurrentPlayer() {
            return this.Turn%this.Players.Length;
        }

        public bool IsHumanTurn() {
            return this.CurrentPlayer() == this.Human;
        }

        public ICard<T, U>? BotPick() {
            // change the player inteface to only include the three phases.

            /*
            if (this.Discard.Count == 0) {
                return null;
            }
            ICard<T, U> c = this.Discard.Peek();

            this.Players[this.CurrentPlayer()].DoPickDiscard(this.Discard);

            return c;
            */
            Console.WriteLine($"Stock: {this.Stock.Count}");
            Console.WriteLine($"Discard: {this.Discard.Count}");
            ICard<T, U> c = this.Stock.Peek();
            this.Players[this.CurrentPlayer()].DoPickStock(this.Stock);

            // stock returns null.
            // discard returns card.
            return null;
        }

        public ICard<T, U> BotDiscard() {
            ICard<T, U> c = this.Players[this.CurrentPlayer()].DoShed(this.Discard);
            return c;
        }

        public int NumPlayers() {
            return this.Players.Length;
        }

        public int StockDepth() {
            return this.Stock.Count;
        }

        public void NextTurn() {
            this.Turn = (this.Turn + 1)%this.Players.Length;
        }

        public int HumanPos() {
            return this.Human;
        }
    }
