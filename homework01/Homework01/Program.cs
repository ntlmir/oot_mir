using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework01
{
    class Program
    {
        //Напишите программу, которая моделирует родственные связи.
        //Программа позволяет создать объекты типа Person и указывать, кто из людей кому является родителем и кто с кем состоит в браке.

        //Должны быть функции, позволяющие для каждого человека вывести список:
        //  Родителей
        //  Двоюродных братьев и сестер
        //  Дядюшек и тетушек
        //  In-laws (cвекра и свекрови или тестя и тещи)
        static void Main(string[] args)
        {
            //Решаем куда будем выводить результаты, в консоль или в файл
            bool outputTXT = false;

            //Создаём наше родственное дерево

            Person p1 = new Person("Adam");
            Person p2 = new Person("Eva");
            p1.SetPartner(p2);

            Person p3 = new Person("Kolya");
            Person p4 = new Person("Anya");
            p3.SetPartner(p4);

            Person p5 = new Person("Petya", p3, p4);

            Person p6 = new Person("Sasha", p1, p2);
            Person p7 = new Person("Masha");
            p6.SetPartner(p7);

            Person p8 = new Person("Vasya", p1, p2);
            Person p9 = new Person("Lena");
            p8.SetPartner(p9);

            Person p10 = new Person("Sonya", p6, p7);
            p10.SetPartner(p5);

            Person p11 = new Person("Katya", p6, p7);
            Person p12 = new Person("Oleg", p6, p7);

            Person p13 = new Person("Olya", p8, p9);
            Person p14 = new Person("Rita", p8, p9);

            Person p15 = new Person("Roma");
            p15.SetPartner(p13);

            //Работаем с созданным деревом

            Dictionary<string, List<string>> relatives = new Dictionary<string, List<string>>();

            List<string> listA = p10.GetParentsNames(); //out Sasha and Masha
            relatives.Add("Parents", listA);

            List<string> listB = p10.GetCousins(); //out Olya and Rita
            relatives.Add("Cousins", listB);

            List<string> listC = p10.GetUnclesAnts(); //out Vasya and Lena
            relatives.Add("Uncles and Ants", listC);

            List<string> listD = p10.GetInLaws(); //out Kolya and Anya
            relatives.Add("In-laws", listD);

            switch (outputTXT)
            {
                case false:
                    WriteInConsole(p10, relatives);
                    break;
                case true:
                    WriteInFile(p10, relatives);
                    Console.WriteLine("See results in file.");
                    break;
            }

            Console.ReadLine();
        }

        static void WriteInConsole(Person p, Dictionary<string, List<string>> relatives)
        {
            foreach(KeyValuePair<string, List<string>> keyValue in relatives)
            {
                Console.Write(p.name + "'s " + keyValue.Key + ": ");
                foreach(string human in keyValue.Value)
                {
                    Console.Write(human + " ");
                }
                Console.WriteLine();
            }
        }

        static void WriteInFile(Person p, Dictionary<string, List<string>> relatives)
        {
            string writeRel = @"C:\Users\Наташа\Desktop\OOT\Homework01\Relatives.txt";

            string record = "";

            foreach (KeyValuePair<string, List<string>> keyValue in relatives)
            {
                record = record + p.name + "'s " + keyValue.Key + ": ";
                foreach (string human in keyValue.Value)
                {
                    record = record + human + " ";
                }
                record = record + "\n";
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(writeRel, false))
                {
                    sw.Write(record);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
