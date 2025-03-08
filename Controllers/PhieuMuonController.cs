using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Linq;
using QuanLyThuVien.ViewModels;
using System;

namespace QuanLyThuVien.Controllers
{
    public class PhieuMuonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhieuMuonController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {
            string? maNguoiDung = HttpContext.Session.GetString("sMaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem phiếu mượn!";
                return RedirectToAction("Login", "Account");
            }

            var phieuMuons = _context.MuonTra
                .Where(mt => mt.MaNguoiDung == maNguoiDung)
                .ToList()
                .Select(mt => new PhieuMuonViewModel
                {
                    NguoiDung = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung),
                    GioSachItems = _context.ChiTietMuonTra
                        .Where(ct => ct.MaMuonTra == mt.MaMuonTra)
                        .ToList()
                        .Select(ct => 
                        {
                            var taiLieu = _context.TaiLieu.FirstOrDefault(t => t.MaTaiLieu == ct.MaTaiLieu);
                            return new GioSach
                            {
                                MaTaiLieu = ct.MaTaiLieu ?? string.Empty,
                                TenTaiLieu = taiLieu.TenTaiLieu,
                                TenTacGia = taiLieu.TenTacGia,
                                SoLuong = ct.SoluongMuon
                            };
                        }).ToList(),
                    MuonTra = mt,
                    ChiTietMuonTra = _context.ChiTietMuonTra.Where(ct => ct.MaMuonTra == mt.MaMuonTra).ToList()
                }).ToList();

            return View(phieuMuons);
        }

    
        public IActionResult ChiTiet(string maChiTiet)
        {
            var chiTiet = _context.ChiTietMuonTra.FirstOrDefault(ct => ct.MaChiTiet == maChiTiet);
            if (chiTiet == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin chi tiết!";
                return RedirectToAction("Index");
            }

            return View(chiTiet);
        }
         [HttpPost]
        public IActionResult HuyMuon(string maMuonTra)
        {
            var muonTra = _context.MuonTra.FirstOrDefault(mt => mt.MaMuonTra == maMuonTra);
            if (muonTra != null)
            {
                var chiTietMuonTraList = _context.ChiTietMuonTra.Where(ct => ct.MaMuonTra == maMuonTra).ToList();
                _context.ChiTietMuonTra.RemoveRange(chiTietMuonTraList);
                _context.MuonTra.Remove(muonTra);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "📚 Đã hủy phiếu mượn!";
            }
            else
            {
                TempData["ErrorMessage"] = "❌ Không tìm thấy phiếu mượn để hủy!";
            }
            return RedirectToAction("Index");
        }
    }
}