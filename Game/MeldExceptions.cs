using System;

namespace Game;


// Safeguard for when an enum is given more values.
public class UnimplementedStateException : Exception {
    public UnimplementedStateException() {
    }

    public UnimplementedStateException(string i) :
        base(String.Format("The meld is in an inconsistent state: {0}", i))
    {
    }
}

public class MeldInvalidStateException : Exception {
    public MeldInvalidStateException() {
    }

    public MeldInvalidStateException(string i) :
        base(String.Format("The meld is in an inconsistent state: {0}", i))
    {
    }
}

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
