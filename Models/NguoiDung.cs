using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblNguoiDung")] 
    public class NguoiDung
    {
        [Key]
        [Column("sMaNguoiDung")]
        public string MaNguoiDung { get; set; } = string.Empty; 

        [Required]
        [Column("sHoTen")]
       public string HoTen { get; set; } = string.Empty; 

        [Column("sSDT")]
        public string? SDT { get; set; } 

        [Column("sMatKhau")]
       public string MatKhau { get; set; } = string.Empty; 

        [Column("sEmail")]
        public string? Email { get; set; }

        [Required]
        [Column("sLoaiNguoiDung")]
        public string? LoaiNguoiDung { get; set; } 
    }
}
