using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System.Text;

//using (PubContext context = new())
//{
//    context.Database.EnsureCreated();
//}

PubContext _context = new();

//GetAuthors();
//AddAuthor();
//GetAuthors();
//AddAuthorWithBook();
//GetAuthorsWithBooks();
//QueryFilters();
//FindIt();
//AddSomeMoreAuthors();
//SkipAndTakeAuthors();
//SortAuthors();
//QueryAggregate();
//InsertAuthor();
//RetrieveAndUpdateAuthor();
//RetrieveAndUpdateMultipleAuthors();
//VariousOperations();
//CoordinatedRetrieveAndUpdateAuthor();
//DeleteAuthor();
//InsertMultipleAuthors();
//BulkAddUpdate();
//GetAuthorsParameterized();
//InsertNewAuthorWithNewBook();
//InsertNewAuthorWith2NewBook();
//AddNewBookToExistingAuthorInMemory();
//AddNewBookToExistingAuthorInMemoryViaBook();
//EagerLoadBooksWithAuthors();
//Projections();
//ExplicitLoadCollection();
//FilterUsingRelatedData();
//ModifyingRelatedDataWhenTracked();
//ModifyingRelatedDataWhenNotTracked();
//CascadeDeleteInActionWhenTracked();
//ConnectExistingArtistAndCoverObjects();
//CreateCoverWithExistingArtist();
//CreateNewCoverAndArtistTogether();
//RetrieveAnArtistWithTheirCovers();
//RetrieveACoverWithItsArtists();
//RetrieveAllArtistsWithTheirCovers();
//RetrieveAllArtistsWhoHaveCovers();
//UnassignAnArtistFromACover();
//DeleteAnObjectThatIsInARelationship();
//ReassignACover();
//GetAllBooksWithTheirCovers();
//OneToOneQueries();
//MultiLevelInclude();
//NewBookAndCover();
//AddCoverToExistingBook();
//ProtectingFromUniqueFK();
//SimpleRawSQL();
//RawSqlStoredProc();
//InterpolatedSqlStoredProc();
//GetAuthorsByArtist();
DeleCover(10);

void DeleCover(int coverId)
{
    var rowCount = _context.Database.ExecuteSqlRaw("DeleteCover {0}", coverId);
    Console.WriteLine(rowCount);
}

void GetAuthorsByArtist()
{
    var authorsArtist = _context.AuthorsByArtist.ToList();
    var oneAuthorsArtist = _context.AuthorsByArtist.FirstOrDefault();
    var kAuthorsArtist = _context.AuthorsByArtist
        .Where(a => a.Artist.StartsWith("K"))
        .ToList();

    var debugView = _context.ChangeTracker.DebugView.ShortView;
}

void RawSqlStoredProc()
{
    var authors = _context.Authors
        .FromSqlRaw("AuthorsPublishedInYearRange {0}, {1}", 2010, 2015)
        .ToList();
}

void InterpolatedSqlStoredProc()
{
    int start = 2010;
    int end = 2015;
    var authors = _context.Authors
        .FromSqlInterpolated($"AuthorsPublishedInYearRange {start}, {end}")
        .ToList();
}

void SimpleRawSQL()
{
    var authors = _context.Authors
        .FromSqlRaw("SELECT * FROM authors")
        .Include(a => a.Books)
        .ToList();
}

void ProtectingFromUniqueFK()
{
    var theNeverDesignIdeas = "A spirally spiral";
    var book = _context.Books
        .Include(b => b.Cover)
        .FirstOrDefault(b => b.BookId == 5);
    if (book.Cover != null)
    {
        book.Cover.DesignIdeas = theNeverDesignIdeas;
    }
    else
    {
        book.Cover = new Cover { DesignIdeas = theNeverDesignIdeas };
    }
    _context.SaveChanges();
}

void NewBookAndCover()
{
    var book = new Book
    {
        AuthorId = 1,
        Title = "Call Me Ishtar",
        PublishDate = new DateTime(1973, 1, 1)
    };
    book.Cover = new Cover { DesignIdeas = "Image of Ishtar?" };
    _context.Books.Add(book);
    _context.SaveChanges();
}

