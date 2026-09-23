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
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Withdraw(decimal amount)
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

            }
            catch (ArgumentOutOfRangeException ex)
            {

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
            Console.Clear();
            Console.WriteLine("Меню:");
            Console.WriteLine($"1. Пополнить");
            Console.WriteLine($"2. Снять");
        }
    }
}