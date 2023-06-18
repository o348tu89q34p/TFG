namespace Domain;

public class PosCard<T, U> where T : Scale, new() where U : Scale, new()
{
    public ICard<T, U> Card;
    public int Pos;

    public PosCard(ICard<T, U> card, int pos) {
        this.Card = card;
        this.Pos = pos;
    }
}

public class Brain<T, U> where T : Scale, new() where U : Scale, new()
{
    private Rules<T, U> Rules { get; }

    public Brain(Rules<T, U> rules) {
        this.Rules = rules;
    }

    private bool HasCard(ICard<T, U> card, List<PosCard<T, U>> hand) {
        for (int i = 0; i < hand.Count; i++) {
            if (card.Equals(hand[i])) {
                return true;
            }
        }

        return false;
    }

    // Remove all duplicates from a hand.
    private List<PosCard<T, U>> MakeUnique(List<PosCard<T, U>> hand) {
        List<PosCard<T, U>> newHand = new List<PosCard<T, U>>();

        for (int i = 0; i < hand.Count; i++) {
            if (!this.HasCard(hand[i].Card, newHand)) {
                newHand.Add(hand[i]);
            }
        }

        return newHand;
    }

    private List<List<PosCard<T, U>>> GroupByRank(List<PosCard<T, U>> hand) {
        List<List<PosCard<T, U>>> newHand = new List<List<PosCard<T, U>>>();

        int count = 0;
        bool found = false;
        for (int i = 0; i < hand.Count; i++) {
            while (count < newHand.Count && !found) {
                found = hand[i].Card.CompareRank(newHand[count][0].Card) == 0;
                if (!found) {
                    count++;
                }
            }

            if (found) {
                newHand[count].Add(hand[i]);
            } else {
                newHand.Add(new List<PosCard<T, U>>());
                newHand[newHand.Count - 1].Add(hand[i]);
            }

            found = false;
            count = 0;
        }

        return newHand;
    }

    // hand can be sorted or not.
    private List<List<ICard<T, U>>> HandByRank(List<ICard<T, U>> hand) {
        List<List<ICard<T, U>>> ranks = new List<List<ICard<T, U>>>(5);
        for (int i = 0; i < ranks.Count; i++) {
            // Create the lists in the list.
        }

        for (int i = 0; i < hand.Count; i++) {
            ICard<T, U> c = hand[i];
            if (c.IsWild()) {
                ranks[4].Add(c);
            } else {
                ranks[c.GetSuit().Position()].Add(c);
            }
        }

        return ranks;
    }

    // Pair each card with its current position.
    private List<(ICard<T, U>, int)> NumberCards(ArrayHand<T, U> hand) {
        List<(ICard<T, U>, int)> res = new List<(ICard<T, U>, int)>(hand.Size());

        for (int i = 0; i < hand.Size(); i++) {
            res.Add((hand.GetAt(i), i));
        }

        return res;
    }

    private List<(ICard<T, U>, int)> PrimeRun(ArrayHand<T, U> hand) {
        List<(ICard<T, U>, int)> res = this.NumberCards(hand);
        res.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        return res;
    }

    private List<(ICard<T, U>, int)> PrimeSet(ArrayHand<T, U> hand) {
        List<(ICard<T, U>, int)> res = this.NumberCards(hand);
        res.Sort((a, b) => a.Item1.CompareRank(b.Item1));

        return res;
    }

