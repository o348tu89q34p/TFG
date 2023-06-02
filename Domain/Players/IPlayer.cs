namespace Domain;

public interface IPlayer<T, U>
    where T : Scale, new()
    where U : Scale, new()
    {
        bool HasWon();
        PlayerAction ChooseAction();
        // Make these return a status line.
        void DoPickStock(Stack<ICard<T, U>> stock);
        void DoPickDiscard(Stack<ICard<T, U>> discard);
        void DoMeldRun(Rules<T, U> rules, List<IMeld<T, U>> melds);
        void DoMeldSet(Rules<T, U> rules, List<IMeld<T, U>> melds);
        void DoLayOff(List<IMeld<T, U>> melds);
        void DoReplace(List<IMeld<T, U>> melds);
        ICard<T, U> DoShed(Stack<ICard<T, U>> discard);

        void Add(ICard<T, U> card);

        PlayerProfile GetProfile();
        bool HasComeOut();
        ArrayHand<T, U> GetHand();

        /*
          REMOVE THESE FROM THE CLASSES THAT IMPLEMENT IPLAYER.
          string GetName();
          void ShowHand();
        */
    }
