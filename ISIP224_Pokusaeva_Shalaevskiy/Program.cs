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

    
}

