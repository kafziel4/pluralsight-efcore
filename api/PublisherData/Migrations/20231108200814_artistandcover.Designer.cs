﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublisherData;

#nullable disable

namespace PublisherData.Migrations
{
    [DbContext(typeof(PubContext))]
    [Migration("20231108200814_artistandcover")]
    partial class artistandcover
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArtistCover", b =>
                {
                    b.Property<int>("ArtistsArtistId")
                        .HasColumnType("int");

                    b.Property<int>("CoversCoverId")
                        .HasColumnType("int");

                    b.HasKey("ArtistsArtistId", "CoversCoverId");

                    b.HasIndex("CoversCoverId");

                    b.ToTable("ArtistCover");
                });

            modelBuilder.Entity("PublisherDomain.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("PublisherDomain.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            FirstName = "Rhoda",
                            LastName = "Lerman"
                        },
                        new
                        {
                            AuthorId = 2,
                            FirstName = "Ruth",
                            LastName = "Ozeki"
                        },
                        new
                        {
                            AuthorId = 3,
                            FirstName = "Sofia",
                            LastName = "Segovia"
                        },
                        new
                        {
                            AuthorId = 4,
                            FirstName = "Ursula K.",
                            LastName = "LeGuin"
                        },
                        new
                        {
                            AuthorId = 5,
                            FirstName = "Hugh",
                            LastName = "Howey"
                        },
                        new
                        {
                            AuthorId = 6,
                            FirstName = "Isabelle",
                            LastName = "Allende"
                        });
                });

            modelBuilder.Entity("PublisherDomain.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            AuthorId = 1,
                            BasePrice = 0m,
                            PublishDate = new DateTime(1989, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "In God's Ear"
                        },
                        new
                        {
                            BookId = 2,
                            AuthorId = 2,
                            BasePrice = 0m,
                            PublishDate = new DateTime(2013, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "A Tale For the Time Being"
                        },
                        new
                        {
                            BookId = 3,
                            AuthorId = 3,
                            BasePrice = 0m,
                            PublishDate = new DateTime(1969, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Left Hand of Darkness"
                        });
                });

            modelBuilder.Entity("PublisherDomain.Cover", b =>
                {
                    b.Property<int>("CoverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoverId"), 1L, 1);

                    b.Property<string>("DesignIdeas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DigitalOnly")
                        .HasColumnType("bit");

                    b.HasKey("CoverId");

                    b.ToTable("Covers");
                });

            modelBuilder.Entity("ArtistCover", b =>
                {
                    b.HasOne("PublisherDomain.Artist", null)
                        .WithMany()
                        .HasForeignKey("ArtistsArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublisherDomain.Cover", null)
                        .WithMany()
                        .HasForeignKey("CoversCoverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PublisherDomain.Book", b =>
                {
                    b.HasOne("PublisherDomain.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("PublisherDomain.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
