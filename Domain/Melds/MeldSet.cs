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
                    Console.WriteLine("No natural cards in the meld.");
                    throw new ArgumentException("No natural cards.");
                }
                if (!rules.MultWc && hand.NumWc > 1) {
                    Console.WriteLine("No multiple wild cards allowed.");
                    throw new ArgumentException("No multiple wild cards allowed.");
                }
                if (!rules.ConsecWc && hand.HasConsecWc) {
                    Console.WriteLine("No consecutive wild cards allowed.");
                    throw new ArgumentException("No consecutive wild cards allowed.");
                }
                if (hand.Size() < rules.MinSetLen) {
                    Console.WriteLine("Hand size below minimum number of cards.");
                    throw new ArgumentException("Hand size below minimum number of cards.");
                }
                if (hand.Size() > rules.MaxSetLen) {
                    Console.WriteLine("Hand size above maximum number of cards.");
                    throw new ArgumentException("Hand size above maximum number of cards.");
                }

                this.First = first;
                this.Cards = new LinkedList<NaturalCard<T, U>>();
                this.Wilds = new LinkedList<WildCard<T, U>>();

                for (int i = 0; i < hand.Size(); i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    if (aux.IsWild()) {
                        this.Wilds.AddLast((WildCard<T, U>)aux);
                    } else if (first.CompareRank(aux) != 0) {
                        throw new ArgumentException("mixed ranks");
                    } else if (this.Cards.Contains(aux)) {
                        throw new ArgumentException("duplicated cards");
                    } else {
                        this.Cards.AddLast((NaturalCard<T, U>)aux);
                    }
                }
            }

            private int Size() {
                return this.Cards.Count + this.Wilds.Count;
            }

            private bool CanAddHand(ArrayHand<T, U> hand) {
                int handLen = hand.Size();
                if ((this.Size() + handLen > this.Rules.MaxSetLen) ||
                    (!this.Rules.MultWc && (hand.NumWc + this.Wilds.Count) > 1))
                {
                    return false;
                }

                for (int i = 0; i < handLen; i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    if (aux.IsWild()) {
                        continue;
                    }

                    if (aux.CompareRank(this.First) != 0) {
                        return false;
                    }

                    for (int j = i + 1; j < handLen; j++) {
                        if (aux.CompareTo(hand.GetAt(j)) == 0) {
                            return false;
                        }
                    }

                    if (this.Cards.Contains(aux)) {
                        return false;
                    }
                }

                return true;
            }

            public void Add(ArrayHand<T, U> hand) {
                if (!this.CanAddHand(hand)) {
                    throw new ArgumentException("Invalid addition to set.");
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

            /*
            private bool CanReplaceHand(ArrayHand<T, U> hand) {
                int handLen = hand.Size();

                if ((this.Size() != this.MaxSize) || // The set has to have MaxSize cards in it.
                    (handLen > this.Wilds.Count) ||
                    (hand.NumWc > 0)) // No wc allowed in hand.
                {
                    return false;
                }

                for (int i = 0; i < handLen; i++) {
                    ICard<T, U> c = (NaturalCard<T, U>)hand.GetAt(i);

                    if (c.CompareRank(this.First) != 0) {
                        return false;
                    }

                    for (int j = i + 1; j < handLen; j++) {
                        if (c.CompareTo(hand.GetAt(j)) == 0) {
                            return false;
                        }
                    }

                    if (this.Cards.Contains(c)) {
                        return false;
                    }
                }

                return true;

                public void Replace(ICard<T, U> card, int _) {
                if (!this.CanReplaceHand(card)) {
                throw new ArgumentException("Invalid replacement to set.");
                }

                for (int i = 0; i < card.Size(); i++) {
                this.Cards.AddLast((NaturalCard<T, U>)card.GetAt(i));
                this.Wilds.RemoveFirst();
                }
                }
            }
            */

            public WildCard<T, U> Replace(ICard<T, U> card, int _) {
                if ((this.Size() != this.Rules.MaxSetLen) || // The set has to have MaxSize cards in it.
                    (this.Wilds.Count > 0) ||
                    (card.IsWild()))
                {
                    throw new ArgumentException();
                }

                NaturalCard<T, U> newCard = (NaturalCard<T, U>)card;
                if ((newCard.CompareRank(this.First) != 0) ||
                    (this.Cards.Contains(newCard)))
                    {
                        throw new ArgumentException();
                    }

                if (this.Wilds.First == null) {
                    throw new Exception();
                }
                this.Cards.AddLast(newCard);
                WildCard<T, U> ret = this.Wilds.First.Value;
                this.Wilds.RemoveFirst();

                return ret;
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
