using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class Cart
    {
        public Dictionary<Book, int> CartBooks;
        public Dictionary<string, int> CartDiscounts = new Dictionary<string, int>
        {
            ["deliv"] = 0,
            ["percent"] = 0,
            ["dollar"] = 0
        };
        public string CartPromo;
        public Promo cartpromos;
        public List<Book> CartLibrary;
        private float commonCost;
        private bool activeAction = false;
        private Book gift;
        public Cart(List<int> numbsbooks, string promo, Promo ourpromos, List<Book> library)
        {
            this.CartBooks = new Dictionary<Book, int>();
            CartLibrary = library;
            foreach (int n in numbsbooks)
            {
                Book b = CartLibrary[n - 1];
                CartBooks[b] = b.Cost;
            }
            CartPromo = promo;
            cartpromos = ourpromos;
        }

        public float CalculateCost()
        {
            int startcost = 0;
            bool paperbooks = false;
            List<string> paperAuthors = new List<string>();

            foreach(KeyValuePair<Book,int> kvp in CartBooks)
            {
                startcost += kvp.Value;
                if(kvp.Key.BookType == TypeofBook.paper)
                {
                    paperbooks = true;
                    paperAuthors.Add(kvp.Key.Author);
                }
            }
            if (startcost < 13 && paperbooks == true)
            {
                CartDiscounts["deliv"] = 2;
            }

            if(CartPromo != "no")
            {
                cartpromos.UseCode(CartPromo, ref CartBooks, ref CartDiscounts);
            }

            //Action
            var authorCounts = paperAuthors.GroupBy(i => i);
            foreach (var a in authorCounts)
            {
                //Console.WriteLine("{0} {1}", a.Key, a.Count());
                if (a.Count() >= 2)
                {
                    foreach (Book b in CartLibrary)
                    {
                        if(b.Author == a.Key && b.BookType == TypeofBook.fb2)
                        {
                            gift = b;
                            activeAction = true;
                            break;
                        }
                    }
                }
            }

            //Common Cost
            float comcost = 0;
            foreach (KeyValuePair<Book, int> kvp in CartBooks)
            {
                comcost += kvp.Value;
            }
            comcost += CartDiscounts["deliv"];
            if(CartDiscounts["percent"] != 0)
            {
                comcost = comcost * (100 - CartDiscounts["percent"]) / 100;
            }
            comcost -= CartDiscounts["dollar"];
            commonCost = comcost;

            return comcost;
        }

        public void PrintCartInfo()
        {
            Console.WriteLine("\nYour cart:\n");
            foreach (KeyValuePair<Book, int> kvp in CartBooks)
            {
                Console.WriteLine($"Book {kvp.Key.Title} - {kvp.Key.Author} - {kvp.Value}$");
                if (kvp.Value == 0)
                {
                    Console.WriteLine("             Congratulations! You got free book!");
                }
            }
            Console.WriteLine($"Delivery: {CartDiscounts["deliv"]}$");
            if (CartDiscounts["percent"] != 0)
            {
                Console.WriteLine($"Percent Discount: {CartDiscounts["percent"]}%");
            }
            if (CartDiscounts["dollar"] != 0)
            {
                Console.WriteLine($"Dollars Discount: -{CartDiscounts["dollar"]}$");
            }
            if(activeAction == true)
            {
                Console.WriteLine("Congratulations! You have one fb2 book as a gift! This is:");
                Console.WriteLine($"Book {gift.Title} - {gift.Author}");
            }
            Console.WriteLine($"\nTotal purchase value: {commonCost}$");
            Console.WriteLine("\nThank you to visiting our shop! See you soon!");
        }
    }
}
