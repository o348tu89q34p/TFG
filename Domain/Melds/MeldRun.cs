namespace Domain {
    public class MeldRun<T, U> : IMeld<T, U>
        where T : Scale, new()
        where U : Scale, new()
        {
            private LinkedList<ICard<T, U>> Cards { get; set; }
            private NaturalCard<T, U> First { get; set; }
            private NaturalCard<T, U> Last { get; set; }
            private MeldRules Rules { get; }
            private int NumWc { get; set; }

            public MeldRun(ArrayHand<T, U> hand, MeldRules rules) {
                this.Rules = rules;
                var (first, pos) = hand.FirstNatural();
                if (first == null) {
                    throw new ArgumentException("There are no natural cards.");
                }
                if (!rules.ConsecWc && hand.HasConsecWc) {
                    throw new ArgumentException("No consecutive wild cards allowed.");
                }
                if (hand.Size() < rules.MinRunLen) {
                    throw new ArgumentException("There are too few cards.");
                }

                (NaturalCard<T, U>? low, NaturalCard<T, U>? high) = this.RunEdges(hand);
                if (low == null || high == null) {
                    throw new ArgumentException("The cards do not form a consecutive run.");
                }
                this.First = low;
                this.Last = high;
                /*
                NaturalCard<T, U> firstCard = (NaturalCard<T, U>)ICard<T, U>.Copy(first);
                for (int i = pos - 1; i >= 0; i--) {
                    ICard<T, U> aux = hand.GetAt(i);
                    firstCard.Prev(false, rules.CanWrap);
                    if (aux.IsNatural() && !firstCard.Equals(aux)) {
                        throw new ArgumentException("Invalid run.");
                    }
                }
                this.First = firstCard;

                NaturalCard<T, U> lastCard = (NaturalCard<T, U>)ICard<T, U>.Copy(first);
                for (int i = pos + 1; i < hand.Size(); i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    lastCard.Next(false, rules.CanWrap);
                    if (aux.IsNatural() && !lastCard.Equals(aux)) {
                        throw new ArgumentException("Invalid run.");
                    }
                }
                this.Last = lastCard;
                */

                this.Cards = new LinkedList<ICard<T, U>>();
                for (int i = 0; i < hand.Size(); i++) {
                    this.Cards.AddLast(hand.GetAt(i));
                }
                this.NumWc = hand.NumWc;
            }

            private (NaturalCard<T, U>?, NaturalCard<T, U>?) RunEdges(ArrayHand<T, U> hand) {
                var (first, pos) = hand.FirstNatural();
                if ((first == null) ||
                    (!this.Rules.ConsecWc && hand.HasConsecWc))
                {
                    return (null, null);
                }

                NaturalCard<T, U> firstCard = (NaturalCard<T, U>)ICard<T, U>.Copy(first);
                for (int i = pos - 1; i >= 0; i--) {
                    ICard<T, U> aux = hand.GetAt(i);
                    firstCard.Prev(false, this.Rules.CanWrap);
                    if (aux.IsNatural() && !firstCard.Equals(aux)) {
                        return (null, null);
                    }
                }

                NaturalCard<T, U> lastCard = (NaturalCard<T, U>)ICard<T, U>.Copy(first);
                for (int i = pos + 1; i < hand.Size(); i++) {
                    ICard<T, U> aux = hand.GetAt(i);
                    lastCard.Next(false, this.Rules.CanWrap);
                    if (aux.IsNatural() && !lastCard.Equals(aux)) {
                        return (null, null);
                    }
                }

                return (firstCard, lastCard);
            }

            delegate LinkedListNode<ICard<T, U>> Adder(ICard<T, U> c);

            public void Add(ArrayHand<T, U> hand) {
                if (this.Cards.Count + hand.Size() > this.Rules.MaxRunLen) {
                    throw new ArgumentException("The cards do not fit in the run.");
                }
                if (!this.Rules.MultWc && (this.NumWc + hand.NumWc) > 1) {
                    throw new ArgumentException("The cards make the set have multiple wild cards.");
                }

                var (first, last) = this.RunEdges(hand);
                if (first == null || last == null) {
                    throw new ArgumentException("No natural cards in hand.");
                }

                // Watch.
                try {
                    last.Next(false, this.Rules.CanWrap);
                    first.Prev(false, this.Rules.CanWrap);
                } catch {
                    throw new ArgumentException("The wrapping case.");
                }
                Adder adder;
                if (this.First.CompareTo(last) == 0) {
                    adder = this.Cards.AddFirst;
                    hand.Reverse();
                } else if (this.Last.CompareTo(first) == 0) {
                    adder = this.Cards.AddLast;
                } else {
                    throw new ArgumentException("Hand not contiguous to meld.");
                }

                for (int i = 0; i < hand.Size(); i++) {
                    ICard<T, U> c = hand.GetAt(i);
                    adder(c);
                    if (c.IsWild()) {
                        this.NumWc =+ 1;
                    }
                }
            }

            public WildCard<T, U> Replace(ICard<T, U> card, int pos) {
                if (card.IsWild()) {
                    throw new ArgumentException("A wild card cannot be a replacement.");
                }
                if (this.NumWc == 0) {
                    throw new ArgumentException("There are no wild cards to replace.");
                }
                if (pos < 0 || pos >= this.Cards.Count) {
                    throw new Exception("Invalid index.");
                }

                NaturalCard<T, U>guide = (NaturalCard<T, U>)ICard<T, U>.Copy(this.First);
                LinkedListNode<ICard<T, U>>? pointer = this.Cards.First;
                for (int i = 0; i < pos; i++) {
                    guide.Next(false, this.Rules.CanWrap);
                    if (pointer == null) {
                        throw new Exception();
                    }
                    pointer = pointer.Next;
                }

                if (!card.Equals(guide)) {
                    throw new ArgumentException("The replacement does not correspond with the right card.");
                }
                if (pointer == null) {
                    throw new Exception("Bad pointer.");
                }
                if (pointer.Value.IsNatural()) {
                    throw new ArgumentException("The card to be replaced is not a wild card.");
                }

                WildCard<T, U> current = (WildCard<T, U>)pointer.Value;
                pointer.Value = card;

                return current;
            }

            public List<ICard<T, U>> GetCards() {
                return this.Cards.ToList();
            }

            public void Print() {
                foreach (ICard<T, U> c in this.Cards) {
                    c.Print();
                }
            }

            public string Type() {
                return "Run";
            }
        }
}
