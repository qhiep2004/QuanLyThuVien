using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers;

public class HomeController : Controller
{
     private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger ,ApplicationDbContext context)
    {
        _logger = logger;
          _context = context;
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


        public IActionResult Index()
        {
         
            var taiLieuList = _context.TaiLieu.ToList();

         
            var theLoaiList = _context.TaiLieu
                .Select(tl => tl.TheLoai)
                .Distinct()
                .ToList();

       
            var tongSoTaiLieu = taiLieuList.Count;

    
            var taiLieuSanSangChoMuon = taiLieuList.Count(tl => tl.TinhTrang == "Có sẵn");

         
            ViewBag.TaiLieuList = taiLieuList;
            ViewBag.TheLoaiList = theLoaiList;
            ViewBag.TongSoTaiLieu = tongSoTaiLieu;
            ViewBag.TaiLieuSanSangChoMuon = taiLieuSanSangChoMuon;

          
            return View();
        }
}
