namespace QuanLyThuVien.ViewModels
{
    public class TaiKhoanNguoiDungViewModel
    {
        // Thông tin tài khoản
        public required string MaTaiKhoan { get; set; }
        public required string TenDangNhap { get; set; }
        public required string MatKhau { get; set; }

        // Thông tin người dùng
        public required string MaNguoiDung { get; set; }
        public required string HoTen { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? LoaiNguoiDung { get; set; }
    }
}