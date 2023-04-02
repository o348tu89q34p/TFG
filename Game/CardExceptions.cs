using System;

namespace Game;

public class BadCardException : Exception {
    public BadCardException() {
    }

    public BadCardException(string i) :
        base(String.Format("Badly formed card at: {0}", i))
    {
    }
}

public class InvalidCardException : Exception {
    public InvalidCardException() {
    }

    public InvalidCardException(string i) :
        base(String.Format("Invalid card type for: {0}", i))
    {
    }
}

/*
 * Exception thrown when an operation is called on a card that
 * does not admit that opreation.
 */
public class CardOperationException : Exception {
    public CardOperationException() {
    }

    public CardOperationException(string i) :
        base(String.Format("Invalid card operation for: {0}", i))
    {
    }
}

public class CardBadStateException : Exception {
    public CardBadStateException() {
    }

    public CardBadStateException(string i) :
        base(String.Format("Card in invalid state at: {0}", i))
    {
    }
}
