using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class ShopLibrary
    {
        public List<Book> LibraryList;

        public ShopLibrary()
        {
            LibraryList = new List<Book>();
        }

        public void AddBook(TypeofBook type, string title, string author, int cost)
        {
            Book newBook = new Book(type, title, author, cost);
            LibraryList.Add(newBook);
        }

        public void RemoveBook(Book book)
        {
            LibraryList.Remove(book);
        }

        public void PrintLibrary()
        {
            foreach (Book b in LibraryList)
            {
                Console.Write($"Book {LibraryList.IndexOf(b) + 1}: \"{b.Title}\" - {b.Author}");
                Console.Write($" - {b.Cost}$");
                if (b.BookType == TypeofBook.paper)
                {
                    Console.WriteLine(" - paper;");
                }
                else
                {
                    Console.WriteLine(" - fb2;");
                }
            }
        }
    }
}
