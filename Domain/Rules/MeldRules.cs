namespace Domain {
    public class MeldRules {
        public bool CanWrap { get; }
        public bool MultWc { get; }
        public bool ConsecWc { get; }
        public int MaxRunLen { get; }
        public int MaxSetLen { get; }
        public int MinRunLen { get; }
        public int MinSetLen { get; }

        public MeldRules(bool canWrap, bool multWc, bool consecWc,
                         int maxRunLen, int maxSetLen,
                         int minRunLen, int minSetLen) {
            this.CanWrap = canWrap;
            this.MultWc = multWc;
            this.ConsecWc = consecWc;
            this.MaxRunLen = maxRunLen;
            this.MaxSetLen = maxSetLen;
            this.MinRunLen = minRunLen;
            this.MinSetLen = minSetLen;
        }
    }
}
