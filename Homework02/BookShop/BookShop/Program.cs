using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    class Program
    {
        // Реализовать модуль корзины для вычисления общей стоимости заказа в книжном интернет-магазине:

        // Два вида книг: бумажные и электронные
        // Для бумажных книг доставка от 13$ бесплатная, иначе 2$.Для электронных всегда бесплатная
        // Есть промокоды: на конкретную книгу бесплатно, на бесплатную доставку, на скидку X%, на скидку X$
        // Есть акция: при покупке двух бумажных книг одного автора одна электронная книга того же автора в подарок
        // Система должна быть расширяемой: легко добавить новые правила, скидки, виды промокодов и книг

        static void Main(string[] args)
        {
            ShopLibrary OurLibrary = new ShopLibrary();

            OurLibrary.AddBook(TypeofBook.paper, "Mouse", "Mark Pole", 15);
            OurLibrary.AddBook(TypeofBook.paper, "Elephant", "Jhon Sedvic", 8);
            OurLibrary.AddBook(TypeofBook.paper, "Lion", "Jhon Sedvic", 20);
            OurLibrary.AddBook(TypeofBook.paper, "Cat", "Mark Pole", 5);
            OurLibrary.AddBook(TypeofBook.paper, "Dog", "Betty Pie", 11);
            OurLibrary.AddBook(TypeofBook.fb2, "Rabbit", "Mark Pole", 7);
            OurLibrary.AddBook(TypeofBook.fb2, "Pig", "Loo Spark", 2);
            OurLibrary.AddBook(TypeofBook.fb2, "Frog", "Eshton Bree", 18);
            OurLibrary.AddBook(TypeofBook.fb2, "Sheep", "Enola Holms", 9);
            OurLibrary.AddBook(TypeofBook.fb2, "Monkey", "Jhon Sedvic", 14);

            Promo OurPromos = new Promo();
            OurPromos.CreatePromo("freeBBLion", TypeofPromo.book, OurLibrary.LibraryList[2]); //книга Лев бесплатно
            OurPromos.CreatePromo("freeBBMonkey", TypeofPromo.book, OurLibrary.LibraryList[9]); //книга Обезьянка бесплатно
            OurPromos.CreatePromo("freeBBDeliv", TypeofPromo.delivery); //бесплтаная доставка
            OurPromos.CreatePromo("fiveBBDisc", TypeofPromo.percent, 5); //5% скидка
            OurPromos.CreatePromo("tenBBDisc", TypeofPromo.percent, 10); //10% скидка
            OurPromos.CreatePromo("oneBBdollar", TypeofPromo.dollar, 1); //скидка в 1$
            OurPromos.CreatePromo("threeBBdollar", TypeofPromo.dollar, 3); //скидка в 3$

            OurLibrary.PrintLibrary();

            Console.WriteLine("\nWrite numbers of book that you want to buy:");
            var numbs = Console.ReadLine().Split(',').Select(Int32.Parse).ToList();

            string promo = "";
            while (true)
            {
                Console.WriteLine("Maybe you have a promocode? White it or no:"); //на одну покупку один промокод
                promo = Console.ReadLine();
                if (promo == "no")
                {
                    break;
                }
                else if (OurPromos.promonames.Contains(promo) == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("InvalideCode, try again.");
                }
            }

            Cart c1 = new Cart(numbs, promo, OurPromos, OurLibrary.LibraryList);
            float cost = c1.CalculateCost();
            c1.PrintCartInfo();

            Console.ReadLine();
        }
    }
}