void AddCoverToExistingBook()
{
    var book = _context.Books.Find(7);
    book.Cover = new Cover { DesignIdeas = "A wool scouring pad" };
    _context.SaveChanges();
}

void MultiLevelInclude()
{
    var authorGraph = _context.Authors
        .AsNoTracking()
        .Include(a => a.Books)
        .ThenInclude(b => b.Cover)
        .ThenInclude(c => c.Artists)
        .FirstOrDefault(a => a.AuthorId == 1);

    Console.WriteLine($"{authorGraph?.FirstName} {authorGraph?.LastName}");
    foreach (var book in authorGraph.Books)
    {
        Console.WriteLine($"Book: {book.Title}");
        if (book.Cover != null)
        {
            Console.WriteLine($"Design Ideas: {book.Cover.DesignIdeas}");
            Console.Write("Artist(s): ");
            book.Cover.Artists.ForEach(a => Console.Write($"{a.LastName} "));
        }
    }
}

void OneToOneQueries()
{
    var booksWithCovers = _context.Books.Include(b => b.Cover).ToList();

    var booksThatHaveCover = _context.Books
        .Include(b => b.Cover)
        .Where(b => b.Cover != null)
        .ToList();

    var booksThatDoNotHaveCover = _context.Books.Where(b => b.Cover == null).ToList();

    var projection = _context.Books
        .Where(b => b.Cover != null)
        .Select(b =>
            new
            {
                b.Title,
                b.Cover.DesignIdeas
            })
        .ToList();
}

void GetAllBooksWithTheirCovers()
{
    var booksAndCovers = _context.Books.Include(b => b.Cover).ToList();
    booksAndCovers.ForEach(book =>
        Console.WriteLine($"{book.Title}{(book.Cover == null ? ": No cover yet" : $": {book.Cover.DesignIdeas}")}"));
}

void ReassignACover()
{
    var coverWithArtist4 = _context.Covers
        .Include(c =>
            c.Artists.Where(a => a.ArtistId == 4))
        .FirstOrDefault(c => c.CoverId == 5);

    coverWithArtist4.Artists.RemoveAt(0);
    var artist3 = _context.Artists.Find(3);
    coverWithArtist4.Artists.Add(artist3);
    _context.ChangeTracker.DetectChanges();
}

void DeleteAnObjectThatIsInARelationship()
{
    var cover = _context.Covers.Find(4);
    _context.Covers.Remove(cover);
    _context.SaveChanges();
}

void UnassignAnArtistFromACover()
{
    var coverWithArtist = _context.Covers
        .Include(c =>
            c.Artists.Where(a => a.ArtistId == 1))
        .FirstOrDefault(c => c.CoverId == 1);
    coverWithArtist.Artists.RemoveAt(0);
    //var artistToRemove = coverWithArtist.Artists[0];
    //coverWithArtist.Artists.Remove(artistToRemove);
    _context.ChangeTracker.DetectChanges();
    var debugView = _context.ChangeTracker.DebugView.ShortView;
    _context.SaveChanges();
}

void RetrieveAnArtistWithTheirCovers()
{
    var artisWithCover = _context.Artists
        .Include(a => a.Covers)
        .FirstOrDefault(a => a.ArtistId == 1);
}

void RetrieveACoverWithItsArtists()
{
    var coverWithArtists = _context.Covers
        .Include(c => c.Artists)
        .FirstOrDefault(c => c.CoverId == 1);
}

void RetrieveAllArtistsWithTheirCovers()
{
    var artistsWithCovers = _context.Artists.Include(a => a.Covers).ToList();
    foreach (var artist in artistsWithCovers)
    {
        Console.WriteLine($"{artist.FirstName} {artist.LastName}, Designs to work on:");
        var primaryArtistId = artist.ArtistId;
        if (!artist.Covers.Any())
        {
            Console.WriteLine("\tNo covers");
        }
        else
        {
            foreach (var cover in artist.Covers)
            {
                StringBuilder collaborators = new();
                foreach (var collabArtist in cover.Artists.Where(ca => ca.ArtistId != primaryArtistId))
                {
                    if (collaborators.Length > 0)
                    {
                        collaborators.Append(", ");
                    }
                    collaborators.Append($"{collabArtist.FirstName} {collabArtist.LastName}");
                }
                if (collaborators.Length > 0)
                {
                    collaborators.Insert(0, "(with ");
                    collaborators.Append(')');
                }
                Console.WriteLine($"\t*{cover.DesignIdeas} {collaborators}");
            }
        }
    }
}

