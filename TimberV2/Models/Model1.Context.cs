﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimberV2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TimberV2Entities2 : DbContext
    {
        public TimberV2Entities2()
            : base("name=TimberV2Entities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<DraftCart> DraftCarts { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<OrderedItem> OrderedItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
