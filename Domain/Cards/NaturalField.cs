namespace Domain {
    public class NaturalField<T> where T : Scale, new() {
        private int _pointer;

        public NaturalField(int pointer) {
            if (pointer >= LenData()) {
                throw new ArgumentException("Invalid value.");
            }
            this._pointer = pointer;
        }

        public static int LenData() {
            return new T().Data.Length;
        }

        public int NumFields() {
            return LenData();
        }

        private int NextVal() {
            return (this._pointer + 1)%LenData();
        }

        private int PrevVal() {
            return (this._pointer - 1 + LenData())%LenData();
        }

        public int Position() {
            return this._pointer;
        }

        public bool IsWithin(NaturalField<T> a, NaturalField<T> b) {
            return ((this._pointer >= a._pointer) &&
                    (this._pointer <= b._pointer));
        }

        public void Next() {
            this._pointer = this.NextVal();
        }

        public void Prev() {
            this._pointer = this.PrevVal();
        }

        public bool IsNext(NaturalField<T> nf) {
            return this.NextVal() == nf._pointer;
        }

        public bool IsPrev(NaturalField<T> nf) {
            return this.PrevVal() == nf._pointer;
        }

        public bool IsFirst() {
            return this._pointer == 0;
        }

        public bool IsLast() {
            return this._pointer == (LenData() - 1);
        }

        public int CompareTo(NaturalField<T> nf) {
            return this._pointer.CompareTo(nf._pointer);
        }

        public bool Equals(NaturalField<T> nf) {
            return this._pointer == nf._pointer;
        }

        public override string ToString() {
            return (new T().Data[this._pointer]);
        }

        public static NaturalField<T> First() {
            return new NaturalField<T>(0);
        }

        public static NaturalField<T> Last() {
            return new NaturalField<T>(LenData());
        }

        public static NaturalField<T> Copy(NaturalField<T> field) {
            return new NaturalField<T>(field._pointer);
        }
    }
}
