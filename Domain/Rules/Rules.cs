namespace Domain;

public enum DeckType {
    SPANISH_DECK,
    FRENCH_DECK
}

public class Rules<T, U> where T : Scale, new() where U : Scale, new()
{
    public MeldRules MeldR { get; }
    public PlayerRules PlayerR { get; }

    public int NumPlayers { get; }
    public int NumWc { get; }
    public int NumDecks { get; }
    public int LimitRounds { get; }

    public bool EndDiscard { get; }

    public DeckType Kind { get; }

    public Rules(int numSuits, int numRanks,
                 int numPlayers, int numDecks, int numWc, int numCards,
                 bool canWrap, bool multWc, bool consecWc,
                 bool needsOut, int minRunLen, int minSetLen,
                 bool endDiscard, DeckType kind)
    {
        int margin = 1; // At least one card at the stock.
        if (!needsOut) {
            margin++; // At least one card at the discard pile.
        }

        if (((numSuits*numRanks)*numDecks + numWc) < (numCards*numPlayers + margin)) {
            throw new Exception("Not enough cards for each player.");
        }

        if (minRunLen < 0 || minRunLen > numRanks) {
            throw new Exception("Invalid minimum run length.");
        }

        if (minSetLen < 0 || minSetLen > numSuits) {
            throw new Exception("Invalid minimum set length.");
        }

        if (minSetLen > numCards || minRunLen > numCards) {
            throw new Exception("Some melds can never be formed.");
        }

        this.PlayerR = new PlayerRules(numCards, needsOut);
        this.MeldR = new MeldRules(canWrap, multWc, consecWc, numRanks, numSuits, minRunLen, minSetLen);

        this.NumPlayers = numPlayers;
        this.NumDecks = numDecks;
        this.NumWc = numWc;

        this.LimitRounds = -1;

        this.EndDiscard = endDiscard;

        this.Kind = kind;
    }
}
