using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublisherData.Migrations
{
    public partial class addview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW AuthorsByArtist
                    AS
                    SELECT Artists.FirstName + ' ' + Artists.LastName AS Artist,
                            Authors.FirstName + ' ' + Authors.LastName AS Author
                    FROM ARTISTS LEFT JOIN
                    ArtistCover ON Artists.ArtistId = ArtistCover.ArtistsArtistId LEFT JOIN
                    Covers ON ArtistCover.CoversCoverId = Covers.CoverId LEFT JOIN
                    Books ON Books.BookId = Covers.BookId LEFT JOIN
                    Authors on Books.AuthorId = Authors.AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW AuthorsByArtist");
        }
    }
}
