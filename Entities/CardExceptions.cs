using System;

namespace Entities {

/*
 * Exception thrown when an operation is called on a card that
 * does not admit that opreation.
 */
public class CardBadKindException : Exception {
    public CardBadKindException() {
    }

    public CardBadKindException(string i) :
        base(String.Format("Invalid card type for: {0}", i))
    {
    }
}

public class CardOperationException : Exception {
    public CardOperationException() {
    }

    public CardOperationException(string i) :
        base(String.Format("Current state does not support: {0}", i))
    {
    }
}

/*
 * Exception thrown when a card is found to be in an inconsistent
 * internal state.
 */
public class CardBadStateException : Exception {
    public CardBadStateException() {
    }

    public CardBadStateException(string i) :
        base(String.Format("Internal representation inconsistent at: {0}", i))
    {
    }
}
}