    /*
    private List<int>? FirstSet(ArrayHand<T, U> hand) {
        if (hand.Size() == 0) {
            return null;
        }

        List<(ICard<T, U>, int)> cards = this.PrimeSet(hand);
        List<int> poses = new List<int>();
        ICard<T, U>? pointer = null;
        ICard<T, U>? prev = null;

        for (int i = 1; i < cards.Count; i++) {
            ICard<T, U> c = cards.ElementAt(i).Item1;
            if (c.IsWild()) {
                return null;
            }
            if (pointer == null || prev == null) {
                pointer = ICard<T, U>.Copy(c);
                prev = c;
            } else {
                pointer.Next(true, true);
                if (c.Equals(pointer)) {
                    poses.Add(cards.ElementAt(i).Item2);
                    prev = c;
                } else if (c.Equals(prev)) {
                    pointer.Prev(true, true);
                } else {
                    if (poses.Count >= this.Rules.MeldR.MinRunLen) {
                        return poses;
                    }
                    pointer = ICard<T, U>.Copy(c);
                    prev = c;
                    poses.Clear();
                    poses.Add(cards.ElementAt(i).Item2);
                }
            }
        }

        return null;
    }
    */

    private List<int>? FirstSet(ArrayHand<T, U> hand) {
        if (hand.Size() == 0) {
            return null;
        }

        List<(ICard<T, U>, int)> cards = this.PrimeSet(hand);
        List<int> poses = new List<int>();
        ICard<T, U>? pointer = null;
        ICard<T, U>? prev = null;

        for (int i = 0; i < cards.Count; i++) {
            ICard<T, U> c = cards.ElementAt(i).Item1;
            if (c.IsWild()) {
                return null;
            }
            if (pointer == null || prev == null) {
                pointer = ICard<T, U>.Copy(c);
                prev = c;
            } else {
                pointer.Next(true, true);
                if (c.Equals(pointer)) {
                    poses.Add(cards.ElementAt(i).Item2);
                    prev = c;
                } else if (c.Equals(prev)) {
                    pointer.Prev(true, true);
                } else {
                    if (poses.Count >= this.Rules.MeldR.MinRunLen) {
                        return poses;
                    }
                    pointer = ICard<T, U>.Copy(c);
                    prev = c;
                    poses.Clear();
                    poses.Add(cards.ElementAt(i).Item2);
                }
            }
        }

        return null;
    }

    private List<int>? FirstRun(ArrayHand<T, U> hand) {
        if (hand.Size() == 0) {
            return null;
        }

        List<(ICard<T, U>, int)> cards = this.PrimeRun(hand);
        List<int> poses = new List<int>();
        ICard<T, U>? pointer = null;
        ICard<T, U>? prev = null;

        for (int i = 0; i < cards.Count; i++) {
            ICard<T, U> c = cards.ElementAt(i).Item1;
            if (c.IsWild()) {
                return null;
            }
            if (pointer == null || prev == null) {
                pointer = ICard<T, U>.Copy(c);
                prev = c;
            } else {
                pointer.Next(true, true);
                if (c.Equals(pointer)) {
                    poses.Add(cards.ElementAt(i).Item2);
                    prev = c;
                } else if (c.Equals(prev)) {
                    pointer.Prev(true, true);
                } else {
                    if (poses.Count >= this.Rules.MeldR.MinRunLen) {
                        return poses;
                    }
                    pointer = ICard<T, U>.Copy(c);
                    prev = c;
                    poses.Clear();
                    poses.Add(cards.ElementAt(i).Item2);
                }
            }
        }

        return null;
    }

    public ResultMove<int> MakePick(SinglePlayer<T, U> p) {
        if (this.Rules.PlayerR.NeedsOut && !p.HasComeOut()) {
            return new ResultMove<int>(MoveKind.STOCK, new List<int>(), null, null);
        }

        // Else the player can consider if the discard is worth it.
        return new ResultMove<int>(MoveKind.STOCK, new List<int>(), null, null);
    }

    public ResultMove<int>? MakePlay(SinglePlayer<T, U> p) {
        List<int>? poses = this.FirstRun(p.GetHand());

        if (poses == null) {
            return null;
            //poses = this.FirstRun(p.GetHand());
        } else {
            return new ResultMove<int>(MoveKind.RUN, poses, null, null);
        }
    }

    public ResultMove<int> MakeDiscard(SinglePlayer<T, U> p) {
        List<int> res = new List<int>();
        res.Add(0);

        return new ResultMove<int>(MoveKind.SHED, res, null, null);
    }
}
