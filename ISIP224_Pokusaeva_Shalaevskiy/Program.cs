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
            Console.Write("Введите количество операций(от 2 до 40): ");
            int col = Convert.ToInt32(Console.ReadLine());
            if (col >= 2 && col <= 40)
            {
                for (int i = 1; i <= col; i++)
                {
                    Console.Write($"{i}.Введите название услуги или товара; количество денег(Руб.): ");
                    string trata = (Console.ReadLine());
                }
            }
            else Console.WriteLine("Введено неверное количество операций!");

        }
    }
}
