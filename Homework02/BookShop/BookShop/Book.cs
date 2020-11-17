using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    public enum TypeofBook { paper, fb2 }
    class Book 
    {
        public string Title { get; }
        public string Author { get; }
        public int Cost { get; set; }
        public TypeofBook BookType;

        public Book(TypeofBook type, string title, string author, int cost)
        {
            BookType = type;
            Title = title;
            Author = author;
            Cost = cost;
        }
    }
}
