using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblNguoiDung")] // Ánh xạ tới bảng tblNguoiDung
    public class NguoiDung
    {
        [Key]
        [Column("sMaNguoiDung")]
        public required string MaNguoiDung { get; set; }

        [Required]
        [Column("sHoTen")]
        public required string HoTen { get; set; }

        [Column("sSDT")]
        public required string SDT { get; set; }

        [Column("sMatKhau")]
        public required string MatKhau { get; set; }

        [Column("sEmail")]
        public required string Email { get; set; }

        [Required]
        [Column("sLoaiNguoiDung")]
        public required string LoaiNguoiDung { get; set; }
    }
}
