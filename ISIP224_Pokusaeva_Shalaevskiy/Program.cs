using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    internal class Program
    {
        static List<KeyValuePair<string, decimal>> expenses = new List<KeyValuePair<string, decimal>>();

        static void Main(string[] args)
        {
            int operationCount = GetOperationCount();

            InputExpenses(operationCount);

            bool exit = false;
            while (!exit)
            {
                ShowMenu();

                string choise = Console.ReadLine();
                switch (choise)
                {
                    case "1":
                        DisplayExpenses();
                        break;
                    case "2":
                        ShowStatistics();
                        break;
                    case "3":
                        BubbleSort();
                        break;
                    case "4":
                        CurencyConversion();
                        break;
                    case "5":
                        SearchByName();
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Программа завершена. До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static int GetOperationCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите число от 2 до 40.");
                }
            }
        }

        static void InputExpenses(int count)
        {
            Console.Clear();
            Console.WriteLine("Введите траты в формате: Название; Сумма");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235\n");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Операция {i + 1}: ");
                    string input = Console.ReadLine();

                    string[] parts = input.Split(';');

                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        string amountStr = parts[1].Trim();

                        if (decimal.TryParse(amountStr, out decimal amount) && amount >= 0)
                        {
                            expenses.Add(new KeyValuePair<string, decimal>(name, amount));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Ошибка! Сумма должна быть числом (Руб.). Попробуйте снова.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка! Используйте формат: Название; Сумма");
                    }
                }
            }

            Console.Clear();
            Console.WriteLine("Все операции успешно записаны!\n");
        }

        static void ShowMenu()
        {
            Console.WriteLine("МЕНЮ ОПЕРАЦИЙ");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор:");
        }

        static void DisplayExpenses()
        {
            Console.Clear();
            Console.WriteLine("Список трат:");
            if (expenses.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }
            Console.WriteLine($"{"№",-4} {"Название",-30} {"Сумма(Руб.)",10}");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i+1,-4} {expenses[i].Key,-30} {expenses[i].Value,10:F2}");
            }
        }

        static void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("Статистика:");

            var values = expenses.Select(e => e.Value).ToList();
            decimal sum = values.Sum();
            decimal average = sum / values.Count;
            decimal max = values.Max();
            decimal min = values.Min();

            Console.WriteLine($"Всего операций: {values.Count}");
            Console.WriteLine($"Общая сумма: {sum:F2} руб.");
            Console.WriteLine($"Среднее значение: {average:F2} Руб.");
            Console.WriteLine($"Максимальная трата: {max:F2}  Руб.");
            Console.WriteLine($"Минимальная трата: {min:F2}  Руб.");
        }

        static void BubbleSort()
        {
            Console.Clear();
            Console.WriteLine("Сортировка по цене:");

            for (int i = 0; i < expenses.Count - 1; i++)
            {
                for (int j = 0; j < expenses.Count - 1 - i; j++)
                {
                    if (expenses[j].Value > expenses[j + 1].Value)
                    {
                        var temp = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Список отсортирован по возрастанию цены:");
            DisplayExpenses();
        }

        static void CurencyConversion()
        {
            Console.Clear();
            Console.WriteLine("Конвертация валюты:");

            Console.WriteLine("Выберите валюту или введите свой курс:");
            Console.WriteLine("1. USD (Доллар США) - курс: ~84.35 Руб.");
            Console.WriteLine("2. EUR (Евро) - курс: ~98.29 Руб.");
            Console.WriteLine("3. CNY (Юань) - курс: ~12.56 Руб.");
            Console.WriteLine("4. Ввести свой курс");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            decimal rate = 0;
            string currencyName = "";

            switch (choice)
            {
                case "1":
                    rate = 84.35m;
                    currencyName = "USD";
                    break;
                case "2":
                    rate = 98.29m;
                    currencyName = "EUR";
                    break;
                case "3":
                    rate = 12.56m;
                    currencyName = "CNY";
                    break;
                case "4":
                    Console.Write("Введите курс (1 валюта = X рублей): ");
                    if (!decimal.TryParse(Console.ReadLine(), out rate) || rate <= 0)
                    {
                        Console.WriteLine("Ошибка! Введите корректный курс.");
                        return;
                    }
                    Console.Write("Введите название валюты: ");
                    currencyName = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(currencyName))
                        currencyName = "Валюта";
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            Console.Clear();
            Console.WriteLine($"Конвертация в {currencyName.ToUpper()}");
            Console.WriteLine($"Курс: 1 {currencyName} = {rate:F2} Руб.");

            Console.WriteLine($"{"№",-4} {"Название",-30} {"Сумма (Руб.)",12} {currencyName,12}");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < expenses.Count; i++)
            {
                decimal converted = expenses[i].Value / rate;
                Console.WriteLine($"{i + 1,-4} {expenses[i].Key,-30} {expenses[i].Value,12:F2} {converted,12:F2}");
            }

            decimal totalRub = expenses.Sum(e => e.Value);
            decimal totalConverted = totalRub / rate;
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Итого:",-36} {totalRub,12:F2} {totalConverted,12:F2}");
        }

        static void SearchByName()
        {
            Console.Clear();
            Console.WriteLine("Поиск по названию:");

            Console.Write("Введите ключевое слово для поиска: ");
            string searchTerm = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                Console.WriteLine("Поисковый запрос не может быть пустым.");
                return;
            }

            var results = expenses.Where(e => e.Key.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            Console.Clear();
            Console.WriteLine($"Результаты поиска: \"{searchTerm}\"");

            if (results.Count == 0)
            {
                Console.WriteLine("Совпадений не найдено.");
                return;
            }

            Console.WriteLine($"{"№",-4} {"Название",-30} {"Сумма (Руб.)",10}");
            Console.WriteLine(new string('-', 46));

            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4} {results[i].Key,-30} {results[i].Value,10:F2}");
            }

            Console.WriteLine(new string('-', 46));
            Console.WriteLine($"Найдено совпадений: {results.Count}");
        }
    }
}
