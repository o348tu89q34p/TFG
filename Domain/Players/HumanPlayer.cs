namespace Domain;

public class HumanPlayer<T, U> : IPlayer<T, U>
    where T : Scale, new() where U : Scale, new()
    {
        private PlayerRules Rules { get; }
        private ArrayHand<T, U> Hand { get; }
        private string Name { get; }
        public bool CameOut { get; }
        public bool HasPicked { get; set; }
        private ICard<T, U>? Picked { get; set; }
        //private 

        public HumanPlayer(PlayerRules rules, string name) {
            this.Rules = rules;
            this.Hand = new ArrayHand<T, U>(rules.NumCards + 1);
            this.Name = name;
            // Need to figure out how to determine this.
            this.CameOut = false;
            this.HasPicked = false;
            this.Picked = null;
        }

        public PlayerAction ChooseAction() {
            return InputReader.PickAction();
        }

        public void DoPickStock(Stack<ICard<T, U>> stock) {
            if (this.HasPicked) {
                Console.WriteLine("You already picked this turn.");
                return;
            }
            if (this.Hand.IsFull()) {
                throw new Exception("Hand full when picking.");
            }

            this.Hand.Append(stock.Pop());
            this.HasPicked = true;
        }

        public void DoPickDiscard(Stack<ICard<T, U>> discard) {
            if (this.HasPicked) {
                Console.WriteLine("You already picked this turn.");
                return;
            }
            if (this.Rules.NeedsOut && !this.CameOut) {
                Console.WriteLine("You must come out in order to pick from the discard pile.");
                return;
            }

            if (this.Hand.IsFull()) {
                throw new Exception("Hand full when picking.");
            } else if (discard.Count == 0) {
                throw new Exception("Cannot pick from an empty discard pile.");
            }

            ICard<T, U> card = discard.Pop();
            this.HasPicked = true;
            this.Picked = card;
            this.Hand.Append(card);
        }

        public void DoMeldRun(Rules<T, U> rules, List<IMeld<T, U>> melds) {
            if (!this.HasPicked) {
                Console.WriteLine("You must pick a card before doing any other action.");
                return;
            }

            string prompt = "Select the cards that form the run.";
            int[] nCards = InputReader.ReadIndices(prompt, 0, this.Hand.Size(), true);
            ArrayHand<T, U> cards = this.GetCards(nCards);

            try {
                var mr = new MeldRun<T, U>(cards, rules.MeldR);
                if (rules.EndDiscard && cards.Size() == this.Hand.Size()) {
                    Console.WriteLine("The last card must be discaded.");
                    return;
                }
                melds.Add(mr);
            } catch {
                Console.WriteLine("The cards given don't form a valid run.");
                return;
            }
            this.Dispose(nCards);
        }

        public void DoMeldSet(Rules<T, U> rules, List<IMeld<T, U>> melds) {
            if (!this.HasPicked) {
                Console.WriteLine("You must pick a card before doing any other action.");
                return;
            }

            string prompt = "Select the cards that form the set.";
            int[] nCards = InputReader.ReadIndices(prompt, 0, this.Hand.Size(), true);
            ArrayHand<T, U> cards = this.GetCards(nCards);

            try {
                var mr = new MeldSet<T, U>(cards, rules.MeldR);
                if (rules.EndDiscard && cards.Size() == this.Hand.Size()) {
                    Console.WriteLine("The last card must be discaded.");
                    return;
                }
                melds.Add(mr);
            } catch {
                Console.WriteLine("The cards given don't form a valid set.");
                return;
            }
            this.Dispose(nCards);
        }

        public void DoLayOff(List<IMeld<T, U>> melds) {
            if (melds.Count == 0) {
                Console.WriteLine("There are no melds.");
                return;
            }

            string prompt = "Choose the meld where to lay off.";
            int nMeld = InputReader.ReadIndex(prompt, 0, melds.Count);

            prompt = "Secect the cards to lay off.";
            int[] nCards = InputReader.ReadIndices(prompt, 0, this.Hand.Size(), true);
            ArrayHand<T, U> cards = this.GetCards(nCards);

            try {
                melds[nMeld].Add(cards);
            } catch {
                Console.WriteLine("The cards selected do not match the meld.");
                return;
            }
            this.Dispose(nCards);
        }

        public void DoReplace(List<IMeld<T, U>> melds) {
            string prompt = "Choose the meld where the wild card is.";
            int nMeld = InputReader.ReadIndex(prompt, 0, melds.Count);
            prompt = "Choose the wild card to replace.";
            int nWild = InputReader.ReadIndex(prompt, 0, melds.Count);
            prompt = "Choose the natural card to replace it with.";
            int nNat = InputReader.ReadIndex(prompt, 0, melds.Count);

            try {
                melds[nMeld].Replace(this.Hand.GetAt(nNat), nWild);
            } catch {
                Console.WriteLine("Invalid replacement.");
                return;
            }
        }

        public ICard<T, U> DoShed(Stack<ICard<T, U>> discard) {
            string prompt = "Enter the position of the card to shed.";
            int i = InputReader.ReadIndex(prompt, 0, this.Hand.Size());

            ICard<T, U> card = this.Hand.GetAt(i);
            if (this.Picked == card) {
                throw new Exception("Cannot discard the picked card if it's from the discard pile.");
            }

            discard.Push(this.Hand.GetAt(i));
            ICard<T, U> c = this.Hand.GetAt(i);
            this.Hand.RemoveAt(i);
            this.HasPicked = false;
            this.Picked = null;

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

        private ArrayHand<T, U> GetCards(int[] positions) {
            var arr = new ArrayHand<T, U>(positions.Length);
            for (int i = 0; i < positions.Length; i++) {
                arr.Append(this.Hand.GetAt(positions[i]));
            }

            return arr;
        }

        private void Dispose(int[] pos) {
            Array.Sort(pos);

            for (int i = 0; i < pos.Length; i++) {
                this.Hand.RemoveAt(pos[i] - i);
            }
        }

        public string GetName() {
            return this.Name;
        }

        /*
          public void ShowHand() {
          this.Hand.Print();
          }
        */

        public PlayerProfile GetProfile() {
            return new PlayerProfile(this.Name, this.Hand.Size());
        }

        public bool HasComeOut() {
            return this.CameOut;
        }

        public ArrayHand<T, U> GetHand() {
            return this.Hand;
        }
    }
