namespace Domain;

public enum MoveKind {
    STOCK,
    DISCARD,
    RUN,
    SET,
    LAY_OFF,
    REPLACE,
    SHED,
    EMPTY
}

public class ResultMove<T> {
    public MoveKind Move { get; }
    public List<T> CardsMoved { get; }
    public int? MeldAffected { get; }
    public int? CardAffected { get; }

    public ResultMove(MoveKind move, List<T> cards, int? meld, int? card) {
        this.Move = move;
        this.CardsMoved = cards;
        this.MeldAffected = meld;
        this.CardAffected = card;
    }
}

/*
public interface IPlayerMove<T, U>
    where T : Scale, new() where U : Scale, new()
    {
        List<ICard<T, U>> CardsMoved();
        int? MeldAffected();
        int? CardAffected();
    }

public class PlayerMovePick<T, U> : IPlayerMove
    where T : Scale, new() where U : Scale, new()
    {
    
    }

public class PlayerMovePlay<T, U> : IPlayerMove
    where T : Scale, new() where U : Scale, new()
    {
    
    }

public class PlayerMoveDisc<T, U> : IPlayerMove
    where T : Scale, new() where U : Scale, new()
    {
    
    }

public class PlayerMoveNone<T, U> : IPlayerMove
    where T : Scale, new() where U : Scale, new()
    {
    
    }
*/
