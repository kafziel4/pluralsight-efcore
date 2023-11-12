using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using Xunit.Abstractions;

namespace PubAppTest
{
    public class DatabaseTests
    {
        private readonly ITestOutputHelper _output;

        public DatabaseTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanInsertAuthorIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubTestData");

            using var context = new PubContext(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var author = new Author { FirstName = "a", LastName = "b" };
            context.Authors.Add(author);
            _output.WriteLine($"Before save: {author.AuthorId}");
            context.SaveChanges();
            _output.WriteLine($"After save: {author.AuthorId}");

            Assert.NotEqual(0, author.AuthorId);
        }
    }
}