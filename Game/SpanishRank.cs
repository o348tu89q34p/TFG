public class SpanishRank {
    private Rank _rank;

    /// <summary>Possible values a rank can take for a Spanish card.</summary>
    private enum Rank {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Knave,
        Knight,
        King
    }

    /// <param name=n>Human readable name for a rank.</param>
    /// <returns>Card rank that represents the rank indicated by the name received.</returns>
    /// <exception cref="ArgumentException">The name provided does not represent a valid rank in the Spanish rank.</exception>
    public SpanishRank(string name) {
        if (Enum.TryParse(name, out Rank rank)) {
            this.SetRank(rank);
        } else {
            throw new ArgumentException();
        }
    }

    public SpanishRank(int i) {
        if (i < 0 || i >= SpanishRank.GetNumRanks()) {
            throw new ArgumentException();
        }

        this.SetRank((Rank)i);
    }

    private Rank GetRank() {
        return _rank;
    }

    private void SetRank(Rank rank) {
        _rank = rank;
    }

    public static int GetNumRanks() {
        return Enum.GetNames(typeof(Rank)).Length;
    }

    public string GetName() {
        return this.GetRank().ToString();
    }

    public int CompareTo(SpanishRank s) {
        return this.GetRank().CompareTo(s.GetRank());
    }

    // These transform this.
    public void NextRank() {
        // Consider rising an exception if last.
        int thisRank = this.ToInt();

        if (thisRank >= SpanishRank.GetNumRanks()) {
            return;
        }

        this.SetRank((Rank)(thisRank + 1));
    }

    public void NextRankWrap() {
        int thisRank = this.ToInt();
        int nextRank = (thisRank + 1) % SpanishRank.GetNumRanks();

        this.SetRank((Rank)nextRank);
    }

    public bool IsNextRank(SpanishRank s) {
        int thisRank = this.ToInt();
        int thatRank = this.ToInt();

        return (thisRank + 1)  == thatRank;
    }

    public bool IsNextRankWrap(SpanishRank s) {
        int thisRank = this.ToInt();
        int thatRank = this.ToInt();
        int numRanks = SpanishRank.GetNumRanks();

        return ((thisRank + 1) % numRanks) == thatRank;
    }

    private bool IsNthRank(int n) {
        return this.ToInt() == n;
    }

    public bool IsFirstRank() {
        return this.IsNthRank(0);
    }

    public bool IsLastRank() {
        return this.IsNthRank(SpanishRank.GetNumRanks() - 1);
    }

    private int ToInt() {
        return Convert.ToInt32(this.GetRank());
    }

    public static SpanishRank FirstRank() {
        return new SpanishRank(0);
    }

    public static SpanishRank LastRank() {
        int last = GetNumRanks() - 1;

        return new SpanishRank(last);
    }
}
