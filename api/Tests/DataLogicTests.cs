using Microsoft.EntityFrameworkCore;
using PubAPI;
using PublisherData;
using PublisherDomain;

namespace Tests
{
    public class DataLogicTests
    {
        [Fact]
        public async Task CanGetAnAuthorById()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseInMemoryDatabase(
                nameof(CanGetAnAuthorById));
            int seededId = SeedOneAuthor(builder.Options);

            // Act
            using var context = new PubContext(builder.Options);
            var bizLogic = new DataLogic(context);
            var authorRetrieved = await bizLogic.GetAuthorById(seededId);

            // Assert
            Assert.Equal(seededId, authorRetrieved.AuthorId);
        }

        private int SeedOneAuthor(DbContextOptions<PubContext> options)
        {
            using var seedContext = new PubContext(options);
            var author = new Author { FirstName = "a", LastName = "b" };
            seedContext.Authors.Add(author);
            seedContext.SaveChanges();
            return author.AuthorId;
        }
    }
}