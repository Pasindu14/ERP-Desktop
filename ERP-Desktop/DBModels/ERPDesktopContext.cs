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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ERP-Desktop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tblCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.cat_code).HasName("PK__tblCateg__13FB48BCED124A51");

            entity.Property(e => e.cat_code).HasMaxLength(255);
            entity.Property(e => e.cat_name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
