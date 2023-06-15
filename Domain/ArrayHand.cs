namespace Domain;

public class ArrayHand<T, U> where T : Scale, new() where U : Scale, new()
{
    public List<ICard<T, U>> Hand { get; private set; }
    private int Capacity { get; set; }
    public int NumWc { get; private set; }
    public bool HasConsecWc { get; private set; }

    public ArrayHand(int n) {
        this.Hand = new List<ICard<T, U>>(n);
        this.Capacity = n;
        this.NumWc = 0;
        this.HasConsecWc = false;
    }

    public int Size() {
        return this.Hand.Count;
    }

    public bool IsEmpty() {
        return this.Hand.Count == 0;
    }

    public bool IsFull() {
        return this.Hand.Count == this.Capacity;
    }

    /*
     * Given the position of a card within the hand,
     * determine if it creates a consecutive wild
     * cards situation.
     */
    private void UpdateConsecWc(int pos) {
        ICard<T, U> c = this.GetAt(pos);
        if (c.IsNatural() || this.Size() <= 1) {
            return;
        }

        bool aux = this.HasConsecWc;
        if (pos == 0) {
            aux = aux || this.GetAt(pos + 1).IsWild();
        } else if (pos == (this.Size() - 1)) {
            aux = aux || this.GetAt(pos - 1).IsWild();
        } else {
            aux = (aux ||
                   this.GetAt(pos - 1).IsWild() ||
                   this.GetAt(pos + 1).IsWild());
        }

        this.HasConsecWc = aux;
    }

    // Check if pos is a valid index for this hand.
    private void CheckPos(int pos, string loc) {
        if (pos < 0) {
            throw new ArgumentException($"Negative position at {loc}.");
        } else if (pos >= this.Size()) {
            throw new ArgumentException($"Position greater than the size at {loc}.");
        }
    }

    public ICard<T, U> ReplaceAt(int pos, ICard<T, U> c) {
        this.CheckPos(pos, "ReplaceAt");

        ICard<T, U> target = this.GetAt(pos);
        if (!target.IsWild() && c.IsWild()) {
            this.NumWc++;
        } else if (target.IsWild() && !c.IsWild()) {
            this.NumWc--;
        }

        this.UpdateConsecWc(pos);
        this.Hand[pos] = c;

        return target;
    }

    public void Append(ICard<T, U> c) {
        if (this.IsFull()) {
            throw new Exception("Full hand.");
        }

        if (c.IsWild()) {
            this.NumWc++;
        }

        this.Hand.Add(c);
        this.UpdateConsecWc(this.Hand.Count - 1);
    }

    public ICard<T, U> GetAt(int pos) {
        this.CheckPos(pos, "GetAt");

        return this.Hand[pos];
    }

    public void RemoveAt(int pos) {
        this.CheckPos(pos, "RemoveAt");

        if (this.GetAt(pos).IsWild()) {
            this.NumWc--;
        }

        this.Hand.RemoveAt(pos);

        if (this.IsEmpty()) {
            this.HasConsecWc = false;
        } else if (pos == this.Size()) { // Before remove.
            this.UpdateConsecWc(pos - 1);
        } else {
            this.UpdateConsecWc(pos);
        }
    }

    public void Shuffle() {
        int n = this.Size();
        int r;
        Random rnd = new Random();

        for (int i = 0; i < n; i++) {
            r = rnd.Next(i, n);
            this.Exchange(i, r);
            if (!this.HasConsecWc && i > 0) {
                //this.HasConsecWc = CheckIfConsecWc(i, i - 1);
                this.HasConsecWc = (this.HasConsecWc ||
                                    (this.GetAt(i).IsWild() &&
                                     this.GetAt(i - 1).IsWild()));
            }
        }
    }

    public void Reverse() {
        for (int i = 0; i < this.Hand.Count/2; i++) {
            this.Exchange(i, this.Hand.Count - 1 - i);
        }
    }

    public void Exchange(int a, int b) {
        int max = this.Size();
        if (a < 0 || b < 0 || a >= max || b >= max) {
            throw new ArgumentException("Invalid exchange ranges.");
        }

        ICard<T, U> aux = this.GetAt(a);
        this.ReplaceAt(a, this.GetAt(b));
        this.ReplaceAt(b, aux);
    }

    // Starting from position 0, return the first card that is not wild.
    public (NaturalCard<T, U>?, int) FirstNatural() {
        if (this.NumWc == this.Size()) {
            return (null, 0);
        }

        for (int i = 0; i < this.Size(); i++) {
            ICard<T, U> current = this.GetAt(i);
            if (current.IsNatural()) {
                return ((NaturalCard<T, U>)current, i);
            }
        }

        return (null, 0);
    }

    public void Print() {
        for (int i = 0; i < this.Hand.Count; i++) {
            Console.Write($"{i} - ");
            this.Hand.ElementAt(i).Print();
        }
    }

    public void SortRuns() {
        this.Hand.Sort((x, y) => x.SortCompareSuit(y));
    }

    public void SortSets() {
        this.Hand.Sort((x, y) => x.SortCompareRank(y));
    }
}
