using Microsoft.EntityFrameworkCore;
using OngProject.Common;
using OngProject.Core.Entities;
using System;

namespace OngProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //de esta manera la propiedad email de los usuarios sera unica y no se podra repetir
            builder.Entity<User>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            SeedRoles(builder);
            SeedUsers(builder);
            SeedActivities(builder);
            SeedCategories(builder);
            SeedNews(builder);
            SeedComments(builder);
            SeedContacts(builder);
            SeedMembers(builder);
            SeedOrganizations(builder);
            SeedTestimonials(builder);
            SeedSlides(builder);
        }

        public DbSet<Activities> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Member> Members { get; set; }  
        public DbSet<News> News { get; set; }   
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Role> Roles { get; set; }  
        public DbSet<Slides> Slides { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<User> Users { get; set; }

        private void SeedActivities(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Activities>().HasData(
                    new Activities
                    {
                        Id = i,
                        Name = "Activity " + i,
                        Image = "ImageActivities" + i + ".jpg",
                        Content = "Content from activity " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedCategories(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Category>().HasData(
                    new Category
                    {
                        Id = i,
                        Name = "Activity " + i,
                        Image = "ImageCategories" + i + ".jpg",
                        Description = "Description for Category " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedComments(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Comments>().HasData(
                    new Comments
                    {
                        Id = i,
                        UserId = i,
                        NewId = i,
                        Body = "Body from Comment " + i,
                        CreatedAt = DateTime.Now
                    }
                ); ;
            }
        }
        private void SeedContacts(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Contacts>().HasData(
                    new Contacts
                    {
                        Id = i,
                        Name = "Contact " + i,
                        Phone = 381 + i,
                        Email = "Email for contact " + i,
                        Message = "Message from contact "+ i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedMembers(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Member>().HasData(
                    new Member
                    {
                        Id = i,
                        Name = "Member " + i,
                        Image = "ImageMembers" + i + ".jpg",
                        FacebookUrl = "FacebookURL for member " + i,
                        InstagramUrl = "InstagramURL for member " + i,
                        LinkedinUrl = "LinkdInURL for member " + i,
                        Description = "Description for member " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedOrganizations(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Organizations>().HasData(
                    new Organizations
                    {
                        Id = i,
                        Name = "Organization " + i,
                        Image = "ImageOrganizations" + i + ".jpg",
                        Address = "Address for Organization " + i,
                        Phone = 381 + i,
                        Email = "Email for Organization " + i,
                        WelcomeText = "WelcomeText for Organization " + i,
                        AboutUsText = "AboutUsText for Organization " + i,
                        CreatedAt = DateTime.Now,
                        FacebookUrl = "FacebookUrl" + i,
                        InstagramUrl = "InstagramUrl" + i,
                        LinkedinUrl = "InstragramUrl" + i
                    }
                );
            }
        }
        private void SeedTestimonials(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Testimonials>().HasData(
                    new Testimonials
                    {
                        Id = i,
                        Name = "Organization " + i,
                        Image = "Image for testimonial " + i,
                        Content = "Contetnt for Testimonial " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedUsers(ModelBuilder modelBuilder)
        {
            for (int i = 1; i <= 2; i++)
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = i,
                        FirstName = "User" + i,
                        LastName = "LastName for user " + i,
                        Email = "Email for user " + i,
                        Password = Encrypt.GetSHA256("123456"),
                        Photo = "Photo for user " + i,
                        RoleId = 1,
                        CreatedAt = DateTime.Now
                    }
                );
            }
            for (int i = 3; i <= 10; i++)
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = i,
                        FirstName = "User" + i,
                        LastName = "LastName for user " + i,
                        Email = "Email for user " + i,
                        Password = Encrypt.GetSHA256("123456"),
                        Photo = "Photo for user " + i,
                        RoleId = 2,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedSlides(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Slides>().HasData(
                    new Slides
                    {
                        Id = i,
                        ImageUrl = "ImageUrl for Slide " + i,
                        Text = "Text for Slide " + i,
                        Order = i,
                        OrganizationId = i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedNews(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<News>().HasData(
                    new News
                    {
                        Id = i,
                        Name = "New " + i,
                        Content = "Content for New " + i,
                        Image = "Image for New " + i,
                        CategoryId = i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Administrator",
                        Description = "Description User Admin",
                        CreatedAt = DateTime.Now
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Standard",
                        Description = "Description User Standard"
                    }
                );
        }
    }
}
