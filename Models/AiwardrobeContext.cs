﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AI_Wardrobe.Models;

public partial class AiwardrobeContext : DbContext
{
    public AiwardrobeContext()
    {
    }

    public AiwardrobeContext(DbContextOptions<AiwardrobeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemGender> ItemGenders { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<RegisteredUser> RegisteredUsers { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1; Port=5432;Database=aiwardrobe; Username=postgres;Password=P@ssw0rd!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Addressid).HasName("Address_pkey");

            entity.ToTable("Address");

            entity.Property(e => e.Addressid).HasColumnName("addressid");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(255)
                .HasColumnName("address2");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Fkuserid).HasColumnName("fkuserid");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(255)
                .HasColumnName("postalcode");
            entity.Property(e => e.Province)
                .HasMaxLength(255)
                .HasColumnName("province");

            entity.HasOne(d => d.Fkuser).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.Fkuserid)
                .HasConstraintName("address_user_id_fk");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("Item_pkey");

            entity.ToTable("Item");

            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Fkitemgenderid).HasColumnName("fkitemgenderid");
            entity.Property(e => e.Fksizeid).HasColumnName("fksizeid");
            entity.Property(e => e.Fktypeid).HasColumnName("fktypeid");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .HasColumnName("imageurl");
            entity.Property(e => e.Itemdescription)
                .HasMaxLength(1000)
                .HasColumnName("itemdescription");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .HasColumnName("itemname");
            entity.Property(e => e.Itemprice).HasColumnName("itemprice");
            entity.Property(e => e.Itemtype)
                .HasMaxLength(255)
                .HasColumnName("itemtype");

            entity.HasOne(d => d.Fkitemgender).WithMany(p => p.Items)
                .HasForeignKey(d => d.Fkitemgenderid)
                .HasConstraintName("item_item_gender_id_fk");

            entity.HasOne(d => d.Fksize).WithMany(p => p.Items)
                .HasForeignKey(d => d.Fksizeid)
                .HasConstraintName("item_item_size_id_fk");

            entity.HasOne(d => d.Fktype).WithMany(p => p.Items)
                .HasForeignKey(d => d.Fktypeid)
                .HasConstraintName("item_item_type_id_fk");
        });

        modelBuilder.Entity<ItemGender>(entity =>
        {
            entity.HasKey(e => e.Itemgenderid).HasName("ItemGender_pkey");

            entity.ToTable("ItemGender");

            entity.Property(e => e.Itemgenderid).HasColumnName("itemgenderid");
            entity.Property(e => e.Itemgenderdescription)
                .HasMaxLength(255)
                .HasColumnName("itemgenderdescription");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.Itemtypeid).HasName("ItemTypes_pkey");

            entity.Property(e => e.Itemtypeid).HasColumnName("itemtypeid");
            entity.Property(e => e.Itemtypedescription)
                .HasMaxLength(255)
                .HasColumnName("itemtypedescription");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Fkuserid).HasColumnName("fkuserid");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(255)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("orderstatus");

            entity.HasOne(d => d.Fkuser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Fkuserid)
                .HasConstraintName("order_user_id_fk");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .HasConstraintName("orders_status_fk");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Orderdetailsid).HasName("OrderDetails_pkey");

            entity.Property(e => e.Orderdetailsid).HasColumnName("orderdetailsid");
            entity.Property(e => e.Fkitemid).HasColumnName("fkitemid");
            entity.Property(e => e.Fkorderid).HasColumnName("fkorderid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Fkitem).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Fkitemid)
                .HasConstraintName("order_details_item_id_fk");

            entity.HasOne(d => d.Fkorder).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Fkorderid)
                .HasConstraintName("order_details_order_id_fk");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Status).HasName("OrderStatus_pkey");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<RegisteredUser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("RegisteredUser_pkey");

            entity.ToTable("RegisteredUser");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Sizeid).HasName("Size_pkey");

            entity.ToTable("Size");

            entity.Property(e => e.Sizeid).HasColumnName("sizeid");
            entity.Property(e => e.Sizedescription)
                .HasMaxLength(255)
                .HasColumnName("sizedescription");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("Transaction_pkey");

            entity.ToTable("Transaction");

            entity.Property(e => e.Transactionid).HasColumnName("transactionid");
            entity.Property(e => e.Currency)
                .HasMaxLength(20)
                .HasColumnName("currency");
            entity.Property(e => e.Fkorderid).HasColumnName("fkorderid");
            entity.Property(e => e.Payeremail)
                .HasMaxLength(100)
                .HasColumnName("payeremail");
            entity.Property(e => e.Payername)
                .HasMaxLength(100)
                .HasColumnName("payername");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(20)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Paypaltransactionid)
                .HasMaxLength(100)
                .HasColumnName("paypaltransactionid");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");
            entity.Property(e => e.Transactiondate).HasColumnName("transactiondate");
            entity.Property(e => e.Transactionstatus)
                .HasMaxLength(255)
                .HasColumnName("transactionstatus");

            entity.HasOne(d => d.Fkorder).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Fkorderid)
                .HasConstraintName("transaction_order_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
