namespace Domain {
    public abstract class Scale {
        public abstract string[] Data { get; }
    }

    public class FrenchSuit : Scale {
        private static string[] _data = {
            "clubs",
            "diamonds",
            "hearts",
            "spades"
        };

        public override string[] Data {
            get { return _data; }
        }
    }

    public class FrenchRank : Scale {
        private static string[] _data = {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "ten",
            "jack",
            "queen",
            "king"
        };

        public override string[] Data {
            get { return _data; }
        }
    }

    public class SpanishSuit : Scale {
        private static string[] _data = {
            "sowrds",
            "cups",
            "coins",
            "clubs"
        };

        public override string[] Data {
            get { return _data; }
        }
    }

    public class SpanishRank : Scale {
        private static string[] _data = {
            "ace",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "knave",
            "knight",
            "king"
        };

        public override string[] Data {
            get { return _data; }
        }
    }
}
