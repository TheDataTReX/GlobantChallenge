﻿// <auto-generated />
using System;
using GlobantChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GlobantChallenge.Migrations
{
    [DbContext(typeof(GlobantChallengeContext))]
    [Migration("20231206071836_UpdateDateTimeColumn")]
    partial class UpdateDateTimeColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GlobantChallenge.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Department1")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("department");

                    b.HasKey("Id")
                        .HasName("PK__departme__3213E83FBD7ECA4F");

                    b.ToTable("departments", (string)null);
                });

            modelBuilder.Entity("GlobantChallenge.Models.HiredEmployee", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("department_id");

                    b.Property<DateTime?>("HireDate")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("datetime2")
                        .HasColumnName("datetime");

                    b.Property<int?>("JobId")
                        .HasColumnType("int")
                        .HasColumnName("job_id");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__hired_em__3213E83F58807ED0");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("JobId");

                    b.ToTable("hired_employees", (string)null);
                });

            modelBuilder.Entity("GlobantChallenge.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Job1")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("job");

                    b.HasKey("Id")
                        .HasName("PK__jobs__3213E83F96F7BB39");

                    b.ToTable("jobs", (string)null);
                });

            modelBuilder.Entity("GlobantChallenge.Models.HiredEmployee", b =>
                {
                    b.HasOne("GlobantChallenge.Models.Department", "Department")
                        .WithMany("HiredEmployees")
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("FK__hired_emp__depar__286302EC");

                    b.HasOne("GlobantChallenge.Models.Job", "Job")
                        .WithMany("HiredEmployees")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK__hired_emp__job_i__29572725");

                    b.Navigation("Department");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("GlobantChallenge.Models.Department", b =>
                {
                    b.Navigation("HiredEmployees");
                });

            modelBuilder.Entity("GlobantChallenge.Models.Job", b =>
                {
                    b.Navigation("HiredEmployees");
                });
#pragma warning restore 612, 618
        }
    }
}
