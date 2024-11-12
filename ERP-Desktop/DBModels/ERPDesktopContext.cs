using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ERP_Desktop.DBModels;

public partial class ERPDesktopContext : DbContext
{
    public ERPDesktopContext()
    {
    }

    public ERPDesktopContext(DbContextOptions<ERPDesktopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tblCategoryMaster> tblCategoryMaster { get; set; }

    public virtual DbSet<tblInvoiceLine> tblInvoiceLine { get; set; }

    public virtual DbSet<tblInvoiceMaster> tblInvoiceMaster { get; set; }

    public virtual DbSet<tblProductMaster> tblProductMaster { get; set; }

    public virtual DbSet<tblPurchaseOrderLine> tblPurchaseOrderLine { get; set; }

    public virtual DbSet<tblPurchaseOrderMaster> tblPurchaseOrderMaster { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ERP-Desktop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<tblCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.cat_code).HasName("PK__tblCateg__13FB48BCED124A51");

            entity.Property(e => e.cat_code).HasMaxLength(255);
            entity.Property(e => e.cat_name).HasMaxLength(255);
        });

        modelBuilder.Entity<tblInvoiceLine>(entity =>
        {
            entity.HasKey(e => e.line_id).HasName("PK__tblInvoi__F5AE5F62A0620519");

            entity.Property(e => e.current_price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.line_total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.old_price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.invoice).WithMany(p => p.tblInvoiceLine)
                .HasForeignKey(d => d.invoice_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblInvoic__invoi__3D5E1FD2");

            entity.HasOne(d => d.prod_codeNavigation).WithMany(p => p.tblInvoiceLine)
                .HasForeignKey(d => d.prod_code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblInvoic__prod___3E52440B");
        });

        modelBuilder.Entity<tblInvoiceMaster>(entity =>
        {
            entity.HasKey(e => e.invoice_id).HasName("PK__tblInvoi__F58DFD49ADD1C064");

            entity.Property(e => e.invoice_number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.total_amount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<tblProductMaster>(entity =>
        {
            entity.HasKey(e => e.prod_code).HasName("PK__tblProdu__4C932104B1FAEF53");

            entity.Property(e => e.prod_cat).HasMaxLength(255);
            entity.Property(e => e.prod_code_usergen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.prod_cost_price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.prod_desc).HasMaxLength(255);
            entity.Property(e => e.prod_name).HasMaxLength(100);
            entity.Property(e => e.prod_sales_price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.stock)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.prod_catNavigation).WithMany(p => p.tblProductMaster)
                .HasForeignKey(d => d.prod_cat)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<tblPurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.line_id).HasName("PK__tblPurch__F5AE5F626DA821F3");

            entity.Property(e => e.line_total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.unit_price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.prod_codeNavigation).WithMany(p => p.tblPurchaseOrderLine)
                .HasForeignKey(d => d.prod_code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderLine_ProductMaster");

            entity.HasOne(d => d.purchase_order).WithMany(p => p.tblPurchaseOrderLine)
                .HasForeignKey(d => d.purchase_order_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderLine_PurchaseOrderMaster");
        });

        modelBuilder.Entity<tblPurchaseOrderMaster>(entity =>
        {
            entity.HasKey(e => e.purchase_order_id).HasName("PK__tblPurch__AFCA88E67A150A3A");

            entity.Property(e => e.purchase_order_number).HasMaxLength(50);
            entity.Property(e => e.status).HasDefaultValue(1);
            entity.Property(e => e.total_amount).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
