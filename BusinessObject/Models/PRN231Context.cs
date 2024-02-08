using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class PRN231Context : DbContext
    {
        public PRN231Context(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("DB"));
            }
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogRating> BlogRating { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Whislist> Whislists { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
    }
}
