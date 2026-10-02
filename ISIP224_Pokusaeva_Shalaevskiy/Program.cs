using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    public static class Program
    {
        private static readonly Library Library = new Library();

        public static void Main()
        {
            SeedTestData();
            RunMenu();
        }

        private static void SeedTestData()
        {
            Library.AddBook("Война и мир", "Лев Толстой", Genre.Fiction, 1869, 1500m);
            Library.AddBook("Преступление и наказание", "Фёдор Достоевский", Genre.Fiction, 1866, 1200m);
            Library.AddBook("Властелин колец", "Джон Толкин", Genre.Fantasy, 1954, 2000m);
            Library.AddBook("Шерлок Холмс", "Артур Конан Дойл", Genre.Detective, 1892, 900m);
            Library.AddBook("Краткая история времени", "Стивен Хокинг", Genre.Science, 1988, 1100m);
        }

        private static void RunMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("================ БИБЛИОТЕКА ================");
                Console.WriteLine(" 1. Добавить книгу");
                Console.WriteLine(" 2. Удалить книгу по ID");
                Console.WriteLine(" 3. Найти книги (название / автор / жанр)");
                Console.WriteLine(" 4. Сортировать (по названию / по году)");
                Console.WriteLine(" 5. Самая дорогая / самая дешёвая книга");
                Console.WriteLine(" 6. Сгруппировать по авторам");
                Console.WriteLine(" 7. Показать все книги");
                Console.WriteLine(" 8. Вставить блок книг (пакетный импорт)");
                Console.WriteLine(" 0. Выход");
                Console.WriteLine("===========================================");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddBookFlow(); break;
                        case "2": RemoveBookFlow(); break;
                        case "3": FindBooksFlow(); break;
                        case "4": SortBooksFlow(); break;
                        case "5": ShowPriceExtremes(); break;
                        case "6": GroupByAuthor(); break;
                        case "7": 
                            Console.WriteLine($"  {"ID:",-3}|{"Название:",-30}|{"Автор:",-20}|{"Жанр:",-10}|{"Год:",-4}|{"Цена:",9}");
                            Console.WriteLine("-----------------------------------------------------------------------------------");
                            PrintBooks(Library.Books); 
                            break;
                        case "8": BatchImportFlow(); break;
                        case "0": return;
                        default: Console.WriteLine("Неизвестная команда."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Ошибка] " + ex.Message);
                }

                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
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
            if (Library.Books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            int id = InputHelper.ReadInt("Введите ID книги для удаления: ", 1, int.MaxValue);
            if (Library.RemoveBook(id))
                Console.WriteLine("[OK] Книга с ID={0} удалена.", id);
            else
                Console.WriteLine("[!] Книга с ID={0} не найдена.", id);
        }

        private static void FindBooksFlow()
        {
            Console.WriteLine("--- Поиск книг ---");
            Console.WriteLine(" 1. По названию");
            Console.WriteLine(" 2. По автору");
            Console.WriteLine(" 3. По жанру");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            IEnumerable<Book> results;

            if (choice == "1")
            {
                string title = InputHelper.ReadNonEmptyString("Введите часть названия: ");
                results = Library.Books.Where(b =>
                    b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (choice == "2")
            {
                string author = InputHelper.ReadNonEmptyString("Введите часть имени автора: ");
                results = Library.Books.Where(b =>
                    b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (choice == "3")
            {
                Genre genre = InputHelper.ReadGenre();
                results = Library.Books.Where(b => b.Genre == genre);
            }
            else
            {
                Console.WriteLine("Неизвестный вариант.");
                return;
            }

            PrintBooks(results);
        }

        private static void SortBooksFlow()
        {
            Console.WriteLine("--- Сортировка книг ---");
            Console.WriteLine(" 1. По названию (A-Z)");
            Console.WriteLine(" 2. По году (сначала новые)");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            IEnumerable<Book> sorted;

            if (choice == "1")
                sorted = Library.Books.OrderBy(b => b.Title);
            else if (choice == "2")
                sorted = Library.Books.OrderByDescending(b => b.Year);
            else
            {
                Console.WriteLine("Неизвестный вариант.");
                return;
            }

            PrintBooks(sorted);
        }

        private static void ShowPriceExtremes()
        {
            Console.WriteLine("--- Самая дорогая и самая дешёвая книга ---");
            if (Library.Books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            decimal max = Library.Books.Max(b => b.Price);
            decimal min = Library.Books.Min(b => b.Price);

            IEnumerable<Book> mostExpensive = Library.Books.Where(b => b.Price == max);
            IEnumerable<Book> cheapest = Library.Books.Where(b => b.Price == min);

            Console.WriteLine("\nДороже всех ({0}Руб.):", max);
            PrintBooks(mostExpensive);

            Console.WriteLine("\nДешевле всех ({0}Руб.):", min);
            PrintBooks(cheapest);
        }

        private static void GroupByAuthor()
        {
            Console.WriteLine("--- Количество книг по авторам ---");
            if (Library.Books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            var groups = Library.Books
                .GroupBy(b => b.Author)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key);

            foreach (var g in groups)
                Console.WriteLine("  {0,-30} — {1} шт.", g.Key, g.Count());
        }

        private static void PrintBooks(IEnumerable<Book> books)
        {
            List<Book> list = books.ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            foreach (Book b in list)
                Console.WriteLine("  " + b);
            Console.WriteLine("\n  Всего: " + list.Count);
        }

        private static void BatchImportFlow()
        {
            Console.WriteLine("--- Пакетный импорт книг ---");
            Console.WriteLine("Формат строки: Название;Автор;Жанр;Год;Цена");
            Console.WriteLine("Жанр можно указать именем (Fiction, Science, ...).");
            Console.WriteLine("Введите пустую строку, чтобы закончить.\n");

            int added = 0;
            int lineNum = 0;

            while (true)
            {
                lineNum++;
                Console.Write("Строка {0}: ", lineNum);
                string line = Console.ReadLine();

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

            string[] parts = line.Split(';');
            if (parts.Length != 5)
            {
                error = "ожидается 5 полей, разделённых ';'";
                return false;
            }

            for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim();

            if (string.IsNullOrWhiteSpace(parts[0])) { error = "пустое название"; return false; }
            if (string.IsNullOrWhiteSpace(parts[1])) { error = "пустой автор"; return false; }

            try
            {
                genre = (Genre)Enum.Parse(typeof(Genre), parts[2], true);
            }
            catch
            {
                error = "неизвестный жанр '" + parts[2] + "'";
                return false;
            }

            if (!int.TryParse(parts[3], out year) || year < 1000 || year > 2100)
            {
                error = "некорректный год '" + parts[3] + "'";
                return false;
            }

            string priceStr = parts[4].Replace(',', '.');
            if (!decimal.TryParse(priceStr, NumberStyles.Number,
                    CultureInfo.InvariantCulture, out price) || price <= 0)
            {
                error = "некорректная цена '" + parts[4] + "'";
                return false;
            }

            title = parts[0];
            author = parts[1];
            return true;
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