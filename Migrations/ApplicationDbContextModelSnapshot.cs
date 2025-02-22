﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuanLyThuVien.Data;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuanLyThuVien.Models.NguoiDung", b =>
                {
                    b.Property<string>("MaNguoiDung")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("sMaNguoiDung");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sEmail");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sHoTen");

                    b.Property<string>("LoaiNguoiDung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sLoaiNguoiDung");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sMatKhau");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sSDT");

                    b.HasKey("MaNguoiDung");

                    b.ToTable("tblNguoiDung", (string)null);
                });

            modelBuilder.Entity("QuanLyThuVien.Models.TaiKhoan", b =>
                {
                    b.Property<string>("MaTaiKhoan")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("sMaTaiKhoan");

                    b.Property<string>("MaNguoiDung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sMaNguoiDung");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sMatKhau");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sTenDangNhap");

                    b.HasKey("MaTaiKhoan");

                    b.ToTable("tblTaiKhoan", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
