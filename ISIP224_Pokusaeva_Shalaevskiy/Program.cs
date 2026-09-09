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

            int operationCount = GetOperationCount();

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
    }
    
}
