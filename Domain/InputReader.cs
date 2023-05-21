namespace Domain {
    class InputReader {
        public static PlayerAction PickAction() {
            while (true) {
                Console.WriteLine("Choose the action to perform:");
                string[] options = {"Pick from stock.",
                                    "Pick from discard.",
                                    "Meld run",
                                    "Meld set.",
                                    "Lay off",
                                    "Replace",
                                    "Discard"};
                for (int i = 0; i < options.Length; i++) {
                    Console.WriteLine($"\t{i}: {options[i]}");
                }
                string? answer = Console.ReadLine();
                Console.WriteLine($"This is your answer: {answer}");

                if (answer == null) {
                    Console.WriteLine($"Unknown input.");
                } else if (answer.Equals("0")) {
                    return PlayerAction.PickStock;
                } else if (answer.Equals("1")) {
                    return PlayerAction.PickDiscard;
                } else if (answer.Equals("2")) {
                    return PlayerAction.MeldRun;
                } else if (answer.Equals("3")) {
                    return PlayerAction.MeldSet;
                } else if (answer.Equals("4")) {
                    return PlayerAction.LayOff;
                } else if (answer.Equals("5")) {
                    return PlayerAction.Replace;
                } else if (answer.Equals("6")) {
                    return PlayerAction.Discard;
                } else {
                    Console.WriteLine("Invalid action.");
                }
            }
        }

        public static int[] ReadIndices(string prompt, int a, int b, bool unique) {
            while (true) {
                Console.WriteLine(prompt);
                string? answer = Console.ReadLine();
                if (answer == null) {
                    Console.WriteLine("Unknown input. Try again.");
                    continue;
                }
                string[] words = answer.Split(' ');
                int[] output = new int[words.Length];
                int[] outCopy = new int[words.Length];
                try {
                    output = Array.ConvertAll(words, new Converter<string, int>(Convert.ToInt32));
                    Array.Copy(output, outCopy, output.Length);
                } catch {
                    continue;
                }
                if (output == null) {
                    Console.WriteLine("Invalid input. Expected list of numbers.");
                } else if (!ValidIndices(outCopy, a, b, unique)) {
                    Console.WriteLine("Invalid value for indices.");
                } else {
                    return output;
                }
            }
        }

        private static bool ValidIndices(int[] indices, int a, int b, bool unique) {
            Array.Sort(indices);
            int? prev = null;

            for (int i = 0; i < indices.Length; i++) {
                if (indices[i] < a || indices[i] >= b) {
                    return false;
                } else if (prev == null) {
                    prev = indices[i];
                } else {
                    if (unique && prev == indices[i]) {
                        return false;
                    }
                    prev = indices[i];
                }
            }

            return true;
        }

        public static int ReadIndex(string prompt, int a, int b) {
            while (true) {
                Console.WriteLine(prompt);
                string? answer = Console.ReadLine();
                if (answer == null) {
                    Console.WriteLine("Unknown input. Try again.");
                    continue;
                }

                string[] words = answer.Split(' ');
                if (words.Length != 1) {
                    Console.WriteLine("Invalid format.");
                }

                int n = Array.ConvertAll(words, new Converter<string, int>(Convert.ToInt32))[0];
                if (n < a || n >= b) {
                    Console.WriteLine("Index out of range.");
                } else {
                    return n;
                }
            }
        }
    }
}
