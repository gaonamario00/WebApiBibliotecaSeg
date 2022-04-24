﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiBibliotecaSeg;

#nullable disable

namespace WebApiBibliotecaSeg.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Autor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("id");

                    b.ToTable("autores");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Editorial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("autorId")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("autorId");

                    b.ToTable("editorial");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.LibroAutor", b =>
                {
                    b.Property<int>("libroId")
                        .HasColumnType("int");

                    b.Property<int>("autorId")
                        .HasColumnType("int");

                    b.Property<int>("orden")
                        .HasColumnType("int");

                    b.HasKey("libroId", "autorId");

                    b.HasIndex("autorId");

                    b.ToTable("libroAutor");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Libros", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("titulo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.ToTable("libros");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Editorial", b =>
                {
                    b.HasOne("WebApiBibliotecaSeg.Entidades.Autor", "autor")
                        .WithMany("editoriales")
                        .HasForeignKey("autorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("autor");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.LibroAutor", b =>
                {
                    b.HasOne("WebApiBibliotecaSeg.Entidades.Autor", "autor")
                        .WithMany("libroAutor")
                        .HasForeignKey("autorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBibliotecaSeg.Entidades.Libros", "libro")
                        .WithMany("libroAutor")
                        .HasForeignKey("libroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("autor");

                    b.Navigation("libro");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Autor", b =>
                {
                    b.Navigation("editoriales");

                    b.Navigation("libroAutor");
                });

            modelBuilder.Entity("WebApiBibliotecaSeg.Entidades.Libros", b =>
                {
                    b.Navigation("libroAutor");
                });
#pragma warning restore 612, 618
        }
    }
}