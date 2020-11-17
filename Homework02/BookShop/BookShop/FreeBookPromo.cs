using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class FreeBookPromo : IPromo
    {
        public string promoname;
        public Book promobook;

        public FreeBookPromo(string name, Book book)
        {
            promoname = name;
            promobook = book;
        }

        public void ApplyPromo(ref Dictionary<Book, int> Books, ref Dictionary<string, int> discount)
        {
            Books[promobook] = 0;
        }
    }
}
