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
