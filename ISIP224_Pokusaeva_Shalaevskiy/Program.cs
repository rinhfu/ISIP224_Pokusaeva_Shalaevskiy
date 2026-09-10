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
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
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
                            Console.WriteLine("Ошибка! Сумма должна быть числом (рубли). Попробуйте снова.");
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
            Console.WriteLine($"{"№",-4} {"Название",-30} {"Сумма(руб.)",10}");
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
            Console.WriteLine($"Среднее значение: {average:F2} руб.");
            Console.WriteLine($"Максимальная трата: {max:F2}  руб.");
            Console.WriteLine($"Минимальная трата: {min:F2}  руб.");
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
    }
}
