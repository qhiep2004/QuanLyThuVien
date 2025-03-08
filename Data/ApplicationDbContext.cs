using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<TaiLieu> TaiLieu { get; set; }
        public DbSet<GioSach> GioSach { get; set; }
        public DbSet<tblMuonTra> MuonTra { get; set; }
        public DbSet<tblChiTietMuonTra> ChiTietMuonTra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

       
            modelBuilder.Entity<TaiKhoan>().ToTable("tblTaiKhoan");
            modelBuilder.Entity<NguoiDung>().ToTable("tblNguoiDung");
            modelBuilder.Entity<TaiLieu>().ToTable("tblTaiLieu");
            modelBuilder.Entity<GioSach>().ToTable("tblGioSach");
            modelBuilder.Entity<tblMuonTra>().ToTable("tblMuonTra");
            modelBuilder.Entity<tblChiTietMuonTra>().ToTable("tblChiTietMuonTra");

             modelBuilder.Entity<TaiKhoan>()
                .HasKey(t => t.MaTaiKhoan);

            modelBuilder.Entity<NguoiDung>()
                .HasKey(n => n.MaNguoiDung);

            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.NguoiDung)
                .WithMany()
                .HasForeignKey(t => t.MaNguoiDung);
                    
        }
    }
}