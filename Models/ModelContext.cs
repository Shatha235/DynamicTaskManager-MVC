using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskProject.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mytask> Mytasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=Task;PASSWORD=12345678;DATA SOURCE=localhost:1521/XEPDB1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("TASK")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Mytask>(entity =>
        {
            entity.HasKey(e => e.Taskid).HasName("SYS_C008558");

            entity.ToTable("MYTASK");

            entity.Property(e => e.Taskid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TASKID");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Duedate)
                .HasColumnType("DATE")
                .HasColumnName("DUEDATE");
            entity.Property(e => e.Priority)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PRIORITY");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Taskdescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TASKDESCRIPTION");
            entity.Property(e => e.Taskname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TASKNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
