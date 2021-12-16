using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Dict<int, int> dict = new Dict<int, int>();
            dict.Add(-1, -1);
            dict.Add(1, -1);
            dict.Add(9, -1);
            dict[10] = 34;
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            Console.WriteLine("Список ключей");
            List<int> list = new List<int>(dict.Keys);
            foreach (var item in list)
            {
                Console.Write(item+" ");
            }
            Console.WriteLine();
            Console.WriteLine("Список значений");
            list = new List<int>(dict.Values);
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine("После удаление 9-ого элемента");
            dict.Remove(9);
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            Console.WriteLine();
            Console.WriteLine("Список ключей");
            list = new List<int>(dict.Keys);
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Список значений");
            list = new List<int>(dict.Values);
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Значение ключа 10:" + dict[10]);
            Console.WriteLine("Кол-во элементов: " + dict.Count);

        }
    }
}
