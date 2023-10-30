namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();
            string result = GetBooksReleasedBefore(db, input);

            Console.WriteLine(result);

        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(book => book.Title )
                .ToArray();
            foreach (var book in books.OrderBy(x => x))
            {
                sb.AppendLine(book.ToString());
            }


            return sb.ToString();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {

            var books = context.Books
                .AsEnumerable()
                .Where(b => b.EditionType.ToString() == "Gold" && b.Copies < 5000 )
                .Select(book => book.Title)
                .ToArray();


            return string.Join(Environment.NewLine,books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var bookInfo = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var book in bookInfo)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)


        {
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.ReleaseDate!.Value.Year != year )
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine,books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            string[] jenres = input.ToLower().Split(' ');
            var books = context.BooksCategories
                .Select(b => new
                {
                    b.Book.Title,
                    b.Category.Name,
                })
                .Where(bn => jenres.Contains(bn.Name.ToLower()))
                .ToArray();
            foreach ( var book in books.OrderBy(b => b.Title))
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            
            var books = context.Books
                .AsEnumerable()
                .Select(b => new
                {
                    b.Title,
                    b.ReleaseDate,
                    b.EditionType,
                    b.Price
                })
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - {item.EditionType} - ${item.Price}");
            }

            return sb.ToString().Trim();
        }
    }
}


