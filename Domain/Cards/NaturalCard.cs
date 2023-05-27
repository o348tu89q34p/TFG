namespace Domain;

public class NaturalCard<T, U> : ICard<T, U>
    where T : Scale, new() where U : Scale, new()
    {
        public NaturalField<T> Suit { get; private set; }
        public NaturalField<U> Rank { get; private set; }

        public NaturalCard(NaturalField<T> suit, NaturalField<U> rank) {
            this.Suit = suit;
            this.Rank = rank;
        }

        public bool IsFirst() {
            return (this.Suit.IsFirst() &&
                    this.Rank.IsFirst());
        }

        public bool IsLast() {
            return (this.Suit.IsLast() &&
                    this.Rank.IsLast());
        }

        public bool IsWithin(NaturalCard<T, U> a, NaturalCard<T, U> b) {
            if ((this.GetSuit().CompareTo(a.GetSuit()) != 0) ||
                (this.GetSuit().CompareTo(b.GetSuit()) != 0))
            {
                return false;
            }
            return this.GetRank().IsWithin(a.GetRank(), b.GetRank());
        }

        public bool IsNatural() {
            return true;
        }

        public bool IsWild() {
            return false;
        }

        public NaturalField<T> GetSuit() {
            return this.Suit;
        }

        public NaturalField<U> GetRank() {
            return this.Rank;
        }

        public void Next(bool wrap, bool inSuit) {
            if (this.Rank.IsLast() && !wrap && inSuit) {
                throw new Exception("No wrap in suit.");
            }
            if (this.IsLast() && !wrap) {
                throw new Exception("Cannot prev first card without wrap.");
            }

            if (this.Rank.IsLast() && !inSuit) {
                this.Suit.Next();
            }

            this.Rank.Next();
        }

        public void Prev(bool wrap, bool inSuit) {
            if (this.Rank.IsFirst() && !wrap && inSuit) {
                throw new Exception("No wrap in suit.");
            }
            if (this.IsFirst() && !wrap) {
                throw new Exception("Cannot prev first card without wrap.");
            }

            if (this.Rank.IsFirst() && !inSuit) {
                this.Suit.Prev();
            }

            this.Rank.Prev();
        }

        public int CompareTo(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return 1;
            }

            var nc = (NaturalCard<T, U>)c;
            int res = this.Suit.CompareTo(nc.Suit);
            if (res == 0) {
                return this.Rank.CompareTo(nc.Rank);
            }

            return res;
        }

        public int CompareRank(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                throw new ArgumentException("Cannot compare a natural's rank with a wild card.");
            }

            return c.GetRank().CompareTo(c.GetRank());
        }

        public int CompareSuit(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                throw new ArgumentException("Cannot compare a natural's suit with a wild card.");
            }

            return c.GetSuit().CompareTo(c.GetSuit());
        }

        public bool Equals(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return false;
            }

            var nc = (NaturalCard<T, U>)c;
            return (this.Suit.CompareTo(nc.Suit) == 0 &&
                    this.Rank.CompareTo(nc.Rank) == 0);
        }

        public (int, int) Coords() {
            return (this.GetSuit().Position(), this.GetRank().Position());
        }

        public void Print() {
            Console.WriteLine("{0} of {1}",
                              arg0: this.GetRank().ToString(),
                              arg1: this.GetSuit().ToString());
        }
    }