void RetrieveAllArtistsWhoHaveCovers()
{
    var artistsWithCovers = _context.Artists.Where(a => a.Covers.Any()).ToList();
}

void CreateNewCoverAndArtistTogether()
{
    var newArtist = new Artist { FirstName = "Kir", LastName = "Talmage" };
    var newCover = new Cover { DesignIdeas = "We like birds!" };
    newArtist.Covers.Add(newCover);
    _context.Artists.Add(newArtist);
    _context.SaveChanges();
}

void CreateCoverWithExistingArtist()
{
    var artistA = _context.Artists.Find(1);
    var cover = new Cover { DesignIdeas = "Author has provided a photo" };
    cover.Artists.Add(artistA);
    _context.Covers.Add(cover);
    _context.SaveChanges();
}

void ConnectExistingArtistAndCoverObjects()
{
    var artistA = _context.Artists.Find(1);
    var artistB = _context.Artists.Find(2);
    var coverA = _context.Covers.Find(1);
    coverA.Artists.Add(artistA);
    coverA.Artists.Add(artistB);
    _context.SaveChanges();
}

void CascadeDeleteInActionWhenTracked()
{
    var author = _context.Authors
        .Include(a => a.Books)
        .FirstOrDefault(a => a.AuthorId == 5);
    _context.Authors.Remove(author);
    var state = _context.ChangeTracker.DebugView.ShortView;
    //_context.SaveChanges();
}

void ModifyingRelatedDataWhenTracked()
{
    var author = _context.Authors
        .Include(a => a.Books)
        .FirstOrDefault(a => a.AuthorId == 5);
    author.Books[0].BasePrice = 10.00m;
    //author.Books.Remove(author.Books[1]);
    _context.ChangeTracker.DetectChanges();
    var state = _context.ChangeTracker.DebugView.ShortView;
}

void ModifyingRelatedDataWhenNotTracked()
{
    var author = _context.Authors
        .Include(a => a.Books)
        .FirstOrDefault(a => a.AuthorId == 5);
    author.Books[0].BasePrice = 12.00m;

    var newContext = new PubContext();
    //newContext.Books.Update(author.Books[0]);
    newContext.Entry(author.Books[0]).State = EntityState.Modified;
    var state = _context.ChangeTracker.DebugView.ShortView;
    newContext.SaveChanges();
}

void FilterUsingRelatedData()
{
    var recentAuthors = _context.Authors
        .Where(a => a.Books.Any(b => b.PublishDate.Year >= 2015))
        .ToList();
}

void LazyLoadBooksFromAnAuthor()
{
    // Requires lazy loading to be set up
    var author = _context.Authors.FirstOrDefault(a => a.LastName == "Howey");
    foreach (var book in author.Books)
    {
        Console.WriteLine(book.Title);
    }
}

void ExplicitLoadCollection()
{
    var author = _context.Authors.FirstOrDefault(a => a.LastName == "Howey");
    _context.Entry(author).Collection(a => a.Books).Load();
    var books = _context.Entry(author)
        .Collection(a => a.Books)
        .Query()
        .Where(b => b.Title.Contains("Wool"))
        .ToList();
}

void Projections()
{
    var unknownTypes = _context.Authors
        .Select(a => new
        {
            AuthorId = a.AuthorId,
            Name = $"{a.FirstName} {a.LastName}",
            Books = a.Books.Where(b => b.PublishDate.Year < 2000).Count()
        })
        .ToList();
}

void EagerLoadBooksWithAuthors()
{
    //var authors = _context.Authors.Include(a => a.Books).ToList();
    var pubDateStart = new DateTime(2010, 1, 1);
    var authors = _context.Authors
        .Include(a => a.Books
            .Where(b => b.PublishDate >= pubDateStart)
            .OrderBy(b => b.Title))
        .ToList();
    authors.ForEach(a =>
    {
        Console.WriteLine($"{a.LastName} ({a.Books.Count})");
        a.Books.ForEach(b => Console.WriteLine($"\t{b.Title}"));
    });
}

