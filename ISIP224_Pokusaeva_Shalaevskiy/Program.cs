using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    public class TextStats
    {
        public string OriginalText { get; set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }

        public TextStats()
        {
            LetterFrequency = new Dictionary<char, int>();
        }

        public void Print()
        {
            Console.WriteLine("          СТАТИСТИКА             ");
            Console.WriteLine($"Количество слов: {WordCount}");
            Console.WriteLine($"Количество предложений: {SentenceCount}");
            Console.WriteLine($"Самое короткое слово: {ShortestWord}");
            Console.WriteLine($"Самое длинное слово: {LongestWord}");
            Console.WriteLine($"Гласных букв: {VowelCount}");
            Console.WriteLine($"Согласных букв: {ConsonantCount}");
            Console.WriteLine("Частота встречаемости букв:");

            foreach (KeyValuePair<char, int> pair in LetterFrequency)
            {
                Console.WriteLine($"  '{pair.Key}' : {pair.Value}");
            }

            Console.WriteLine();
        }
    }


    public static class TextAnalyzer
    {
        private const string Vowels = "аеёиоуыэюяaeiouy";

        public static bool IsValidLength(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            return text.Length >= 100;
        }

        private static List<string> SplitIntoWords(string text)
        {
            List<string> words = new List<string>();
            StringBuilder current = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetterOrDigit(c))
                {
                    current.Append(c);
                }
                else
                {
                    if (current.Length > 0)
                    {
                        words.Add(current.ToString());
                        current.Clear();
                    }
                }
            }

            if (current.Length > 0)
            {
                words.Add(current.ToString());
            }

            return words;
        }

        public static int CountSentences(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                {
                    count++;
                }
            }
            return count;
        }

        public static TextStats Analyze(string text)
        {
            TextStats stats = new TextStats();
            stats.OriginalText = text;

            List<string> words = SplitIntoWords(text);
            stats.WordCount = words.Count;

            stats.SentenceCount = CountSentences(text);

            string shortest = null;
            string longest = null;

            foreach (string word in words)
            {
                if (shortest == null || word.Length < shortest.Length)
                {
                    shortest = word;
                }
                if (longest == null || word.Length > longest.Length)
                {
                    longest = word;
                }
            }

            stats.ShortestWord = shortest ?? "-";
            stats.LongestWord = longest ?? "-";

            int vowels = 0;
            int consonants = 0;
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            foreach (char raw in text)
            {
                if (!char.IsLetter(raw))
                {
                    continue;
                }

                char c = char.ToLower(raw);

                if (Vowels.IndexOf(c) >= 0)
                {
                    vowels++;
                }
                else
                {
                    consonants++;
                }

                if (frequency.ContainsKey(c))
                {
                    frequency[c] = frequency[c] + 1;
                }
                else
                {
                    frequency[c] = 1;
                }
            }

            stats.VowelCount = vowels;
            stats.ConsonantCount = consonants;
            stats.LetterFrequency = frequency;

            return stats;
        }
    }

    public class StatsStorage
    {
        private readonly List<TextStats> _history = new List<TextStats>();

        public void Add(TextStats stats)
        {
            _history.Add(stats);
        }

        public int Count
        {
            get { return _history.Count; }
        }

        public TextStats GetAt(int index)
        {
            if (index < 0 || index >= _history.Count)
            {
                return null;
            }
            return _history[index];
        }

        public List<TextStats> GetAll()
        {
            return _history;
        }
    }

    
}

