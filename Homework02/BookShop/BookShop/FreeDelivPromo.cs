using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class FreeDelivPromo : IPromo
    {
        public string promoname;

        public FreeDelivPromo(string name)
        {
            promoname = name;
        }

        public void ApplyPromo(ref Dictionary<Book, int> Books, ref Dictionary<string, int> discount)
        {
            discount["deliv"] = 0;
        }
    }
}
