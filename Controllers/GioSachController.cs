using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Linq;
using QuanLyThuVien.ViewModels;
using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Controllers
{
    public class GioSachController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioSachController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? maNguoiDung = HttpContext.Session.GetString("sMaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem giỏ sách!";
                return RedirectToAction("Login", "Account");
            }

            var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index", "Home");
            }

            var gioSach = _context.GioSach.Where(g => g.MaNguoiDung == maNguoiDung).ToList();

            var viewModel = new GioSachViewModel
            {
                NguoiDung = nguoiDung,
                GioSachItems = gioSach
            };

            return View(viewModel);
        }

        public IActionResult ThemVaoGio(string maTaiLieu, int soLuong)
        {
            string? maNguoiDung = HttpContext.Session.GetString("sMaNguoiDung");

            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("Login", "Account");
            }

            var gioSachItem = _context.GioSach.FirstOrDefault(g => g.MaNguoiDung == maNguoiDung && g.MaTaiLieu == maTaiLieu);

            if (gioSachItem != null)
            {
                gioSachItem.SoLuong += soLuong;
            }
            else
            {
           
                var taiLieu = _context.TaiLieu.FirstOrDefault(t => t.MaTaiLieu == maTaiLieu);
                if (taiLieu == null)
                {
                    TempData["ErrorMessage"] = "Tài liệu không tồn tại!";
                    return RedirectToAction("Index");
                }

                var gioSach = new GioSach
                {
                    MaNguoiDung = maNguoiDung,
                    MaTaiLieu = maTaiLieu,
                    SoLuong = soLuong,
                    TenTaiLieu = taiLieu.TenTaiLieu,
                    TenTacGia = taiLieu.TenTacGia,  
                    img = taiLieu.img                
                };
                _context.GioSach.Add(gioSach);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "📚 Đã thêm vào giỏ sách!";
            return RedirectToAction("Index");
        }

        public IActionResult XoaKhoiGio(string id)
        {
            var gioSach = _context.GioSach.FirstOrDefault(g => g.MaTaiLieu == id);
            if (gioSach != null)
            {
                _context.GioSach.Remove(gioSach);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "📚 Đã xóa tài liệu khỏi giỏ!";
            }
            else
            {
                TempData["ErrorMessage"] = "❌ Không tìm thấy tài liệu trong giỏ!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult XacNhanMuonSach(List<string> selectedItems, DateTime ngayTraChung)
        {
            if (selectedItems == null || !selectedItems.Any())
            {
                TempData["ErrorMessage"] = "Bạn cần chọn ít nhất một tài liệu để mượn!";
                return RedirectToAction("Index");
            }

            string? maNguoiDung = HttpContext.Session.GetString("sMaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để mượn sách!";
                return RedirectToAction("Login", "Account");
            }

            var gioSachItems = _context.GioSach.Where(g => selectedItems.Contains(g.MaTaiLieu) && g.MaNguoiDung == maNguoiDung).ToList();
            if (!gioSachItems.Any())
            {
                TempData["ErrorMessage"] = "❌ Không tìm thấy tài liệu trong giỏ!";
                return RedirectToAction("Index");
            }

         
            var maMuonTra = "MT" + DateTime.Now.Ticks.ToString();

        
            var muonTra = new tblMuonTra
            {
                MaMuonTra = maMuonTra,
                MaNguoiDung = maNguoiDung,
                NgayMuon = DateTime.Now,
                NgayHenTra = ngayTraChung,
                TinhTrang = "Chờ xác nhận"
            };
            _context.MuonTra.Add(muonTra);
            _context.SaveChanges();

        
            var chiTietMuonTraList = new List<tblChiTietMuonTra>();
            foreach (var gioSachItem in gioSachItems)
            {
                var chiTietMuonTra = new tblChiTietMuonTra
                {
                    MaChiTiet = "CT" + Guid.NewGuid().ToString("N").Substring(0, 20), // Sử dụng Guid để tạo mã duy nhất
                    MaMuonTra = maMuonTra,
                    MaTaiLieu = gioSachItem.MaTaiLieu,
                    SoluongMuon = gioSachItem.SoLuong,
                    TinhTrangSachMuon = "Mới"
                };
                _context.ChiTietMuonTra.Add(chiTietMuonTra);
                chiTietMuonTraList.Add(chiTietMuonTra);
            }
            _context.SaveChanges();

        
            _context.GioSach.RemoveRange(gioSachItems);
            _context.SaveChanges();

       
            var viewModel = new PhieuMuonViewModel
            {
                GioSachItems = gioSachItems,
                NguoiDung = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == maNguoiDung),
                MuonTra = muonTra,
                ChiTietMuonTra = chiTietMuonTraList
            };

            return View("PhieuMuon", new List<PhieuMuonViewModel> { viewModel });
        }

     
        public IActionResult PhieuMuon()
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
                                MaTaiLieu = ct.MaTaiLieu,
                                TenTaiLieu = taiLieu?.TenTaiLieu,
                                TenTacGia = taiLieu?.TenTacGia,
                                SoLuong = ct.SoluongMuon
                            };
                        }).ToList(),
                    MuonTra = mt,
                    ChiTietMuonTra = _context.ChiTietMuonTra.Where(ct => ct.MaMuonTra == mt.MaMuonTra).ToList()
                }).ToList();

            return View(phieuMuons);
        }
    }
}