namespace Domain {
    public class WildCard<T, U> : ICard<T, U>
        where T : Scale, new()
        where U : Scale, new()
        {
            public bool IsFirst() {
                throw new Exception();
            }

            public bool IsLast() {
                throw new Exception();
            }

            public bool IsWithin(NaturalCard<T, U> a, NaturalCard<T, U> b) {
                throw new Exception();
            }

            public bool IsNatural() {
                return false;
            }

            public bool IsWild() {
                return true;
            }

            public NaturalField<T> GetSuit() {
                throw new Exception();
            }

            public NaturalField<U> GetRank() {
                throw new Exception();
            }

            public void Next(bool wrap, bool inSuit) {
                throw new Exception();
            }

            public void Prev(bool wrap, bool inSuit) {
                throw new Exception();
            }

            public int CompareTo(ICard<T, U> c) {
                if (c.IsWild()) {
                    return 0;
                }

                return 1;
            }

            public int CompareRank(ICard<T, U> c) {
                if (c.IsWild()) {
                    return 0;
                }

                return 1;
            }

            public int CompareSuit(ICard<T, U> c) {
                if (c.IsWild()) {
                    return 0;
                }

                return 1;
            }

            public int SortCompareRank(ICard<T, U> c) {
                if (c.IsWild()) {
                    return 0;
                }

                return 1;
            }

            public int SortCompareSuit(ICard<T, U> c) {
                if (c.IsWild()) {
                    return 0;
                }

                return 1;
            }

            public bool Equals(ICard<T, U> c) {
                return this.IsWild() && c.IsWild();
            }

            public (int, int) Coords() {
                throw new Exception();
            }

            public void Print() {
                Console.WriteLine("Wildcard");
            }
        }
}
