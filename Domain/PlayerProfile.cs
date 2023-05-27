namespace Domain;

public class PlayerProfile {
    public string Name { get; }
    public int NumCards { get; }

    public PlayerProfile(string name, int numCards) {
        this.Name = name;
        this.NumCards = numCards;
    }
}
