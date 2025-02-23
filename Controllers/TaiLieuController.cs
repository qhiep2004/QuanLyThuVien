using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data; // Đường dẫn đến DbContext
using QuanLyThuVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyThuVien.Controllers
{
    public class TaiLieuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiLieuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách thể loại và danh sách tài liệu
public IActionResult Index(string? theLoai = null)
{
    // Lấy danh sách thể loại, loại bỏ null và chuỗi rỗng
    var theLoaiList = _context.TaiLieu
        .Select(t => t.TheLoai ?? string.Empty) // Thay thế null bằng chuỗi trống
        .Where(t => !string.IsNullOrEmpty(t))  // Loại bỏ chuỗi rỗng nếu cần
        .Distinct()
        .ToList();

    // Lấy danh sách tài liệu theo thể loại
    var taiLieuList = string.IsNullOrEmpty(theLoai)
        ? _context.TaiLieu.ToList()
        : _context.TaiLieu
            .Where(t => t.TheLoai != null && t.TheLoai.Trim().ToLower() == theLoai.Trim().ToLower())
            .ToList();

    // Truyền dữ liệu vào View
    ViewBag.TheLoaiList = theLoaiList;
    ViewBag.SelectedTheLoai = theLoai;
    return View(taiLieuList);
}


    }
}
