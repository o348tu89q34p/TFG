namespace Domain {
    public class PlayerRules {
        // The number of cards given to a player at the start of a game.
        public int NumCards { get; }

        // A player must have come out in order to pick from discard.
        public bool NeedsOut { get; }

        public PlayerRules(int numCards, bool needsOut) {
            this.NumCards = numCards;
            this.NeedsOut = needsOut;
        }
    }
}
