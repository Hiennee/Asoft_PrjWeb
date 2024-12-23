using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestPrj3.Services;

namespace TestPrj3.Models;

public partial class Asoft2Context : DbContext
{
    public Asoft2Context()
    {
        // String trong c# thi sd 2 slash, con console thi 1 slash la dc, dcm
    }

    public Asoft2Context(DbContextOptions<Asoft2Context> options)
        : base(options)
    {
    }
    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PrimaryKeyConfig> PrimaryKeyConfigs { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Tasks> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Asoft_2;User Id=sa;Password=1;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD0E464D9F");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId)
                .HasMaxLength(10)
                .HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1C548E739");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(10)
                .HasColumnName("DepartmentID");
            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.PositionId)
                .HasMaxLength(10)
                .HasColumnName("PositionID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Employee__Depart__4F7CD00D");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Employee__Positi__5070F446");
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("PK__Issue__6C86162452BD78CA");

            entity.ToTable("Issue");

            entity.Property(e => e.IssueId)
                .HasMaxLength(10)
                .HasColumnName("IssueID");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.IssueName).HasMaxLength(255);
            entity.Property(e => e.IssueType).HasMaxLength(50);
            entity.Property(e => e.ProjectId)
                .HasMaxLength(10)
                .HasColumnName("ProjectID");
            entity.Property(e => e.ReportedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaskId)
                .HasMaxLength(10)
                .HasColumnName("TaskID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Issues)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Issue__EmployeeI__5BE2A6F2");

            entity.HasOne(d => d.Project).WithMany(p => p.Issues)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Issue__ProjectID__5AEE82B9");

            entity.HasOne(d => d.Task).WithMany(p => p.Issues)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Issue__TaskID__59FA5E80");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A5959C896E4");

            entity.ToTable("Position");

            entity.Property(e => e.PositionId)
                .HasMaxLength(10)
                .HasColumnName("PositionID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PositionName).HasMaxLength(50);
        });

        modelBuilder.Entity<PrimaryKeyConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrimaryK__3214EC27A3D89F72");

            entity.ToTable("PrimaryKeyConfig");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Format).HasMaxLength(10);
            entity.Property(e => e.Symbol1).HasMaxLength(10);
            entity.Property(e => e.Symbol2).HasMaxLength(10);
            entity.Property(e => e.Symbol3).HasMaxLength(10);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Project__761ABED0235F3A53");

            entity.ToTable("Project");

            entity.Property(e => e.ProjectId)
                .HasMaxLength(10)
                .HasColumnName("ProjectID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectName).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Projects)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Project__Employe__534D60F1");
        });

        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949D18D2F626D");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId)
                .HasMaxLength(10)
                .HasColumnName("TaskID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(10)
                .HasColumnName("ProjectID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaskName).HasMaxLength(255);
            entity.Property(e => e.TaskType).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Task__EmployeeID__571DF1D5");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Task__ProjectID__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
