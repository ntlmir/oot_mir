using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework01
{
    class Person
    {
        public string name;
        public Person Partner { get; set; }
        public Person parent;

        public Person(string name)
        {
            this.name = name;
        }

        public Person(string name, Person parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public Person(string name, Person parent, Person partner)
        {
            this.name = name;
            this.parent = parent;
            this.Partner = partner;
        }

        public void ParentsNames()
        {
            Console.Write("Parents: ");
            if(this.parent != null)
            {
                Console.WriteLine(this.parent.name + " and " + this.parent.Partner.name);
            }
            else
            {
                Console.WriteLine("Anknown");
            }
        }

        public void Cousins(List<Person> people)
        {
            Console.Write("Cousins: ");
            if (this.parent != null)
            {
                Person grand = this.parent.parent;
                int i = 0;
                foreach (Person p in people)
                {
                    if (p.parent != null && this.parent != p.parent)
                    {
                        if (p.parent.parent != null)
                        {
                            if (grand == p.parent.parent)
                            {
                                if (i == 0)
                                {
                                    Console.Write(p.name);
                                }
                                else
                                {
                                    Console.Write(", " + p.name);
                                }
                                i++;
                            }
                        }
                    }
                }
                if (i == 0)
                {
                    Console.WriteLine("Anknown");
                }
            }
            else
            {
                Console.WriteLine("Anknown");
            }
            Console.WriteLine();
        }

        public void UnclesAnts(List<Person> people)
        {
            Console.WriteLine("Uncles ans Ants: ");
            if(this.parent != null)
            {
                if (this.parent.parent != null)
                {
                    foreach (Person p in people)
                    {
                        if (p.parent != null && this.parent.parent == p.parent && p != this.parent)
                        {
                            Console.WriteLine(p.name + " and "+ p.Partner.name);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Anknown");
                }
            }
            else
            {
                Console.WriteLine("Anknown");
            }
        }

        public void InLaws(List<Person> people)
        {
            Console.Write("In-laws: ");
            if (this.Partner != null && this.Partner.parent != null)
            {
                Console.WriteLine(this.Partner.parent.name + " and " + this.Partner.parent.Partner.name);
            }
            else
            {
                Console.WriteLine("Doesn't have");
            }
        }
    }
}
