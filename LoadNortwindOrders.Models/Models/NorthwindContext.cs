﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LoadNortwindOrders.Models.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServedCustomer> ServedCustomers { get; set; }

    public virtual DbSet<ViewOrderDate> ViewOrderDates { get; set; }

    public virtual DbSet<Vwventa> Vwventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServedCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ServedCustomers");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(31);
        });

        modelBuilder.Entity<ViewOrderDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ViewOrderDates");

            entity.Property(e => e.FullDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Vwventa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWVentas");

            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.CustomerId)
                .IsRequired()
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(31);
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}