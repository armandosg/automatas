using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintaxis1
{
    class Program
    {
        float radio;
        public float area;
        int x26;
        static void Main(string[] args)
        {
            Console.WriteLine("Radio = ");
            radio = Console.ReadLine();
            Console.WriteLine("Area = ");
            area = (radio * radio) * (3.14159);
            Console.WriteLine(area);           
        }
    }
}