using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<Person> people = new List<Person>();

            Person p1 = new Person("Adam");
            Person p2 = new Person("Eva");
            p1.Partner = p2;
            p2.Partner = p1;
            people.Add(p1);
            people.Add(p2);

            Person p3 = new Person("Kolya");
            Person p4 = new Person("Anya");
            p3.Partner = p4;
            p4.Partner = p3;
            people.Add(p3);
            people.Add(p4);

            //____________________________

            Person p5 = new Person("Petya", p3);
            people.Add(p5);

            Person p6 = new Person("Sasha", p1);
            Person p7 = new Person("Masha");
            p7.Partner = p6;
            p6.Partner = p7;
            people.Add(p6);
            people.Add(p7);

            Person p8 = new Person("Vasya", p1);
            Person p9 = new Person("Lena");
            p8.Partner = p9;
            p9.Partner = p8;
            people.Add(p8);
            people.Add(p9);

            Person p10 = new Person("Sonya", p6, p5);
            p5.Partner = p10;
            Person p11 = new Person("Katya", p6);
            Person p12 = new Person("Oleg", p6);
            people.Add(p10);
            people.Add(p11);
            people.Add(p12);

            Person p13 = new Person("Olya", p8);
            Person p14 = new Person("Rita", p8);
            people.Add(p13);
            people.Add(p14);

            Person p15 = new Person("Roma");
            p15.Partner = p13;
            p13.Partner = p15;
            people.Add(p15);

            //____________________________

            p10.ParentsNames(); //out Sasha and Masha

            p10.Cousins(people); //out Olya and Rita

            p10.UnclesAnts(people); //out Vasya and Lena

            p10.InLaws(people); //out Kolya and Anya

            Console.ReadLine();
        }
    }
}
