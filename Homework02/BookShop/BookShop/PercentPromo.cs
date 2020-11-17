using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class PercentPromo : IPromo
    {
        public string promoname;
        public int promovalue;

        public PercentPromo(string name, int value)
        {
            promoname = name;
            promovalue = value;
        }

        public void ApplyPromo(ref Dictionary<Book, int> Books, ref Dictionary<string, int> discount)
        {
            discount["percent"] = promovalue;
        }
    }
}
