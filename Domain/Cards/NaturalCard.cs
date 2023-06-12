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

        // Don't need this.
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
            if (this.IsLast() && wrap) {
                this.Rank.Next();
                this.Suit.Next();

                return;
            }
            if (this.Rank.IsLast() && inSuit) {
                this.Rank.Next();

                return;
            } else if (this.Rank.IsLast() && !inSuit) {
                this.Rank.Next();
                this.Suit.Next();
            }

            this.Rank.Next();
        }

        public void Prev(bool wrap, bool inSuit) {
            if (this.IsFirst() && wrap) {
                this.Rank.Prev();
                this.Suit.Prev();

                return;
            }
            if (this.Rank.IsFirst() && inSuit) {
                this.Rank.Prev();

                return;
            } else if (this.Rank.IsFirst() && !inSuit) {
                this.Rank.Prev();
                this.Suit.Prev();
            }

            this.Rank.Prev();
        }

        public int CompareTo(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return -1;
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
                return -1;
            }

            var nc = (NaturalCard<T, U>)c;
            return this.GetRank().CompareTo(nc.GetRank());
        }

        public int CompareSuit(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return -1;
            }

            var nc = (NaturalCard<T, U>)c;
            return this.GetSuit().CompareTo(nc.GetSuit());
        }

        public int SortCompareRank(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return -1;
            }

            var nc = (NaturalCard<T, U>)c;
            int res = this.GetRank().CompareTo(nc.GetRank());
            if (res == 0) {
                return this.GetSuit().CompareTo(nc.GetSuit());
            }

            return res;
        }

        public int SortCompareSuit(ICard<T, U> c) {
            if (c is WildCard<T, U>) {
                return -1;
            }

            var nc = (NaturalCard<T, U>)c;
            int res = this.GetSuit().CompareTo(nc.GetSuit());
            if (res == 0) {
                return this.GetRank().CompareTo(nc.GetRank());
            }

            return res;
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
