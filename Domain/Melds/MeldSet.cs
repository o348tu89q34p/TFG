namespace Domain {
    public class MeldSet<T, U> : IMeld<T, U>
        where T : Scale, new()
        where U : Scale, new()
        {
            private LinkedList<NaturalCard<T, U>> Cards { get; set; }
            private LinkedList<WildCard<T, U>> Wilds { get; set; }
            private NaturalCard<T, U> First { get; set; }
            private MeldRules Rules { get; }

            public MeldSet(ArrayHand<T, U> hand, MeldRules rules) {
                this.Rules = rules;
                var (first, _) = hand.FirstNatural();
                if (first == null) {
                    throw new ArgumentException("No natural cards in the meld.");
                }
                if (!rules.MultWc && hand.NumWc > 1) {
                    throw new ArgumentException("No multiple wild cards allowed.");
                }
                if (!rules.ConsecWc && hand.HasConsecWc) {
                    throw new ArgumentException("No consecutive wild cards allowed.");
                }
                if (hand.Size() < rules.MinSetLen) {
                    throw new ArgumentException("Hand size below minimum number of cards.");
                }
                if (hand.Size() > rules.MaxSetLen) {
                    throw new ArgumentException("Hand size above maximum number of cards.");
                }

                this.First = first;
                this.Cards = new LinkedList<NaturalCard<T, U>>();
                this.Wilds = new LinkedList<WildCard<T, U>>();

                (bool canAdd, string msg) = this.ValidateSet(hand);
                if (!canAdd) {
                    throw new ArgumentException(msg);
                }

                for (int i = 0; i < hand.Size(); i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    if (aux.IsWild()) {
                        this.Wilds.AddLast((WildCard<T, U>)aux);
                    } else {
                        this.Cards.AddLast((NaturalCard<T, U>)aux);
                    }
                }
            }

            private int Size() {
                return this.Cards.Count + this.Wilds.Count;
            }

            private (bool, string) ValidateSet(ArrayHand<T, U> hand) {
                int handLen = hand.Size();
                if (this.Size() + handLen > this.Rules.MaxSetLen) {
                    return (false, "The cards do not fit.");
                }
                if (!this.Rules.MultWc && (hand.NumWc + this.Wilds.Count) > 1) {
                    return (false, "The cards make the set have multiple wild cards.");
                }

                for (int i = 0; i < handLen; i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    if (aux.IsWild()) {
                        continue;
                    }

                    if (aux.CompareRank(this.First) != 0) {
                        return (false, "The cards have different ranks.");
                    }

                    for (int j = i + 1; j < handLen; j++) {
                        if (aux.CompareTo(hand.GetAt(j)) == 0) {
                            return (false, "Some cards are duplicated.");
                        } else if (!hand.GetAt(j).IsWild() && aux.CompareRank(hand.GetAt(j)) != 0) {
                            return (false, "The cards do not have the same rank.");
                        }
                    }

                    if (this.Cards.Contains(aux)) {
                        return (false, "Some cards are already in the set.");
                    }
                }

                return (true, "");
            }

            public void Add(ArrayHand<T, U> hand) {
                (bool canAdd, string msg) = this.ValidateSet(hand);
                if (!canAdd) {
                    throw new ArgumentException(msg);
                }

                for (int i = 0; i < hand.Size(); i++) {
                    ICard<T, U> c = hand.GetAt(i);
                    if (c.IsWild()) {
                        this.Wilds.AddLast((WildCard<T, U>)c);
                    } else {
                        this.Cards.AddLast((NaturalCard<T, U>)c);
                    }
                }
            }

            public WildCard<T, U> Replace(ICard<T, U> card, int n) {
                if (this.Size() != this.Rules.MaxSetLen) {
                    throw new ArgumentException("The set must be full before replacing.");
                }
                if (this.Wilds.Count == 0) {
                    throw new ArgumentException("The set has no wild cards.");
                }
                if (card.IsWild()) {
                    throw new ArgumentException("The replacement cannot be a wild card.");
                }
                /*
                  // There are other guards in place that might make this not needed.
                if (n < this.Cards.Count) {
                    // Wilds will be at the end.
                    throw new Exception("The position refers to a non wild card.");
                }
                */

                NaturalCard<T, U> newCard = (NaturalCard<T, U>)card;
                if (newCard.CompareRank(this.First) != 0) {
                    throw new ArgumentException("Invalid rank for the replacement.");
                }
                if (this.Cards.Contains(newCard)) {
                    throw new ArgumentException("The replacement is already in the set.");
                }

                if (this.Wilds.First == null) {
                    throw new Exception("Check to remove the warning.");
                }
                this.Cards.AddLast(newCard);
                WildCard<T, U> ret = this.Wilds.First.Value;
                this.Wilds.RemoveFirst();

                return ret;
            }

            public List<ICard<T, U>> GetCards() {
                int dims = this.Wilds.Count + this.Cards.Count;
                List<ICard<T, U>> res = new List<ICard<T, U>>(dims);
                foreach (WildCard<T, U> wc in this.Wilds) {
                    res.Add((ICard<T, U>)wc);
                }
                foreach (NaturalCard<T, U> nc in this.Cards) {
                    res.Add((ICard<T, U>)nc);
                }

                return res;
            }

            public void Print() {
                foreach (ICard<T, U> c in this.Cards) {
                    c.Print();
                }

                foreach (ICard<T, U> c in this.Wilds) {
                    c.Print();
                }
            }

            public string Type() {
                return "Set";
            }
        }
}
