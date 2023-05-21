using SFML.Graphics;
using SFML.Window;

namespace Domain {
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
            void DoShed(Stack<ICard<T, U>> discard);

            void Add(ICard<T, U> card);

            /*
            string GetName();
            void ShowHand();
            */

            void Update(RenderWindow window);
            void Render(RenderWindow window);
        }
}
