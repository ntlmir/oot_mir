using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    public enum TypeofPromo { book, delivery, percent, dollar}

    interface IPromo
    {
        void ApplyPromo(ref Dictionary<Book, int> Books, ref Dictionary<string, int> discount);
    }
    class Promo
    {
        public Dictionary<string, IPromo> ActivePromos = new Dictionary<string, IPromo>();
        public List<string> promonames = new List<string>();

        public void CreatePromo(string name, TypeofPromo type, Book book)
        {
            promonames.Add(name);
            if(type == TypeofPromo.book)
            {
                FreeBookPromo code = new FreeBookPromo(name, book);
                ActivePromos.Add(name, code);
            }
        }

        public void CreatePromo(string name, TypeofPromo type)
        {
            promonames.Add(name);
            if (type == TypeofPromo.delivery)
            {
                FreeDelivPromo code = new FreeDelivPromo(name);
                ActivePromos.Add(name, code);
            }
        }

        public void CreatePromo(string name, TypeofPromo type, int value)
        {
            promonames.Add(name);
            if (type == TypeofPromo.percent)
            {
                PercentPromo code = new PercentPromo(name, value);
                ActivePromos.Add(name, code);
            }
            else if (type == TypeofPromo.dollar)
            {
                DollarPromo code = new DollarPromo(name, value);
                ActivePromos.Add(name, code);
            }
        }

        public void UseCode(string name, ref Dictionary<Book, int> Books, ref Dictionary<string, int> discount)
        {
            IPromo code = ActivePromos[name];
            code.ApplyPromo(ref Books, ref discount);
        }
    }
}
