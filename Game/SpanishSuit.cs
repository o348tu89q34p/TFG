using System;

public class SpanishSuit {
    private Suit _suit;

    /// <summary>Possible values a suit can take for a Spanish card.</summary>
    private enum Suit {
        Swords,
        Cups,
        Coins,
        Clubs
    }

    /// <param name=n>Human readable name for a suit.</param>
    /// <returns>Card suit that represents the suit indicated by the name received.</returns>
    /// <exception cref="ArgumentException">The name provided does not represent a valid suit in the Spanish suit.</exception>
    public SpanishSuit(string name) {
        if (Enum.TryParse(name, out Suit suit)) {
            this.SetSuit(suit);
        } else {
            throw new ArgumentException();
        }
    }

    public SpanishSuit(int i) {
        if (i < 0 || i >= SpanishSuit.GetNumSuits()) {
            throw new ArgumentException();
        }

        this.SetSuit((Suit)i);
    }

    private Suit GetSuit() {
        return _suit;
    }

    private void SetSuit(Suit suit) {
        _suit = suit;
    }

    public static int GetNumSuits() {
        return Enum.GetNames(typeof(Suit)).Length;
    }

    public string GetName() {
        return this.GetSuit().ToString();
    }

    public int CompareTo(SpanishSuit s) {
        return this.GetSuit().CompareTo(s.GetSuit());
    }

    // These transform this.
    public void NextSuit() {
        // Consider rising an exception if last.
        int thisSuit = this.ToInt();

        if (thisSuit >= SpanishSuit.GetNumSuits()) {
            return;
        }

        this.SetSuit((Suit)(thisSuit + 1));
    }

    public void NextSuitWrap() {
        int thisSuit = this.ToInt();
        int nextSuit = (thisSuit + 1) % SpanishSuit.GetNumSuits();

        this.SetSuit((Suit)nextSuit);
    }

    public bool IsNextSuit(SpanishSuit s) {
        int thisSuit = this.ToInt();
        int thatSuit = this.ToInt();

        return (thisSuit + 1)  == thatSuit;
    }

    public bool IsNextSuitWrap(SpanishSuit s) {
        int thisSuit = this.ToInt();
        int thatSuit = this.ToInt();
        int numSuits = SpanishSuit.GetNumSuits();

        return ((thisSuit + 1) % numSuits) == thatSuit;
    }

    private bool IsNthSuit(int n) {
        return this.ToInt() == n;
    }

    public bool IsFirstSuit() {
        return this.IsNthSuit(0);
    }

    public bool IsLastSuit() {
        return this.IsNthSuit(SpanishSuit.GetNumSuits() - 1);
    }

    private int ToInt() {
        return Convert.ToInt32(this.GetSuit());
    }

    public static SpanishSuit FirstSuit() {
        return new SpanishSuit(0);
    }

    public static SpanishSuit LastSuit() {
        int last = GetNumSuits() - 1;

        return new SpanishSuit(last);
    }
}
