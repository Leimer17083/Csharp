using System;
using System.Collections.Generic;
using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var library = new Library();
        library.AddBook(new Book(1, "Кобзар", "Тарас Шевченко"));
        library.AddBook(new Book(2, "Лісова пісня", "Леся Українка"));
        library.AddBook(new Book(3, "Тигролови", "Іван Багряний"));

        library.AddReader(new Reader(1, "Олена"));
        library.AddReader(new Reader(2, "Андрій"));

        library.ShowAllBooks();
        library.ShowAllReaders();
        library.ShowAvailableBooks();
        library.ShowBorrowedBooks();

        Console.WriteLine("\n--- Видаємо книгу ---");
        library.BorrowBook(readerId: 1, bookId: 2);
        library.ShowAllReaders();

        Console.WriteLine("\n--- Спроба видати ту саму книгу іншому читачеві ---");
        library.BorrowBook(readerId: 2, bookId: 2);

        Console.WriteLine("\n--- Повернення книги ---");
        library.ReturnBook(readerId: 1, bookId: 2);

        Console.WriteLine("\n--- Доступні книги після повернення ---");
        library.ShowAvailableBooks();
    }
}

class Book
{
    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public bool IsAvailable { get; private set; } = true;
    public Book(int id, string title, string author)
    {
        Id = id;
        Title = title;
        Author = author;
    }
    internal void MarkAsBorrowed()
    {
        IsAvailable = false;
    }
    internal void MarkAsReturned()
    {
        IsAvailable = true;
    }
}

class Reader
{
    private readonly List<Book> _borrowedBooks = new List<Book>();
    public int Id { get; }
    public string Name { get; }
    public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks.AsReadOnly();

    public Reader(int id, string name)
    {
        Id = id;
        Name = name;
    }

    internal bool HasBorrowedBook(int bookId)
    {
        return _borrowedBooks.Exists(book => book.Id == bookId);
    }

    internal void AddBorrowedBook(Book book)
    {
        _borrowedBooks.Add(book);
    }

    internal bool RemoveBorrowedBook(int bookId)
    {
        Book borrowedBook = _borrowedBooks.Find(book => book.Id == bookId);

        if (borrowedBook == null)
            return false;

        _borrowedBooks.Remove(borrowedBook);
        return true;
    }
}

class Library
{
    private readonly Dictionary<int, Book> _books = new Dictionary<int, Book>();
    private readonly Dictionary<int, Reader> _readers = new Dictionary<int, Reader>();

    public void AddBook(Book book)
    {
        if (book.Id <= 0 || _books.ContainsKey(book.Id))
        {
            Console.WriteLine($"Помилка: книгу з ID {book.Id} не додано. ID має бути додатним і унікальним.");
            return;
        }

        _books.Add(book.Id, book);
    }

    public void AddReader(Reader reader)
    {
        if (reader.Id <= 0 || _readers.ContainsKey(reader.Id))
        {
            Console.WriteLine($"Помилка: читача з ID {reader.Id} не додано. ID має бути додатним і унікальним.");
            return;
        }

        _readers.Add(reader.Id, reader);
    }

    public void BorrowBook(int readerId, int bookId)
    {
        if (!_readers.TryGetValue(readerId, out Reader reader))
        {
            Console.WriteLine($"Помилка: читача з ID {readerId} не знайдено.");
            return;
        }

        if (!_books.TryGetValue(bookId, out Book book))
        {
            Console.WriteLine($"Помилка: книгу з ID {bookId} не знайдено.");
            return;
        }

        if (!book.IsAvailable)
        {
            Console.WriteLine($"Помилка: книга «{book.Title}» вже видана.");
            return;
        }

        book.MarkAsBorrowed();
        reader.AddBorrowedBook(book);

        Console.WriteLine($"Читач {reader.Name} взяв книгу «{book.Title}».");
    }

    public void ReturnBook(int readerId, int bookId)
    {
        if (!_readers.TryGetValue(readerId, out Reader reader))
        {
            Console.WriteLine($"Помилка: читача з ID {readerId} не знайдено.");
            return;
        }

        if (!_books.TryGetValue(bookId, out Book book))
        {
            Console.WriteLine($"Помилка: книгу з ID {bookId} не знайдено.");
            return;
        }

        if (!reader.HasBorrowedBook(bookId))
        {
            Console.WriteLine($"Помилка: читач {reader.Name} не орендував книгу «{book.Title}».");
            return;
        }

        reader.RemoveBorrowedBook(bookId);
        book.MarkAsReturned();

        Console.WriteLine($"Читач {reader.Name} повернув книгу «{book.Title}».");
    }

    public void ShowAllBooks()
    {
        Console.WriteLine("\n--- Усі книги ---");

        foreach (Book book in _books.Values)
        {
            string status = book.IsAvailable ? "доступна" : "в оренді";
            Console.WriteLine($"ID: {book.Id}, «{book.Title}», автор: {book.Author}, статус: {status}");
        }
    }

    public void ShowAllReaders()
    {
        Console.WriteLine("\n--- Усі читачі ---");

        foreach (Reader reader in _readers.Values)
        {
            Console.Write($"ID: {reader.Id}, ім’я: {reader.Name}, книги: ");

            if (reader.BorrowedBooks.Count == 0)
            {
                Console.WriteLine("немає");
                continue;
            }

            for (int i = 0; i < reader.BorrowedBooks.Count; i++)
            {
                if (i > 0)
                    Console.Write(", ");

                Console.Write($"«{reader.BorrowedBooks[i].Title}»");
            }

            Console.WriteLine();
        }
    }

    public void ShowAvailableBooks()
    {
        Console.WriteLine("\n--- Доступні книги ---");
        bool found = false;

        foreach (Book book in _books.Values)
        {
            if (book.IsAvailable)
            {
                Console.WriteLine($"ID: {book.Id}, «{book.Title}» — {book.Author}");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("Доступних книг немає.");
    }

    public void ShowBorrowedBooks()
    {
        Console.WriteLine("\n--- Книги в оренді ---");
        bool found = false;

        foreach (Reader reader in _readers.Values)
        {
            foreach (Book book in reader.BorrowedBooks)
            {
                Console.WriteLine($"«{book.Title}» — читач: {reader.Name}");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("Зараз книги не орендуються.");
    }
}