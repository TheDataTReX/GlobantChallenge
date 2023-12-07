using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GlobantChallenge.Models;

public partial class GlobantChallengeContext : DbContext
{
    public GlobantChallengeContext()
    {
    }

    public GlobantChallengeContext(DbContextOptions<GlobantChallengeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<HiredEmployee> HiredEmployees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departme__3213E83FBD7ECA4F");

            entity.ToTable("departments");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Department1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("department");
        });

        modelBuilder.Entity<HiredEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__hired_em__3213E83F58807ED0");

            entity.ToTable("hired_employees");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.HireDate)
                .HasColumnName("datetime")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Department).WithMany(p => p.HiredEmployees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__hired_emp__depar__286302EC");

            entity.HasOne(d => d.Job).WithMany(p => p.HiredEmployees)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__hired_emp__job_i__29572725");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__jobs__3213E83F96F7BB39");

            entity.ToTable("jobs");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Job1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("job");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
