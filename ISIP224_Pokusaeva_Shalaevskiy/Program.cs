using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    public class BankAccount
    {
        public decimal Balance { get; private set; }

        public BankAccount(decimal balance)
        {
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentOutOfRangeException("Сумма <= 0!");
                }
                else
                {
                    Balance += amount;
                    Console.WriteLine($"Пополнено: {amount}");
                }
            }
        }

        private static void AddBookFlow()
        {
            Console.WriteLine("--- Добавление книги ---");
            string title = InputHelper.ReadNonEmptyString("Название: ");
            string author = InputHelper.ReadNonEmptyString("Автор: ");
            Genre genre = InputHelper.ReadGenre();
            int year = InputHelper.ReadInt("Год издания (1000-2100): ", 1000, 2100);
            decimal price = InputHelper.ReadPositiveDecimal("Цена: ");

            Book book = Library.AddBook(title, author, genre, year, price);
            Console.WriteLine("\n[OK] Книга добавлена: " + book);
        }

        private static void RemoveBookFlow()
        {
            Console.WriteLine("--- Удаление книги ---");
            Console.WriteLine($"  {"ID:",-3}|{"Название:",-30}|{"Автор:",-20}|{"Жанр:",-10}|{"Год:",-4}|{"Цена:",9}");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            PrintBooks(Library.Books);
            if (Library.Books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            int id = InputHelper.ReadInt("Введите ID книги для удаления: ", 1, int.MaxValue);
            if (Library.RemoveBook(id))
                Console.WriteLine("[OK] Книга с ID: {0} удалена.", id);
            else
                Console.WriteLine("[!] Книга с ID: {0} не найдена.", id);
        }

        private static void FindBooksFlow()
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentOutOfRangeException("Вы ввели неверное значение.");
                }
                else if (Balance < amount)
                {
                    throw new InvalidOperationException("Недостаточно средств.");
                }
                else
                {
                    Balance -= amount;
                    Console.WriteLine($"Списано: {amount}. Осталось средств: {Balance}");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            BankAccount bankAccount = new BankAccount(10000);
            ShowMenu
            Console.WriteLine("Введите");
            int input = Console.ReadLine();


            try
            {

                if (string.IsNullOrWhiteSpace(line)) break;

                string title, author, error;
                Genre genre;
                int year;
                decimal price;

                if (!TryParseBookLine(line, out title, out author, out genre,
                        out year, out price, out error))
                {
                    Console.WriteLine("  [!] Пропущено: " + error);
                    continue;
                }

                Book book = Library.AddBook(title, author, genre, year, price);
                Console.WriteLine("  [OK] Добавлено: " + book);
                added++;
            }

            Console.WriteLine("\nИтого добавлено книг: " + added);
        }

        private static bool TryParseBookLine(
            string line,
            out string title, out string author, out Genre genre,
            out int year, out decimal price, out string error)
        {
            title = string.Empty;
            author = string.Empty;
            genre = default(Genre);
            year = 0;
            price = 0;
            error = string.Empty;

            }
            catch
            {

            }
            finally
            {
                Console.WriteLine($"Баланс: {bankAccount.Balance}");
            }
        }
        public void ShowMenu()
        {
            Console.WriteLine("--- Добавление в корзину ---");
            if (Library.Books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            PrintBooks(Library.Books);
            int id = InputHelper.ReadInt("\nВведите ID книги для добавления в корзину: ", 1, int.MaxValue);

            Book book = Library.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine("[!] Книга с ID={0} не найдена.", id);
                return;
            }

            Cart.Add(book);
            Console.WriteLine("[OK] \"{0}\" добавлена в корзину.", book.Title);
        }

        private static void ShowCart()
        {
            Console.WriteLine("--- Корзина ---");
            if (Cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            PrintBooks(Cart);
            decimal total = Cart.Sum(b => b.Price);
            Console.WriteLine("\nИтоговая стоимость: {0}Руб.", total);
        }
    }

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
            return string.Format("{0,-3}|{1,-30}|{2,-20}|{3,-10}|{4}|{5,5}Руб.", Id, Title, Author, Genre, Year, Price);
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
                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value) && value > 0)
                    return value;
                Console.WriteLine("  [!] Введите положительное число (например, 1500 или 1500.50).");
            }
        }

        public static Genre ReadGenre()
        {
            Genre[] values = (Genre[])Enum.GetValues(typeof(Genre));
            Console.WriteLine("  Доступные жанры:");
            for (int i = 0; i < values.Length; i++) Console.WriteLine("    {0}. {1}", i + 1, values[i]);

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