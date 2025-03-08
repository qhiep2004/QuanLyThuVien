using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblTaiKhoan")] // Ánh xạ tới bảng tblTaiKhoan
    public class TaiKhoan
    {
        [Key]
        [Column("sMaTaiKhoan")] // Cột sMaTaiKhoan là khóa chính
        public required string MaTaiKhoan { get; set; }

        [Column("sMaNguoiDung")]
        public required string MaNguoiDung { get; set; }

        [Required]
        [Column("sTenDangNhap")]
        public required string TenDangNhap { get; set; }

        [Required]
        [Column("sMatKhau")]
        public required string MatKhau { get; set; }
        [ForeignKey("MaNguoiDung")]
        public  NguoiDung? NguoiDung { get; set; }
    }
}
