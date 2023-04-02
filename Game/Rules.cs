namespace Game;

public class Rules {
    /* The player has to have a meld on the board to be able to pick a card form the discard pile. */
    private bool _needsOpen;
    /* The player can only get rid of their last card via discarding it, but not by melding or laying off. */
    private bool _discardLast;
    /* A playr can lay off the corresponding card in place of a joker to pick up the joker. */
    private bool _canTakeJoker;

    public Rules() {
    }
}
