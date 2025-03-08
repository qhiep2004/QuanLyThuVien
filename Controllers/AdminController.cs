using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Linq;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult QuanLyPhieuMuon()
    {
        var phieuMuons = _context.MuonTra.Where(x => x.TinhTrang == "Chờ xác nhận").ToList();
        return View(phieuMuons);
    }

    public IActionResult XacNhanPhieuMuon(string id)
    {
        var phieuMuon = _context.MuonTra.FirstOrDefault(x => x.MaMuonTra == id);
        if (phieuMuon != null)
        {
            phieuMuon.TinhTrang = "Đang mượn";
            _context.SaveChanges();
        }
        return RedirectToAction("QuanLyPhieuMuon");
    }
}