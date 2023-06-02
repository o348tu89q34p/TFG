namespace Domain;

public class GameState<T, U>
    where T : Scale, new()
    where U : Scale, new()
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
            // This deals nCards to each player.
            for (int i = 0; i < Rules.NumPlayers; i++) {
                for (int j = 0; j < nCards; j++) {
                    this.Players[i].Add(this.Stock.Pop());
                }
            }

            /*
            // This deals nCards times one card per player.
            for (int i = 0; i < nCards; i++) {
                for (int j = 0; j < Rules.NumPlayers; j++) {
                    this.Players[j].Add(this.Stock.Pop());
                }
            }
            */

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

        /*
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
        */

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

        public int StockDepth() {
            return this.Stock.Count;
        }

        public void NextTurn() {
            this.Turn = (this.Turn + 1)%this.Players.Length;
        }

        public int HumanPos() {
            return this.Human;
        }

        public (string?, ResultMove<ICard<T, U>>?) HumanPlay(ResultMove<int> move) {
            try {
                return (null, this.CurrentPlayer().MakePlay(this.Rules, this.Stock, this.Discard, this.Melds));
            } catch (InvalidCastException e) {
                return (e.Message, null);
            }
        }
    }
