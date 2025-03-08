using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class TaiLieuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

      public TaiLieuController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
      
        public IActionResult Index(string? theLoai = null)
        {
           
            var theLoaiList = _context.TaiLieu
                .Select(t => t.TheLoai ?? string.Empty) 
                .Where(t => !string.IsNullOrEmpty(t)) 
                .Distinct()
                .ToList();

       
            var taiLieuList = string.IsNullOrEmpty(theLoai)
                ? _context.TaiLieu.ToList()
                : _context.TaiLieu
                    .Where(t => t.TheLoai != null && t.TheLoai.Trim().ToLower() == theLoai.Trim().ToLower())
                    .ToList();

         
            ViewBag.TheLoaiList = theLoaiList;
            ViewBag.SelectedTheLoai = theLoai;
            return View(taiLieuList);
        }


public IActionResult MuonSach(string maTaiLieu, int soLuong)
{
    string? maNguoiDung = HttpContext.Session.GetString("sMaNguoiDung");
    if (string.IsNullOrEmpty(maNguoiDung))
    {
        TempData["ErrorMessage"] = "Bạn cần đăng nhập trước khi mượn sách!";
        return RedirectToAction("Login", "Account");
    }

    var taiLieu = _context.TaiLieu.FirstOrDefault(t => t.MaTaiLieu == maTaiLieu);
    if (taiLieu == null || taiLieu.SoLuong < soLuong)
    {
        TempData["ErrorMessage"] = "Sách không đủ!";
        return RedirectToAction("Index");
    }

    var gioSachItem = _context.GioSach
        .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung && g.MaTaiLieu == maTaiLieu);

    if (gioSachItem != null)
    {
        gioSachItem.SoLuong += soLuong;
    }
    else
    {
        _context.GioSach.Add(new GioSach
        {
            MaNguoiDung = maNguoiDung,
            MaTaiLieu = maTaiLieu,
            SoLuong = soLuong,
             TenTaiLieu = taiLieu.TenTaiLieu, 
            TenTacGia = taiLieu.TenTacGia,  
            img = taiLieu.img 
        });
    }

    _context.SaveChanges();
    TempData["SuccessMessage"] = "Thêm vào giỏ sách thành công!";
    return RedirectToAction("Index", "GioSach");
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


        public IActionResult XacNhanMuon()
        {
            var gioSach = HttpContext.Session.GetObjectFromJson<List<TaiLieu>>("GioSach") ?? new List<TaiLieu>();

            if (!gioSach.Any())
            {
                TempData["ErrorMessage"] = "Giỏ sách của bạn đang trống!";
                return RedirectToAction("GioSach");
            }

            foreach (var item in gioSach)
            {
                var taiLieu = _context.TaiLieu.FirstOrDefault(t => t.MaTaiLieu == item.MaTaiLieu);
                if (taiLieu != null)
                {
                    taiLieu.SoLuong -= item.SoLuong;
                }
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("GioSach");

            TempData["SuccessMessage"] = "Bạn đã mượn sách thành công!";
            return RedirectToAction("Index");
        }

         public IActionResult ChiTiet(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var taiLieu = _context.TaiLieu.FirstOrDefault(t => t.MaTaiLieu == id);
        if (taiLieu == null)
        {
            return NotFound();
        }

        return View(taiLieu);
    }

}
}