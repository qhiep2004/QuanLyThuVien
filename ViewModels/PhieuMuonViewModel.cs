using QuanLyThuVien.Models;
using System.Collections.Generic;

namespace QuanLyThuVien.ViewModels
{
    public class PhieuMuonViewModel
    {
        public PhieuMuonViewModel()
        {
            GioSachItems = new List<GioSach>();
            ChiTietMuonTra = new List<tblChiTietMuonTra>();
             MuonTra = new List<tblMuonTra>(); 
        }
       public List<GioSach> GioSachItems { get; set; }
        public  NguoiDung? NguoiDung { get; set; }
        public required tblMuonTra MuonTra { get; set; }
        public required List<tblChiTietMuonTra> ChiTietMuonTra { get; set; }
    }
}