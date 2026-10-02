using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    public enum Genre
    {
        Fiction,
        Science,
        Fantasy,
        Detective,
        History,
        Poetry
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0,-3} | \"{1}\" | {2} | {3} | {4} г. | {5:C}",
                Id, Title, Author, Genre, Year, Price);
        }
    }

    public class Library
    {
        private readonly List<Book> _books = new List<Book>();
        private int _nextId = 1;

        public IReadOnlyList<Book> Books
        {
            get { return _books; }
        }

        public Book AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            Book book = new Book();
            book.Id = _nextId++;
            book.Title = title;
            book.Author = author;
            book.Genre = genre;
            book.Year = year;
            book.Price = price;

            _books.Add(book);
            return book;
        }

        public bool RemoveBook(int id)
        {
            Book book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;
            _books.Remove(book);
            return true;
        }
    }

    public static class InputHelper
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                Console.WriteLine("  [!] Значение не может быть пустым.");
            }
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                int value;
                if (int.TryParse(input, out value) && value >= min && value <= max)
                    return value;
                Console.WriteLine("  [!] Введите целое число от {0} до {1}.", min, max);
            }
        }

        public static decimal ReadPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input != null) input = input.Replace(',', '.');
                decimal value;
                if (decimal.TryParse(input, NumberStyles.Number,
                        CultureInfo.InvariantCulture, out value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("  [!] Введите положительное число (например, 1500 или 1500.50).");
            }
        }

        public static Genre ReadGenre()
        {
            Genre[] values = (Genre[])Enum.GetValues(typeof(Genre));
            Console.WriteLine("  Доступные жанры:");
            for (int i = 0; i < values.Length; i++)
                Console.WriteLine("    {0}. {1}", i + 1, values[i]);

            while (true)
            {
                Console.Write("  Выберите жанр (1-{0}): ", values.Length);
                string input = Console.ReadLine();
                int num;
                if (int.TryParse(input, out num) && num >= 1 && num <= values.Length)
                    return values[num - 1];
                Console.WriteLine("  [!] Некорректный номер жанра.");
            }
        }
    }

}