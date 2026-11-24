// dotnet add package Microsoft.EntityFrameworkCore.Sqlite
// dotnet new console -n CSharp.CRUD.Practice -f net10.0
// dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 10.0.14

using Microsoft.EntityFrameworkCore;
using var db = new BookContext();
db.Database.EnsureCreated();

while (true)
{
    Console.WriteLine("\nBook Management System");
    Console.WriteLine("1. Add book");
    Console.WriteLine("2. Show books");
    Console.WriteLine("3. Edit book");
    Console.WriteLine("4. Delete book");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddBook(db);
            break;
        case "2":
            ShowBooks(db);
            break;
        case "3":
            EditBook(db);
            break;
        case "4":
            DeleteBook(db);
            break;
        case "0":
        case null:
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

static void AddBook(BookContext db)
{
    Console.Write("Title: ");
    string title = Console.ReadLine() ?? "";

    Console.Write("Author: ");
    string author = Console.ReadLine() ?? "";

    Console.Write("Price: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0 ||
        string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
    {
        Console.WriteLine("Invalid book information.");
        return;
    }

    Book book = new Book();
    book.Title = title;
    book.Author = author;
    book.Price = price;

    db.Books.Add(book); // INSERT INTO ...
    db.SaveChanges();
    Console.WriteLine("Book added.");
}

static void ShowBooks(BookContext db)
{
    List<Book> books = db.Books.ToList();

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (Book book in books)
    {
        Console.WriteLine($"ID: {book.Id} | Title: {book.Title} | Author: {book.Author} | Price: {book.Price}");
    }
}

static void EditBook(BookContext db)
{
    ShowBooks(db);
    Console.Write("Book ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    Book? book = db.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.Write("New title: ");
    string title = Console.ReadLine() ?? "";
    Console.Write("New author: ");
    string author = Console.ReadLine() ?? "";
    Console.Write("New price: ");

    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0 ||
        string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
    {
        Console.WriteLine("Invalid book information.");
        return;
    }

    book.Title = title;
    book.Author = author;
    book.Price = price;
    db.SaveChanges();
    Console.WriteLine("Book updated.");
}

static void DeleteBook(BookContext db)
{
    ShowBooks(db);
    Console.Write("Book ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    Book? book = db.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    db.Books.Remove(book);
    db.SaveChanges();
    Console.WriteLine("Book deleted.");
}


public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
}

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=books.db");
    }
}