void AddNewBookToExistingAuthorInMemory()
{
    var author = _context.Authors.FirstOrDefault(a => a.LastName == "Howey");
    if (author != null)
    {
        author.Books.Add(
            new Book { Title = "Wool", PublishDate = new DateTime(2012, 1, 1) });
    }
    _context.SaveChanges();
}

void AddNewBookToExistingAuthorInMemoryViaBook()
{
    var book = new Book
    {
        Title = "Shift",
        PublishDate = new DateTime(2012, 1, 1),
        //AuthorId = 5
    };
    book.Author = _context.Authors.Find(5);
    _context.Books.Add(book);
    _context.SaveChanges();
}

void InsertNewAuthorWithNewBook()
{
    var author = new Author { FirstName = "Lynda", LastName = "Rutledge" };
    author.Books.Add(new Book
    {
        Title = "West With Giraffes",
        PublishDate = new DateTime(2021, 2, 1)
    });
    _context.Authors.Add(author);
    _context.SaveChanges();
}

void InsertNewAuthorWith2NewBook()
{
    var author = new Author { FirstName = "Don", LastName = "Jones" };
    author.Books.AddRange(new List<Book>
    {
        new Book { Title = "The Never", PublishDate = new DateTime(2019, 12, 1) },
        new Book { Title = "Alabaster", PublishDate = new DateTime(2019, 4, 1) }
    });
    _context.Authors.Add(author);
    _context.SaveChanges();
}

void BulkAddUpdate()
{
    var newAuthors = new Author[]
    {
        new Author { FirstName = "Tsitsi", LastName = "Dangaremba"},
        new Author { FirstName = "Lisa", LastName = "See"},
        new Author { FirstName = "Zhang", LastName = "Ling"},
        new Author { FirstName = "Marilynne", LastName = "Robinson"}
    };
    _context.Authors.AddRange(newAuthors);
    var book = _context.Books.Find(2);
    book.Title = "Programming Entity Framework 2nd Edition";
    _context.SaveChanges();
}

void InsertMultipleAuthors()
{
    var authorList = new Author[]
    {
        new Author { FirstName = "Ruth", LastName = "Ozeki"},
        new Author { FirstName = "Sofia", LastName = "Segovia"},
        new Author { FirstName = "Ursula K.", LastName = "LeGuin"},
        new Author { FirstName = "Hugh", LastName = "Howey"},
        new Author { FirstName = "Isabelle", LastName = "Allende"}
    };
    _context.AddRange(authorList);
    _context.SaveChanges();
}

void InsertMultipleAuthorsPassedIn(List<Author> listOfAuthors)
{
    _context.Authors.AddRange(listOfAuthors);
    _context.SaveChanges();
}

void DeleteAuthor()
{
    var extraJL = _context.Authors.Find(1);
    if (extraJL != null)
    {
        _context.Authors.Remove(extraJL);
        _context.SaveChanges();
    }
}

void CoordinatedRetrieveAndUpdateAuthor()
{
    var author = FindThatAuthor(3);
    if (author?.FirstName == "Julie")
    {
        author.FirstName = "Julia";
        SaveThatAuthor(author);
    }
}

Author FindThatAuthor(int authorId)
{
    using var shortLivedContext = new PubContext();
    return shortLivedContext.Authors.Find(authorId);
}

void SaveThatAuthor(Author author)
{
    using var anotherShortLivedContext = new PubContext();
    anotherShortLivedContext.Authors.Update(author);
    anotherShortLivedContext.SaveChanges();
}

void VariousOperations()
{
    var author = _context.Authors.Find(2);
    author.LastName = "Newfoundland";
    var newAuthor = new Author { LastName = "Appleman", FirstName = "Dan" };
    _context.Authors.Add(newAuthor);
    _context.SaveChanges();
}

