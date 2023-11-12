﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublisherData.Migrations
{
    public partial class addstoredproc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE PROCEDURE dbo.AuthorsPublishedInYearRange
                    @yearstart int,
                    @yearend int
                AS
                SELECT Distinct Authors.* FROM authors
                LEFT JOIN Books ON Authors.authorId = books.authorId
                WHERE Year(books.PublishDate) >= @yearstart AND Year(books.PublishDate) <= @yearend");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP PROCEDURE dbo.AuthorsPublishedInYearRange");
        }
    }
}