﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace backend.Models
{
    public partial class ApiDBContext : DbContext
    {
        public ApiDBContext()
        {
        }

        public ApiDBContext(DbContextOptions<ApiDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Detailmachine> Detailmachines { get; set; }
        public virtual DbSet<Detailtechmachine> Detailtechmachines { get; set; }
        public virtual DbSet<Explaimmachine> Explaimmachines { get; set; }
        public virtual DbSet<Explaintype> Explaintypes { get; set; }
        public virtual DbSet<Imageblog> Imageblogs { get; set; }
        public virtual DbSet<Imagemachine> Imagemachines { get; set; }
        public virtual DbSet<Imagenews> Imagenews { get; set; }
        public virtual DbSet<Imageportfolio> Imageportfolios { get; set; }
        public virtual DbSet<Imagetype> Imagetypes { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<Manualmachine> Manualmachines { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Photoall> Photoalls { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Technically> Technicallies { get; set; }
        public virtual DbSet<Typemachine> Typemachines { get; set; }
        public virtual DbSet<Typenews> Typenews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Videomachine> Videomachines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=203.154.83.34;port=3465;database=packing;user=root;password=dewdevP@ssw0rd", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.25-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blog");

                entity.Property(e => e.BlogId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("blogID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.BlogSeo)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("blogSeo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("content")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("title")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.HasIndex(e => e.Position, "position_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CategoryId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("categoryID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoryName)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("categoryName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileImage)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileImage")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocalImage)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Position)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("position");

                entity.Property(e => e.Seo)
                    .HasColumnType("varchar(10000)")
                    .HasColumnName("seo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.Property(e => e.ContactId).HasColumnName("contactID");

                entity.Property(e => e.ContactDetail)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("contactDetail")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ContactMail)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("contactMail")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ContactName)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("contactName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ContactTel)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("contactTel")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ContactTitle)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("contactTitle")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("0: ยังไม่อ่าน\n1: อ่านแล้ว");
            });

            modelBuilder.Entity<Detailmachine>(entity =>
            {
                entity.ToTable("detailmachine");

                entity.HasIndex(e => e.MachineId, "dmachineID_idx");

                entity.Property(e => e.DetailMachineId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("detailMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasColumnName("detail")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Detailmachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("dmachineID");
            });

            modelBuilder.Entity<Detailtechmachine>(entity =>
            {
                entity.ToTable("detailtechmachine");

                entity.HasIndex(e => e.MachineId, "machineID_idx");

                entity.Property(e => e.DetailTechMachineId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("detailTechMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DetailTech)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasColumnName("detailTech")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TechnicallyId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("technicallyID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Detailtechmachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("machTech");
            });

            modelBuilder.Entity<Explaimmachine>(entity =>
            {
                entity.HasKey(e => e.ExplainMachineId)
                    .HasName("PRIMARY");

                entity.ToTable("explaimmachine");

                entity.HasIndex(e => e.MachineId, "xmachineID_idx");

                entity.Property(e => e.ExplainMachineId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("explainMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ExplainDetail)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasColumnName("explainDetail")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Explaimmachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("xmachineID");
            });

            modelBuilder.Entity<Explaintype>(entity =>
            {
                entity.ToTable("explaintype");

                entity.HasIndex(e => e.TypeId, "xtypeID_idx");

                entity.Property(e => e.ExplainTypeId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("explainTypeID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ExplainDetail)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasColumnName("explainDetail")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Explaintypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("xtypeID");
            });

            modelBuilder.Entity<Imageblog>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.BlogId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("imageblog");

                entity.HasIndex(e => e.BlogId, "blogID_idx");

                entity.Property(e => e.ImageId)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("ImageID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.BlogId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("blogID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Imageblogs)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("blogID");
            });

            modelBuilder.Entity<Imagemachine>(entity =>
            {
                entity.ToTable("imagemachine");

                entity.HasIndex(e => e.Local, "Local_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FileName, "fileName_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MachineId, "machineID_idx");

                entity.Property(e => e.ImageMachineId)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("ImageMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Imagemachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("machineID");
            });

            modelBuilder.Entity<Imagenews>(entity =>
            {
                entity.ToTable("imagenews");

                entity.HasIndex(e => e.Local, "Local_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FileName, "fileName_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.NewsId, "newsID");

                entity.Property(e => e.ImagenewsId)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("ImagenewsID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewsId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("newsID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.Imagenews)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("newsID");
            });

            modelBuilder.Entity<Imageportfolio>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PRIMARY");

                entity.ToTable("imageportfolio");

                entity.Property(e => e.ImageId)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("ImageID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PortfolioId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("portfolioID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Imagetype>(entity =>
            {
                entity.ToTable("imagetype");

                entity.HasIndex(e => e.TypeId, "ItypeID_idx");

                entity.HasIndex(e => e.Local, "Local_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FileName, "fileName_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ImagetypeId)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("ImagetypeID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Imagetypes)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItypeID");
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("machine");

                entity.HasIndex(e => e.TypeId, "typeID_idx");

                entity.Property(e => e.MachineId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditDiscountDate).HasColumnType("datetime");

                entity.Property(e => e.EditDiscountUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileImage)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileImage")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ItemsCode)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("itemsCode")
                    .HasComment("models")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocalImage)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineName)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("machineName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineSeo)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("machineSeo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Machinecol)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("machinecol")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Recommend).HasColumnName("recommend");

                entity.Property(e => e.Soldout)
                    .HasColumnName("soldout")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Machines)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("typeID");
            });

            modelBuilder.Entity<Manualmachine>(entity =>
            {
                entity.ToTable("manualmachine");

                entity.HasIndex(e => e.MachineId, "mmachineID_idx");

                entity.Property(e => e.ManualMachineId)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("manualMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Manual)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("manual")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Manualmachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("mmachineID");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.ToTable("news");

                entity.HasIndex(e => e.TypeNewsId, "typeNewsID_idx");

                entity.Property(e => e.NewsId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("newsID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("content")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileImage)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileImage")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocalImage)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewsSeo)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("newsSeo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("title")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeNewsId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeNewsID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.TypeNews)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.TypeNewsId)
                    .HasConstraintName("typeNewsID");
            });

            modelBuilder.Entity<Photoall>(entity =>
            {
                entity.HasKey(e => e.IdPhoto)
                    .HasName("PRIMARY");

                entity.ToTable("photoall");

                entity.Property(e => e.IdPhoto)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("idPhoto")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Local)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.ToTable("portfolio");

                entity.HasIndex(e => e.MachineId, "machineID_idx");

                entity.Property(e => e.PortfolioId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("portfolioID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileImage)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileImage")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocalImage)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Seo)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("seo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("title")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Portfolios)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("pmachineID");
            });

            modelBuilder.Entity<Technically>(entity =>
            {
                entity.ToTable("technically");

                entity.Property(e => e.TechnicallyId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("technicallyID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TechnicallyName)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("technicallyName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Typemachine>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PRIMARY");

                entity.ToTable("typemachine");

                entity.HasIndex(e => e.CategoryId, "category_idx");

                entity.Property(e => e.TypeId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeID")
                    .HasComment("T0000000\\nT วัน เดือน ไอเทม 3 หลัก\\nT2503001")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("categoryID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FileImage)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("fileImage")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocalImage)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeName)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("typeName")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeSeo)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("typeSeo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Typemachines)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("category");
            });

            modelBuilder.Entity<Typenews>(entity =>
            {
                entity.ToTable("typenews");

                entity.Property(e => e.TypeNewsId)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("typeNewsID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TypeNews1)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("typeNews")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.CreateDate, e.CreateUser })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("user");

                entity.HasIndex(e => e.Username, "Username_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Username)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Videomachine>(entity =>
            {
                entity.ToTable("videomachine");

                entity.HasIndex(e => e.MachineId, "machineID_idx");

                entity.Property(e => e.VideoMachineId)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("videoMachineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnType("varchar(300)")
                    .HasColumnName("link")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnType("varchar(7)")
                    .HasColumnName("machineID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Videomachines)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("vmachineID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
