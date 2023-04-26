using System;

namespace Domain {

public class NegativeIndexException : Exception {
    public NegativeIndexException() {
    }

    public NegativeIndexException(string i) :
        base(String.Format("Invalid index when: {0}", i))
    {
    }
}

public class IndexOverflowException : Exception {
    public IndexOverflowException() {
    }

    public IndexOverflowException(string i) :
        base(String.Format("Invalid index when: {0}", i))
    {
    }
}

public class NegativeSizeException : Exception {
    public NegativeSizeException() {
    }

    public NegativeSizeException(string i) :
        base(String.Format("Negative size when creating: {0}", i))
    {
    }
}

public class EmptyHandException : Exception {
    public EmptyHandException() {
    }

    public EmptyHandException(string i) :
        base(String.Format("Invalid operation on an empty hand: {0}", i))
    {
    }
}

public class FullHandException : Exception {
    public FullHandException() {
    }

    public FullHandException(string i) :
        base(String.Format("Invalid operation on a full hand: {0}", i))
    {
    }
}
}
