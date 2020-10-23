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
        private Person partner;
        private Person[] parents = new Person[2];
        private List<Person> children = new List<Person>();
        public Person(string name)
        {
            this.name = name;
        }

        public Person(string name, Person parent1, Person parent2)
        {
            this.name = name;
            if (parent1 != parent2)
            {
                this.parents[0] = parent1;
                this.parents[1] = parent2;
                parent1.children.Add(this);
                parent2.children.Add(this);
            }
            else
            {
                Console.WriteLine("Parents should be different persons.");
            }
        }

        public void SetPartner(Person partner)
        {
            if (partner != this && this.partner == null && partner.partner == null)
            {
                this.partner = partner;
                partner.partner = this;
            }
            else if(this.partner != null)
            {
                Console.WriteLine("Error: " + this.name + " already has a partner.");
            }
            else
            {
                Console.WriteLine("ERROR");
            }
        }
        public List<string> GetParentsNames()
        {
            List<string> parnames = new List<string>();

            if(this.parents[0] != null && this.parents[1] != null)
            {
                parnames.Add(this.parents[0].name);
                parnames.Add(this.parents[1].name);
                return parnames;
            }
            else
            {
                parnames.Add("Anknown");
                return parnames;
            }
        }

        public List<Person> GetSiblings()
        {
            List<Person> siblings = new List<Person>();

            if(this.parents[0] != null)
            {
                foreach(Person child in parents[0].children)
                {
                    if(child != this)
                    {
                        siblings.Add(child);
                    }
                }
                return siblings;
            }
            else
            {
                return siblings;
            }
        }
        public List<string> GetCousins()
        {
            List<string> cousnames = new List<string>();

            if(this.parents != null)
            {
                foreach(Person par in this.parents)
                {
                    List<Person> siblings = par.GetSiblings();

                    foreach(Person sib in siblings)
                    {
                        foreach(Person child in sib.children)
                        {
                            cousnames.Add(child.name);
                        }
                    }
                }
                if(cousnames.Count != 0)
                {
                    return cousnames;
                }
                else
                {
                    cousnames.Add("Anknown");
                    return cousnames;
                }
            }
            else
            {
                cousnames.Add("Anknown");
                return cousnames;
            }
        }

        public List<string> GetUnclesAnts()
        {
            List<string> uncantnames = new List<string>();

            if (parents[0] != null)
            {
                if (parents[0].parents[0] == null && parents[1].parents[0] == null)
                {
                    uncantnames.Add("Anknown");
                    return uncantnames;
                }
                else
                {
                    foreach (Person p in parents)
                    {
                        if (p.parents[0] != null)
                        {
                            foreach (Person child in p.parents[0].children)
                            {
                                if(child != p)
                                {
                                    uncantnames.Add(child.name);
                                    uncantnames.Add(child.partner.name);
                                }
                            }
                        }
                    }
                    return uncantnames;
                }
            }
            else
            {
                uncantnames.Add("Anknown");
                return uncantnames;
            }
        }

        public List<string> GetInLaws()
        {
            List<string> inlawnames = new List<string>();

            if (this.partner != null && this.partner.parents[0] != null)
            {
                foreach(Person parent in this.partner.parents)
                {
                    inlawnames.Add(parent.name);
                }
                return inlawnames;
            }
            else
            {
                inlawnames.Add("Anknown");
                return inlawnames;
            }
        }
    }
}
