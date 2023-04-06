using System;

namespace Game;

public class BadOperationException : Exception {
    public BadOperationException() {
    }

    public BadOperationException(string i) :
        base(String.Format("The state of the meld doesn't admit this operation: {0}", i))
    {
    }
}

public class EmptyMeldException : Exception {
    public EmptyMeldException() {
    }

    public EmptyMeldException(string i) :
        base(String.Format("Operation on an empty meld: {0}", i))
    {
    }
}
