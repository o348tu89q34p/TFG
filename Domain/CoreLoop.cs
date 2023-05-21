using SFML.Graphics;
using SFML.Window;

namespace Domain {
    public class CoreLoop<T, U>
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

            public CoreLoop(RenderWindow window, Rules<T, U> rules) {
                // rules have been validated on creation
                this.Rules = rules;
                this.Stock = ICard<T, U>.BuildStock(rules.NumDecks, rules.NumWc);
                this.Discard = new Stack<ICard<T, U>>();
                this.Melds = new List<IMeld<T, U>>();
                this.Turn = 0;
                // this.Round = 0;

                // Create the players.
                this.Players = new IPlayer<T, U>[this.Rules.NumPlayers];

                this.Players[0] = new HumanPlayer<T, U>(window, this.Rules.PlayerR, "Human");
                int nCards = this.Rules.PlayerR.NumCards;
                for (int i = 1; i < this.Rules.NumPlayers; i++) {
                    this.Players[i] = new DummyPlayer<T, U>(window, this.Rules.PlayerR, "bot_" + i);
                }

                // Deal cards.
                // TODO: UNDO the not shufflin and use the other dealing loop.
                for (int i = 0; i < Rules.NumPlayers; i++) {
                    for (int j = 0; j < nCards; j++) {
                        this.Players[i].Add(this.Stock.Pop());
                    }
                }
                /*
                for (int i = 0; i < nCards; i++) {
                    for (int j = 0; j < Rules.NumPlayers; j++) {
                        this.Players[j].Add(this.Stock.Pop());
                    }
                }
                */

                if (!this.Rules.PlayerR.NeedsOut) {
                    this.Discard.Push(this.Stock.Pop());
                }
            }

            private void PrintMelds() {
                Console.WriteLine("Melds:");
                for (int i = 0; i < this.Melds.Count; i++) {
                    Console.WriteLine($"- {this.Melds.ElementAt(i).Type()} {i}");
                    this.Melds.ElementAt(i).Print();
                }
            }

            /*
            private void PrintPlayers() {
                Console.WriteLine("Players:");
                for (int i = 0; i < this.Players.Length; i++) {
                    Console.WriteLine(this.Players[i].GetName());
                    this.Players[i].ShowHand();
                }
            }
            */

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
                /*
                  this.PrintMelds();
                  this.PrintPlayers();
                  this.PrintPiles();
                */

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

            public void Update(RenderWindow window) {
                this.Players[this.Turn%this.Players.Length].Update(window);
                /*
                private Stack<ICard<T, U>> Stock;
                private Stack<ICard<T, U>> Discard;
                private List<IMeld<T, U>> Melds;
                private int Turn { get; set; }
                */
            }

            public void Draw(RenderWindow window) {
                this.Players[this.Turn%this.Players.Length].Render(window);
            }
        }
}
