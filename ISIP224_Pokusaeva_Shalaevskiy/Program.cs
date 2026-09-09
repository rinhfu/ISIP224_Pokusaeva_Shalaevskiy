using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    internal class Program
    {
        void ToDict(in string[] items)
        {

        }
        static void Main(string[] args)
        {
            Console.Write("Введите количество операций(от 2 до 40): ");
            int col = Convert.ToInt32(Console.ReadLine());
            string[] items = new string[col];
            Dictionary<string, int> trata = new Dictionary<string, int>();
            if (col >= 2 && col <= 40)
            {
                for (int i = 0; i <= col - 1; i++)
                {
                    Console.Write($"{i+1}.Введите название услуги или товара; количество денег(Руб.): ");
                    items[i] = Console.ReadLine();
                }
                foreach (string j in items)
                {
                    Console.WriteLine(j);
                }
            }
            else Console.WriteLine("Введено неверное количество операций!");

        }
    }
}
