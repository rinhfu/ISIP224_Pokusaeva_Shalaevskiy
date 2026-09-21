using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();

            while (true)
            {
                PrintMenu();

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Ошибка: введите число из меню.");
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1: store.ShowAll(); break;
                    case 2: store.AddProduct(); break;
                    case 3: store.RemoveProduct(); break;
                    case 4: store.RestockProduct(); break;
                    case 5: store.SellProduct(); break;
                    case 6: store.SearchProducts(); break;
                    case 7: store.ShowSalesHistory(); break;
                    case 8: store.CancelLastSale(); break;
                    case 9: store.ShowSalesReport(); break;
                    case 0: Console.WriteLine("Выход из программы. До свидания!"); return;
                    default: Console.WriteLine("Неверный пункт меню."); break;
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("УЧЁТ ТОВАРОВ В МАГАЗИНЕ");
            Console.WriteLine("1. Показать все товары");
            Console.WriteLine("2. Добавить товар");
            Console.WriteLine("3. Удалить товар");
            Console.WriteLine("4. Заказать поставку товара");
            Console.WriteLine("5. Продать товар");
            Console.WriteLine("6. Поиск товаров");
            Console.WriteLine("7. История продаж");
            Console.WriteLine("8. Отменить последнюю продажу");
            Console.WriteLine("9. Отчёт о продажах");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }
    }

    public enum Category
    {
        Food = 1,
        Electronics = 2,
        Clothing = 3,
        Household = 4,
        Toys = 5
    }

    public class Product
    {
        private string _name;
        private decimal _price;
        private int _quantity;

        private static int _counter = 0;

        public int Code { get; private set; }
        public Category Category { get; private set; }

        public string Name
        {
            get => _name;
            private set => _name = value.Trim();
        }

        public decimal Price
        {
            get => _price;
            private set => _price = value;
        }

        public int Quantity
        {
            get => _quantity;
            private set => _quantity = value;
        }

        public bool InStock => _quantity > 0;

        public Product(string name, decimal price, int quantity, Category category)
        {
            _counter++;
            Code = _counter;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public bool Sell(int amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Количество для продажи должно быть больше нуля.");
                return false;
            }

            if (amount > Quantity)
            {
                Console.WriteLine($"Недостаточно товара на складе. Доступно: {Quantity}.");
                return false;
            }

            Quantity -= amount;
            Console.WriteLine($"Продано {amount} шт. товара \"{Name}\". Остаток: {Quantity}.");
            return true;
        }

        public void Restock(int amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Количество поставки должно быть больше нуля.");
                return;
            }

            Quantity += amount;
            Console.WriteLine($"Поставка {amount} шт. товара \"{Name}\" выполнена. Новый остаток: {Quantity}.");
        }

        public override string ToString() => $"Код: {Code} | Название: {Name} | Цена: {Price} Руб. | Кол-во: {Quantity} | В наличии: {(InStock ? "Да" : "Нет")} | Категория: {Category}";
    }

    public class Store
    {
        private readonly List<Product> _products = new List<Product>();

        private readonly Stack<SaleRecord> _salesHistory = new Stack<SaleRecord>();

        public Store() => SeedTestData();

        private void SeedTestData()
        {
            _products.Add(new Product("Хлеб Бородинский", 45.50m, 30, Category.Food));
            _products.Add(new Product("Молоко 3.2%", 89.90m, 20, Category.Food));
            _products.Add(new Product("Смартфон Samsung", 25999.00m, 5, Category.Electronics));
            _products.Add(new Product("Наушники Sony", 7990.00m, 12, Category.Electronics));
            _products.Add(new Product("Футболка мужская", 1200.00m, 0, Category.Clothing));
        }

        public void AddProduct()
        {
            Console.WriteLine("--- Добавление товара ---");
            string name = InputHelper.ReadNonEmptyString("Введите название: ");
            decimal price = InputHelper.ReadPositiveDecimal("Введите цену: ");
            int quantity = InputHelper.ReadPositiveInt("Введите количество: ");
            Category category = InputHelper.ReadCategory();

            Product p = new Product(name, price, quantity, category);
            _products.Add(p);
            Console.WriteLine($"Товар успешно добавлен. {p}");
        }

        public void RemoveProduct()
        {
            Console.WriteLine("--- Удаление товара ---");
            ShowAll();
            int code = InputHelper.ReadInt("\nВведите код товара: ");

            Product product = _products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            _products.Remove(product);
            Console.WriteLine($"Товар \"{product.Name}\" удалён из списка.");
        }

        public void RestockProduct()
        {
            Console.WriteLine("--- Поставка товара ---");
            ShowAll();
            int code = InputHelper.ReadInt("\nВведите код товара: ");

            Product product = _products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            int amount = InputHelper.ReadPositiveInt("Введите количество для поставки: ");
            product.Restock(amount);
        }

        public void SellProduct()
        {
            Console.WriteLine("--- Продажа товара ---");
            ShowAll();
            int code = InputHelper.ReadInt("\nВведите код товара: ");

            Product product = _products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("Товара нет в наличии, продажа невозможна.");
                return;
            }

            int amount = InputHelper.ReadPositiveInt("Введите количество для продажи: ");
            if (amount > product.Quantity)
            {
                Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.Quantity}.");
                return;
            }

            SaleRecord record = new SaleRecord(product, amount);

            if (product.Sell(amount))
            {
                _salesHistory.Push(record);
                Console.WriteLine($"Продажа записана в историю. Сумма: {record.TotalSum} Руб.");
            }
        }

        public void CancelLastSale()
        {
            Console.WriteLine("--- Отмена последней продажи ---");

            if (_salesHistory.Count == 0)
            {
                Console.WriteLine("История продаж пуста — отменять нечего.");
                return;
            }

            SaleRecord last = _salesHistory.Peek();
            Console.WriteLine($"Последняя продажа: {last}");
            Console.Write("Отменить её? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLower();

            if (answer != "y" && answer != "yes" && answer != "д" && answer != "да")
            {
                Console.WriteLine("Отмена отклонена.");
                return;
            }

            _salesHistory.Pop();

            Product product = _products.FirstOrDefault(p => p.Code == last.ProductCode);
            if (product == null)
            {
                Console.WriteLine("Внимание: товар с таким кодом удалён из каталога. " +
                                  "Возврат на склад невозможен.");
                return;
            }

            product.Restock(last.Quantity);
            Console.WriteLine($"Продажа отменена. {last.Quantity} шт. товара \"{last.ProductName}\" " +
                              $"возвращены на склад.");
        }

        public void ShowSalesHistory()
        {
            Console.WriteLine("--- История продаж ---");

            if (_salesHistory.Count == 0)
            {
                Console.WriteLine("История продаж пуста.");
                return;
            }

            Console.WriteLine($"Всего записей: {_salesHistory.Count}");
            Console.WriteLine("(от последней к первой)");

            foreach (SaleRecord record in _salesHistory)
            {
                Console.WriteLine(record);
            }
        }

        public void ShowSalesReport()
        {
            Console.WriteLine("--- Отчёт о продажах ---");

            if (_salesHistory.Count == 0)
            {
                Console.WriteLine("Продаж ещё не было — отчёт пуст.");
                return;
            }

            var report = _salesHistory
                .GroupBy(s => new { s.ProductCode, s.ProductName, s.Category })
                .Select(g => new
                {
                    g.Key.ProductCode,
                    g.Key.ProductName,
                    g.Key.Category,
                    TotalQuantity = g.Sum(s => s.Quantity),
                    TotalSum = g.Sum(s => s.TotalSum)
                })
                .OrderBy(r => r.ProductCode)
                .ToList();

            Console.WriteLine($"{"Код",-8}{"Название",-28}{"Категория",-15}{"Кол-во",10}{"Сумма",20}");

            int grandQuantity = 0;
            decimal grandTotal = 0m;

            foreach (var row in report)
            {
                Console.WriteLine(
                    $"{row.ProductCode,-8}" +
                    $"{Truncate(row.ProductName, 26),-28}" +
                    $"{row.Category,-15}" +
                    $"{row.TotalQuantity,10}" +
                    $"{row.TotalSum,20} Руб.");

                grandQuantity += row.TotalQuantity;
                grandTotal += row.TotalSum;
            }

            Console.WriteLine($"{"ИТОГО:",-51}{grandQuantity,10}{grandTotal,20} Руб.");

            Console.WriteLine($"\nУникальных товаров продано: {report.Count}");
            Console.WriteLine($"Всего операций продажи:      {_salesHistory.Count}");
            Console.WriteLine($"Общая выручка:               {grandTotal} Руб.");
        }

        private static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text ?? string.Empty;
            return text.Substring(0, maxLength - 1) + "…";
        }
    
        public void SearchProducts()
        {
            Console.WriteLine("--- Поиск товаров ---");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");

            int choice = InputHelper.ReadInt("Выберите вариант поиска: ");

            List<Product> results = new List<Product>();

            switch (choice)
            {
                case 1:
                    int code = InputHelper.ReadInt("Введите код: ");
                    results = _products.Where(p => p.Code == code).ToList();
                    break;
                case 2:
                    string name = InputHelper.ReadNonEmptyString("Введите часть названия: ");
                    results = _products.Where(p => p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;
                case 3:
                    Category cat = InputHelper.ReadCategory();
                    results = _products.Where(p => p.Category == cat).ToList();
                    break;
                default:
                    Console.WriteLine("Неверный вариант поиска.");
                    return;
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }

            Console.WriteLine($"\nНайдено товаров: {results.Count}");
            foreach (Product p in results)
            {
                Console.WriteLine(p);
            }
        }

        public void ShowAll()
        {
            Console.WriteLine("--- Список всех товаров ---");
            if (_products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }

            foreach (Product p in _products)
            {
                Console.WriteLine(p);
            }
        }
    }

    public class SaleRecord
    {
        public int ProductCode { get; }
        public string ProductName { get; }
        public Category Category { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public DateTime SaleDate { get; }

        public decimal TotalSum => Price * Quantity;

        public SaleRecord(Product product, int quantity)
        {
            ProductCode = product.Code;
            ProductName = product.Name;
            Category = product.Category;
            Price = product.Price;
            Quantity = quantity;
            SaleDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{SaleDate:dd.MM.yyyy HH:mm:ss}] " +
                   $"Код: {ProductCode} | {ProductName} | " +
                   $"{Quantity} шт. х {Price} Руб. = {TotalSum} Руб. | Категория: {Category}";
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

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: значение не может быть пустым. Попробуйте ещё раз.");
                    continue;
                }
                return input.Trim();
            }
        }

        public static decimal ReadPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!decimal.TryParse(input, out decimal value))
                {
                    Console.WriteLine("Ошибка: введите корректное число.");
                    continue;
                }
                if (value < 0)
                {
                    Console.WriteLine("Ошибка: значение не может быть отрицательным.");
                    continue;
                }
                return value;
            }
        }

        public static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int value))
                {
                    Console.WriteLine("Ошибка: введите корректное целое число.");
                    continue;
                }
                if (value < 0)
                {
                    Console.WriteLine("Ошибка: значение не может быть отрицательным.");
                    continue;
                }
                return value;
            }
        }

        public static Category ReadCategory()
        {
            Console.WriteLine("Доступные категории:");
            foreach (Category c in Enum.GetValues(typeof(Category)))
            {
                Console.WriteLine($"  {(int)c}. {c}");
            }

            while (true)
            {
                Console.Write("Выберите номер категории: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(Category), value))
                {
                    return (Category)value;
                }
                Console.WriteLine("Ошибка: выберите категорию из списка.");
            }
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Ошибка: введите корректное целое число.");
            }
        }
    }
}
