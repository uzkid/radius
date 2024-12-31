using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        Console.WriteLine("Kutubxona ilovasiga xush kelibsiz!");
        while (true)
        {
            Console.WriteLine("\n1. Ro'yxatdan o'tish\n2. Kirish\n3. Chiqish");
            Console.Write("Tanlovingiz: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    library.RegisterReader();
                    break;
                case "2":
                    Reader reader = library.Login();
                    if (reader != null)
                    {
                        UserMenu(library, reader);
                    }
                    break;
                case "3":
                    Console.WriteLine("Dastur tugatildi.");
                    return;
                default:
                    Console.WriteLine("Noto'g'ri tanlov! Qaytadan urinib ko'ring.");
                    break;
            }
        }
    }

    static void UserMenu(Library library, Reader reader)
    {
        while (true)
        {
            Console.WriteLine("\nMenyular:");
            Console.WriteLine("1. Kitoblar ro'yxatini ko'rish");
            Console.WriteLine("2. Kitob haqida ma'lumot olish");
            Console.WriteLine("3. Kitob olish");
            Console.WriteLine("4. Kitob qaytarish");
            Console.WriteLine("5. Chiqish");
            Console.Write("Tanlovingiz: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    library.ShowBooks();
                    break;
                case "2":
                    library.ShowBookDetails();
                    break;
                case "3":
                    library.BorrowBook(reader);
                    break;
                case "4":
                    library.ReturnBook(reader);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Noto'g'ri tanlov! Qaytadan urinib ko'ring.");
                    break;
            }
        }
    }
}

class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public int TotalQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public List<string> Borrowers { get; set; } = new List<string>();
}

class Reader
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public int BooksBorrowed { get; set; }
    public List<string> BorrowedBooks { get; set; } = new List<string>();
}

class Library
{
    private List<Book> Books;
    private List<Reader> Readers;

    private const string BooksFile = "books.json";
    private const string ReadersFile = "readers.json";

    public Library()
    {
        Books = LoadData<Book>(BooksFile) ?? new List<Book>();
        Readers = LoadData<Reader>(ReadersFile) ?? new List<Reader>();
    }

    public void RegisterReader()
    {
        Console.Write("Login: ");
        string login = Console.ReadLine();
        Console.Write("Parol: ");
        string password = Console.ReadLine();
        Console.Write("Ismi: ");
        string firstName = Console.ReadLine();
        Console.Write("Familyasi: ");
        string lastName = Console.ReadLine();
        Console.Write("Yoshi: ");
        int age = int.Parse(Console.ReadLine());

        Reader newReader = new Reader
        {
            Id = Readers.Count + 1,
            Login = login,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            Age = age
        };

        Readers.Add(newReader);
        SaveData(Readers, ReadersFile);
        Console.WriteLine("Ro'yxatdan o'tish muvaffaqiyatli yakunlandi!");
    }

    public Reader Login()
    {
        Console.Write("Login: ");
        string login = Console.ReadLine();
        Console.Write("Parol: ");
        string password = Console.ReadLine();

        Reader reader = Readers.FirstOrDefault(r => r.Login == login && r.Password == password);
        if (reader != null)
        {
            Console.WriteLine($"Xush kelibsiz, {reader.FirstName}!");
            return reader;
        }

        Console.WriteLine("Login yoki parol noto'g'ri!");
        return null;
    }

    public void ShowBooks()
    {
        Console.WriteLine("\nKutubxonadagi kitoblar:");
        foreach (var book in Books)
        {
            Console.WriteLine($"ID: {book.Id}, Nomi: {book.Name}, Muallif: {book.Author}, Mavjud: {book.AvailableQuantity}/{book.TotalQuantity}");
        }
    }

    public void ShowBookDetails()
    {
        Console.Write("Ma'lumot olish uchun kitob ID sini kiriting: ");
        int bookId = int.Parse(Console.ReadLine());

        Book book = Books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            Console.WriteLine($"ID: {book.Id}, Nomi: {book.Name}, Janri: {book.Genre}, Muallif: {book.Author}");
            Console.WriteLine($"Jami nusxalar: {book.TotalQuantity}, Mavjud: {book.AvailableQuantity}");
            Console.WriteLine("O'qiyotganlar: " + string.Join(", ", book.Borrowers));
        }
        else
        {
            Console.WriteLine("Kitob topilmadi!");
        }
    }

    public void BorrowBook(Reader reader)
    {
        Console.Write("Olish uchun kitob ID sini kiriting: ");
        int bookId = int.Parse(Console.ReadLine());

        Book book = Books.FirstOrDefault(b => b.Id == bookId);
        if (book != null && book.AvailableQuantity > 0)
        {
            book.AvailableQuantity--;
            book.Borrowers.Add(reader.FirstName);

            reader.BooksBorrowed++;
            reader.BorrowedBooks.Add(book.Name);

            SaveData(Books, BooksFile);
            SaveData(Readers, ReadersFile);

            Console.WriteLine($"Siz {book.Name} kitobini oldingiz.");
        }
        else
        {
            Console.WriteLine("Kitob mavjud emas!");
        }
    }

    public void ReturnBook(Reader reader)
    {
        Console.Write("Qaytarish uchun kitob nomini kiriting: ");
        string bookName = Console.ReadLine();

        Book book = Books.FirstOrDefault(b => b.Name == bookName);
        if (book != null && reader.BorrowedBooks.Contains(bookName))
        {
            book.AvailableQuantity++;
            book.Borrowers.Remove(reader.FirstName);

            reader.BooksBorrowed--;
            reader.BorrowedBooks.Remove(bookName);

            SaveData(Books, BooksFile);
            SaveData(Readers, ReadersFile);

            Console.WriteLine($"Siz {book.Name} kitobini qaytardingiz.");
        }
        else
        {
            Console.WriteLine("Sizda bu kitob mavjud emas!");
        }
    }

    private List<T> LoadData<T>(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        return null;
    }

    private void SaveData<T>(List<T> data, string fileName)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(fileName, json);
    }
}
