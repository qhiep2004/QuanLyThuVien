

using QuanLyThuVien.Models;

namespace QuanLyThuVien.ViewModels
{
    public class GioSachViewModel
    {
        public NguoiDung? NguoiDung { get; set; }
        public List<GioSach> GioSachItems { get; set; } = new List<GioSach>();
    }
}