void RetrieveAndUpdateMultipleAuthors()
{
    var lermanAuthors = _context.Authors.Where(a => a.LastName == "Lehrman").ToList();
    foreach (var la in lermanAuthors)
    {
        la.LastName = "Lerman";
    }

    Console.WriteLine($"Before: {_context.ChangeTracker.DebugView.ShortView}");
    _context.ChangeTracker.DetectChanges();
    Console.WriteLine($"After: {_context.ChangeTracker.DebugView.ShortView}");

    _context.SaveChanges();
}

void RetrieveAndUpdateAuthor()
{
    var author = _context.Authors.FirstOrDefault(a => a.FirstName == "Julie" && a.LastName == "Lerman");
    if (author != null)
    {
        author.FirstName = "Julia";
        _context.SaveChanges();
    }
}

void InsertAuthor()
{
    var author = new Author { FirstName = "Frank", LastName = "Herbert" };
    _context.Authors.Add(author);
    _context.SaveChanges();
}

void QueryAggregate()
{
    var author = _context.Authors
        .OrderByDescending(a => a.FirstName)
        .FirstOrDefault(a => a.LastName == "Lerman");
}

void SortAuthors()
{
    var authorsByLastName = _context.Authors
        .OrderBy(a => a.LastName)
        .ThenBy(a => a.FirstName)
        .ToList();
    authorsByLastName.ForEach(a => Console.WriteLine($"{a.LastName}, {a.FirstName}"));

    var authorsDescending = _context.Authors
        .OrderByDescending(a => a.LastName)
        .ThenByDescending(a => a.FirstName)
        .ToList();
    Console.WriteLine("**Descending Last and First**");
    authorsDescending.ForEach(a => Console.WriteLine($"{a.LastName}, {a.FirstName}"));
}

void SkipAndTakeAuthors()
{
    var groupSize = 2;
    for (int i = 0; i < 5; i++)
    {
        var authors = _context.Authors
             .Skip(groupSize * i)
             .Take(groupSize)
             .ToList();
        Console.WriteLine($"Group {i}:");
        foreach (var author in authors)
        {
            Console.WriteLine($" {author.FirstName} {author.LastName}");
        }
    }
}

void AddSomeMoreAuthors()
{
    _context.Authors.Add(new Author { FirstName = "Rhoda", LastName = "Lerman" });
    _context.Authors.Add(new Author { FirstName = "Don", LastName = "Jones" });
    _context.Authors.Add(new Author { FirstName = "Jim", LastName = "Christopher" });
    _context.Authors.Add(new Author { FirstName = "Stephen", LastName = "Haunts" });
    _context.SaveChanges();
}

void FindIt()
{
    var authorIdTwo = _context.Authors.Find(2);
}

void QueryFilters()
{
    //var name = "Josie";
    //var authors = _context.Authors
    //    .Where(a => a.FirstName == name)
    //    .ToList();
    var filter = "L%";
    var authors = _context.Authors
        .Where(a => EF.Functions.Like(a.LastName, filter))
        .ToList();
}

void AddAuthorWithBook()
{
    var author = new Author { FirstName = "Julie", LastName = "Lerman" };
    author.Books.Add(new Book
    {
        Title = "Programming Entity Framework",
        PublishDate = new DateTime(2009, 1, 1)
    });
    author.Books.Add(new Book
    {
        Title = "Programming Entity Framework 2nd Ed",
        PublishDate = new DateTime(2010, 8, 1)
    });

    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthorsWithBooks()
{
    using var context = new PubContext();
    var authors = context.Authors.Include(a => a.Books).ToList();
    foreach (var author in authors)
    {
        Console.WriteLine($"{author.FirstName} {author.LastName}");
        foreach (var book in author.Books)
        {
            Console.WriteLine($"*{book.Title}");
        }
    }
}

void AddAuthor()
{
    var author = new Author { FirstName = "Josie", LastName = "Newf" };
    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthorsParameterized()
{
    var name = "Ozeki";
    var authors = _context.Authors.Where(a => a.LastName == name).ToList();
}

void GetAuthors()
{
    using var context = new PubContext();
    var authors = context.Authors.ToList();
    foreach (var author in authors)
    {
        Console.WriteLine($"{author.FirstName} {author.LastName}");
    }
}