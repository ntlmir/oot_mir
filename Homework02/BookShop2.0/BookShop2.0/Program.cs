using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            var b1 = new PaperBook("A", "AA", 100);
            var b2 = new ElectronicBook("B", "BB", 200);
            var b3 = new AudioBook("C", "CC", 300);
            var b4 = new PaperBook("B", "AA", 100);
            var b5 = new ElectronicBook("C", "AA", 100);

            var ShopLibrary = new Library();
            ShopLibrary.AddBook(b1);
            ShopLibrary.AddBook(b2);
            ShopLibrary.AddBook(b3);
            ShopLibrary.AddBook(b4);
            ShopLibrary.AddBook(b5);

            var deliv = new DeliveryCalculator();
            var actionProv = new ActionProvider();
            

            //var promo = new FreeBookPromo(b1);
            //var promo2 = new PercentDiscountPromo(10);
            var promolist = new List<IPromo>();
            //promolist.Add(promo2);
            //promolist.Add(promo);

            var cart = new ShoppingCart(deliv, actionProv, ShopLibrary);
            var list = new List<ILiterature>();
            list.Add(b1);
            list.Add(b4);
            Console.WriteLine(cart.GetTotalPrice(list, true, promolist));

            Console.ReadLine();
        }
    }

    interface ILiterature
    {
        string Title { get; }
        string Author { get; }
        decimal Price { get; }
    }

    class PaperBook : ILiterature
    {
        public string Title { get; }
        public string Author { get; }
        public decimal Price { get; }

        public PaperBook(string title, string author, decimal price)
        {
            Title = title;
            Author = author;
            Price = price;
        }
    }

    class ElectronicBook : ILiterature
    {
        public string Title { get; }
        public string Author { get; }
        public decimal Price { get; }

        public ElectronicBook(string title, string author, decimal price)
        {
            Title = title;
            Author = author;
            Price = price;
        }
    }

    class AudioBook : ILiterature
    {
        public string Title { get; }
        public string Author { get; }
        public decimal Price { get; }

        public AudioBook(string title, string author, decimal price)
        {
            Title = title;
            Author = author;
            Price = price;
        }
    }

    class ShoppingCart
    {
        private IDeliveryCalculator _deliveryCalculator;
        private IActionProvider _actionProvider;
        private Library _shopLibrary;

        public ShoppingCart(IDeliveryCalculator deliveryCalculator, IActionProvider actionProvider, Library shopLibrary)
        {
            _deliveryCalculator = deliveryCalculator;
            _actionProvider = actionProvider;
            _shopLibrary = shopLibrary;
        }

        public decimal GetTotalPrice(List<ILiterature> literature, bool isDelivery, List<IPromo> promoCodes)
        {
            var order = new Order(literature)
            {
                DeliveryPrice = isDelivery ? _deliveryCalculator.GetDeliveryPrice(literature) : 0
            };

            promoCodes.ForEach(p => p.ApplyPromo(order));
            _actionProvider.GetActiveActions().ForEach(a => a.ApplyAction(order, _shopLibrary.libraryList));

            return order.GetTotalFinalPrice();
        }
    }

    interface IDeliveryCalculator
    {
        decimal GetDeliveryPrice(List<ILiterature> literature);
    }

    class DeliveryCalculator : IDeliveryCalculator
    {
        private decimal deliveryPrice = 200;
        private decimal MinFreeDeliveryCost = 1000;

        public decimal GetDeliveryPrice(List<ILiterature> literature)
        {
            var paperBooksCost = literature
                                .OfType<PaperBook>()
                                .Sum(b => b.Price);

            if (paperBooksCost == 0)
            {
                return 0;
            }

            var isPaperBookFreeDelivery = paperBooksCost >= MinFreeDeliveryCost;
            
            if (isPaperBookFreeDelivery)
                return 0;

            else if (paperBooksCost != 0)
            {
                var totalCost = literature.Sum(b => b.Price) >= MinFreeDeliveryCost;
                if (totalCost)
                    return 0;
            }

            return deliveryPrice;
        }
    }

    class OrderItem
    {
        public ILiterature Book;
        public decimal initialPrice => Book.Price;
        public decimal Discount { get; set; }
        public bool HasPromoApplied { get; set; }
        public decimal FinalPrice => Math.Max(initialPrice - Discount, 0);

        public OrderItem(ILiterature book)
        {
            Book = book;
        }
    }

    class Order
    {
        public List<OrderItem> _orderItems;
        public decimal DeliveryPrice { get; set; }
        public decimal CommonDiscount { get; set; }

        public Order(List<ILiterature> books)
        {
            _orderItems = books
                            .Select(b => new OrderItem(b))
                            .ToList();
        }

        public decimal GetTotalFinalPrice()
        {
            return Math.Max(_orderItems.Sum(oi => oi.FinalPrice) + DeliveryPrice - CommonDiscount, 0);
        }
    }

    interface IPromo
    {
        void ApplyPromo(Order order);
    }

    class FreeBookPromo : IPromo
    {
        private ILiterature _freeBook;

        public FreeBookPromo(ILiterature freeBook)
        {
            _freeBook = freeBook;
        }

        public void ApplyPromo(Order order)
        {
            var foundBook = order._orderItems
                .Where(oi => !oi.HasPromoApplied)
                .FirstOrDefault(oi => oi.Book.Author == _freeBook.Author && oi.Book.Title == _freeBook.Title); // Equals(oi.Book, _freeBook)

            if (foundBook != null)
            {
                foundBook.Discount = foundBook.initialPrice;
                foundBook.HasPromoApplied = true;
            }
        }
    }

    class FreeDeliveryPromo : IPromo
    {
        public void ApplyPromo(Order order)
        {
            order.DeliveryPrice = 0;
        }
    }

    class PercentDiscountPromo : IPromo
    {
        private readonly int _percent;

        public PercentDiscountPromo(int percent)
        {
            // 1-100
            _percent = percent;
        }

        public void ApplyPromo(Order order)
        {
            foreach (var orderItem in order._orderItems)
            {
                orderItem.Discount += orderItem.initialPrice / 100 * _percent;
            }
        }
    }

    class AbsoluteDiscountPromo : IPromo
    {
        private readonly int _discount;

        public AbsoluteDiscountPromo(int discount)
        {
            // > 0
            _discount = discount;
        }

        public void ApplyPromo(Order order)
        {
            order.CommonDiscount += _discount;
        }
    }

    interface IAction
    {
        void ApplyAction(Order order, List<ILiterature> library);
    }

    interface IActionProvider
    {
        List<IAction> GetActiveActions();
    }

    class ActionProvider : IActionProvider
    {
        public List<IAction> GetActiveActions()
        {
            var actions = new List<IAction>
            {
                new TwoPaperBooksOneAuthor(),
                new ElectronicBookWithAudioBook()
            };

            return actions;
        }
    }

    class TwoPaperBooksOneAuthor : IAction
    {
        public void ApplyAction(Order order, List<ILiterature> library)
        {
            var paperAuthors = order._orderItems
                                    .Select(x => x.Book)
                                    .OfType<PaperBook>()
                                    .Select(oi => oi.Author)
                                    .ToList();

            var authorCounts = paperAuthors.GroupBy(i => i);

            foreach(var author in authorCounts)
            {
                if (author.Count() >= 2)
                {
                    var foundElectBook = library
                                            .OfType<ElectronicBook>()
                                            .FirstOrDefault(book => book.Author == author.Key);

                    if (foundElectBook != null)
                    {
                        var giftBook = new OrderItem(foundElectBook);
                        giftBook.Discount = giftBook.initialPrice;

                        order._orderItems.Add(giftBook);
                    }
                }
            }
        }
    }

    class ElectronicBookWithAudioBook : IAction
    {
        //При покупке аудио-книги её электронная версия в подарок

        public void ApplyAction(Order order, List<ILiterature> library)
        {
            var audioBooks = order._orderItems
                                        .Select(x => x.Book)
                                        .OfType<AudioBook>()
                                        .ToList();

            foreach(var ab in audioBooks)
            {
                var foundElectronicBook = library
                                            .OfType<ElectronicBook>()
                                            .FirstOrDefault(book => book.Author == ab.Author && book.Title == ab.Title);

                if (foundElectronicBook != null)
                {
                    var giftCopyBook = new OrderItem(foundElectronicBook);
                    giftCopyBook.Discount = giftCopyBook.initialPrice;
                    order._orderItems.Add(giftCopyBook);
                }
            }
        }
    }

    class Library
    {
        public List<ILiterature> libraryList;

        public Library()
        {
            libraryList = new List<ILiterature>();
        }

        public void AddBook(ILiterature book)
        {
            libraryList.Add(book);
        }
    }
}
