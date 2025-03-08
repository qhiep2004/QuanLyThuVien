using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblGioSach")] // Tên bảng trong database
    public class GioSach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("sMaNguoiDung")]
        public string MaNguoiDung { get; set; } = string.Empty; // Liên kết với tài khoản

        [Required]
        [Column("sMaTaiLieu")]
        public string MaTaiLieu { get; set; } = string.Empty; 

        [Required]
        [Column("iSoLuong")]
        
        public int SoLuong { get; set; }
         [Required]
        [Column("sTenTaiLieu")]
        public  string TenTaiLieu { get; set; }= string.Empty;

          [Column("sTenTacGia")]
        public  string TenTacGia { get; set; }= string.Empty;

         [Required]
        [Column("img")]
        public string img { get; set; } = string.Empty; 
    }
}
