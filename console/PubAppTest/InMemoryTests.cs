using Microsoft.EntityFrameworkCore;
using PublisherConsole;
using PublisherData;
using PublisherDomain;
using Xunit.Abstractions;

namespace PubAppTest
{
    public class InMemoryTests
    {
        private readonly ITestOutputHelper _output;

        public InMemoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanInsertAuthorIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseInMemoryDatabase(
                nameof(CanInsertAuthorIntoDatabase));

            using var context = new PubContext(builder.Options);
            var author = new Author { FirstName = "a", LastName = "b" };
            context.Authors.Add(author);

            Assert.Equal(EntityState.Added, context.Entry(author).State);
        }

        [Fact]
        public void InsertAuthorReturnsCorrectResultNumber()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseInMemoryDatabase(
                nameof(InsertAuthorReturnsCorrectResultNumber));
            var authorList = new List<ImportAuthorDTO>()
            {
                new("a", "b"),
                new("c", "d"),
                new("e", "f")
            };

            var dataLogic = new DataLogic(new PubContext(builder.Options));
            var result = dataLogic.ImportAuthors(authorList);
            Assert.Equal(authorList.Count, result);
        }
    }
}