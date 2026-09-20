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
            Console.WriteLine("\n--- Продажа товара ---");
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
            product.Sell(amount);
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
