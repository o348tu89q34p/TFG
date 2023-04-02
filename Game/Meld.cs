namespace Game;

public class Meld<T> where T : class {
    private T[] _meld;
    private MeldType _type;

    public Meld(MeldType type, int n) {
        this._meld = new T[n];
        this.SetMeldType(type);
    }

    private MeldType GetMeldType() {
        return this._type;
    }

    private void SetMeldType(MeldType type) {
        this._type = type;
    }

    public bool IsSet() {
        return this.GetMeldType() == MeldType.Set;
    }

    public bool IsRun() {
        return this.GetMeldType() == MeldType.Run;
    }

    // Check a card at a given position.
    public void CheckCard(int pos) {
    }
}
