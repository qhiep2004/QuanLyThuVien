using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Ánh xạ đến bảng có sẵn trong cơ sở dữ liệu
    modelBuilder.Entity<TaiKhoan>().ToTable("tblTaiKhoan");
    modelBuilder.Entity<NguoiDung>().ToTable("tblNguoiDung");
   // Định nghĩa TaiKhoan là một keyless entity
    modelBuilder.Entity<TaiKhoan>().HasNoKey();
}

    }
}
