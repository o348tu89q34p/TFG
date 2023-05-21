namespace Domain {
    public interface IMeld<T, U>
        where T : Scale, new()
        where U : Scale, new()
        {
            void Add(ArrayHand<T, U> hand);
            WildCard<T, U> Replace(ICard<T, U> hand, int pos);
            void Print();
            string Type();
        }
}
