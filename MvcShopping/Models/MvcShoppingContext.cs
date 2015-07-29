using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcShopping.Models
{
    public class MvcShoppingContext:DbContext
    {
        public MvcShoppingContext()
            : base("name = DefaultConnection")
        { 
        }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<OrderHeader> Orders { get; set; }
        public DbSet<OrderDetail> OrderDwtailItems { get; set; }
    }
}