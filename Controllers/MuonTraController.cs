using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;


public class MuonTraController : Controller
{
    private readonly ApplicationDbContext _context;

    public MuonTraController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var danhSachPhieuMuon = await _context.MuonTra
            .Include(mt => mt.MaNguoiDung)
            .OrderByDescending(mt => mt.MaMuonTra)
            .ToListAsync();
        return View(danhSachPhieuMuon);
    }

    public async Task<IActionResult> ChiTiet(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Mã phiếu mượn không hợp lệ.");
        }

        try
        {
            var danhSachChiTiet = await _context.ChiTietMuonTra
                .Where(ct => ct.MaMuonTra == id)
                .Include(ct => ct.MaTaiLieu) 
                .ToListAsync();

            if (danhSachChiTiet == null || danhSachChiTiet.Count == 0)
            {
                return NotFound("Không tìm thấy chi tiết phiếu mượn.");
            }
             ViewBag.MaMuonTra = id;
            return View("ChiTiet", danhSachChiTiet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi server: {ex.Message}");
        }
    }


    public IActionResult ThemPhieuMuon()
    {
      
        var lastMaMuonTra = _context.MuonTra
            .OrderByDescending(mt => mt.MaMuonTra)
            .Select(mt => mt.MaMuonTra)
            .FirstOrDefault();

     
        string newMaMuonTra = GenerateNextMaMuonTra(lastMaMuonTra);

        ViewBag.DanhSachNguoiDung = _context.NguoiDung.ToList(); 

        return View(new tblMuonTra
        {
            MaNguoiDung = "",
            MaMuonTra = newMaMuonTra,
            NgayMuon = DateTime.Now,
            NgayHenTra = DateTime.Now.AddDays(7),
            TinhTrang = "Đang mượn"
        });
    }

  
    private string GenerateNextMaMuonTra(string? lastMa)
    {
        if (string.IsNullOrEmpty(lastMa))
        {
            return "MT001";
        }

        string prefix = lastMa.Substring(0, 2);
        int number = int.Parse(lastMa.Substring(2));
        number++;
        return $"{prefix}{number:D3}";
    }


    [HttpPost]
    public async Task<IActionResult> ThemPhieuMuon([Bind("MaMuonTra,MaCTMuonTra,MaNguoiDung,NgayHenTra,NgayTraThucTe,TinhTrang")] tblMuonTra model)
    {
        if (string.IsNullOrEmpty(model.MaNguoiDung))
        {
            ModelState.AddModelError("MaNguoiDung", "Vui lòng chọn độc giả.");
        }

        if (model.NgayHenTra == default)
        {
            ModelState.AddModelError("NgayHenTra", "Vui lòng chọn ngày hẹn trả.");
        }

        model.NgayMuon = DateTime.Now;

        if (!ModelState.IsValid)
        {
            ViewBag.DanhSachNguoiDung = _context.NguoiDung.ToList();
            return View(model);
        }

        _context.MuonTra.Add(model);
        await _context.SaveChangesAsync();

      
        return RedirectToAction("ThemChiTietMuonTra", "ChiTietMuonTra", new { maMuonTra = model.MaMuonTra });
    }

    [HttpGet]
    public async Task<IActionResult> GetPhieuMuon(string id)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(id);
        if (phieuMuon == null)
        {
            return NotFound();
        }

        return Json(new
        {
            maMuonTra = phieuMuon.MaMuonTra,
            maNguoiDung = phieuMuon.MaNguoiDung,
            ngayMuon = phieuMuon.NgayMuon.ToString("yyyy-MM-dd"),
            ngayHenTra = phieuMuon.NgayHenTra.ToString("yyyy-MM-dd"),
            tinhTrang = phieuMuon.TinhTrang
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePhieuMuon(tblMuonTra model)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(model.MaMuonTra);
        if (phieuMuon == null)
        {
            return NotFound();
        }
        phieuMuon.MaMuonTra = model.MaMuonTra;
        phieuMuon.MaNguoiDung = model.MaNguoiDung;
        phieuMuon.NgayMuon = model.NgayMuon;
        phieuMuon.NgayHenTra = model.NgayHenTra;
        phieuMuon.TinhTrang = model.TinhTrang;

        _context.Update(phieuMuon);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeletePhieuMuon(string id)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(id);
        if (phieuMuon == null)
        {
            return NotFound();
        }

        _context.MuonTra.Remove(phieuMuon);
        await _context.SaveChangesAsync();
        return Ok();
    }








public IActionResult ThemChiTietMuonTra(string maMuonTra)
    {
        var lastMaMuonCTTra = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        string newMaCTMuonTra = GenerateNextMaCTMuonTra(lastMaMuonCTTra);

        ViewBag.MaMuonTra = maMuonTra;
        ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();

        return View(new tblChiTietMuonTra
        {
            MaChiTiet = newMaCTMuonTra,
            MaTaiLieu = "",
            SoluongMuon = 1,
            TinhTrangSachMuon = "Mới",
            MaMuonTra = maMuonTra
        });
    }

    [HttpGet]
    public IActionResult GenerateNewMaChiTiet()
    {
        var lastMaCT = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        string newMaCT = GenerateNextMaCTMuonTra(lastMaCT);
        return Json(newMaCT);
    }



    private string GenerateNextMaCTMuonTra(string? lastMaCT)
    {
        if (string.IsNullOrEmpty(lastMaCT))
        {
            return "CT001"; 
        }

        string prefix = "CT";
        int number;
        if (lastMaCT.StartsWith(prefix) && int.TryParse(lastMaCT.Substring(2), out number))
        {
            number++;
            return $"{prefix}{number:D3}";
        }

        return "CT001"; 
    }


    [HttpPost]
    public async Task<IActionResult> ThemChiTietMuonTra(List<tblChiTietMuonTra> ChiTietMuonTraList, string maMuonTra)
    {
        if (ChiTietMuonTraList == null || !ChiTietMuonTraList.Any())
        {
            ModelState.AddModelError("", "Dữ liệu không được gửi lên đúng cách.");
            ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();
            return View();
        }

        var lastMaMuonCTTra = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        List<string> errors = new List<string>();

        foreach (var item in ChiTietMuonTraList)
        {
            var taiLieu = await _context.TaiLieu.FindAsync(item.MaTaiLieu);
            if (taiLieu == null)
            {
                errors.Add($"Tài liệu có mã {item.MaTaiLieu} không tồn tại.");
                continue;
            }

            if (taiLieu.SoLuong < item.SoluongMuon)
            {
                errors.Add($"Tài liệu '{taiLieu.TenTaiLieu}' chỉ còn {taiLieu.SoLuong} cuốn, không đủ để mượn {item.SoluongMuon} cuốn.");
                continue;
            }

            item.MaChiTiet = GenerateNextMaCTMuonTra(lastMaMuonCTTra);
            lastMaMuonCTTra = item.MaChiTiet; 

            item.MaMuonTra = maMuonTra;
            taiLieu.SoLuong -= item.SoluongMuon;
            _context.TaiLieu.Update(taiLieu);
        }


        if (errors.Count > 0)
        {
            ViewBag.Errors = errors;
            ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();
            ViewBag.MaMuonTra = maMuonTra;
            return View(ChiTietMuonTraList);
        }


        _context.ChiTietMuonTra.AddRange(ChiTietMuonTraList);
        await _context.SaveChangesAsync();

        return RedirectToAction("ChiTiet", "MuonTra", new { id = maMuonTra });
    }
[HttpPost]
    public async Task<IActionResult> XoaChiTietMuonTra(string id)
    {
        var chiTiet = await _context.ChiTietMuonTra.FindAsync(id);
        if (chiTiet == null)
        {
            return NotFound();
        }

        _context.ChiTietMuonTra.Remove(chiTiet);
        await _context.SaveChangesAsync();

        return RedirectToAction("ChiTiet", "MuonTra", new { id = chiTiet.MaMuonTra });
    }
}
