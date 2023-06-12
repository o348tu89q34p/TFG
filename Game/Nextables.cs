namespace Game {
    public interface Nextable<T> {
        void Next();
        void Prev();
        T GetIt();
        string ToString();
    }

    public class NextableInt : Nextable<int> {
        private int Val { get; set; }
        private int Min { get; }
        private int Dif { get; }

        public NextableInt(int min, int max, int val) {
            this.Val = val - min;
            this.Min = min;
            this.Dif = (max - min) + 1;
        }

        public void Next() {
            this.Val = (this.Val + 1)%this.Dif;
        }

        public void Prev() {
            this.Val = (this.Val - 1 + this.Dif)%this.Dif;
        }

        public int GetIt() {
            return this.Val + this.Min;
        }

        public override string ToString() {
            return (this.Val + this.Min).ToString();
        }
    }

    public class NextableBool : Nextable<Boolean> {
        private bool Val { get; set; }

        public NextableBool(bool val) {
            this.Val = val;
        }

        public void Next() {
            this.Val = !this.Val;
        }

        public void Prev() {
            this.Val = !this.Val;
        }

        public bool GetIt() {
            return this.Val;
        }

        public override string ToString() {
            if (this.Val) {
                return "Yes";
            } else {
                return "No";
            }
        }
    }

    public class NextableStrings : Nextable<string> {
        private string[] Val { get; }
        private int Pointer { get; set; }

        public NextableStrings(string[] val, int pointer) {
            this.Val = val;
            this.Pointer = pointer;
        }

        public void Next() {
            this.Pointer = (this.Pointer + 1)%this.Val.Length;
        }

        public void Prev() {
            this.Pointer = (this.Pointer - 1 + this.Val.Length)%this.Val.Length;
        }

        public string GetIt() {
            return this.Val[this.Pointer];
        }

        public override string ToString() {
            return this.Val[this.Pointer];
        }
    }
}
