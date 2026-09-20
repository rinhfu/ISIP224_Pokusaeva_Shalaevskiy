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
    }
}
