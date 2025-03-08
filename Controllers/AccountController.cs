using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Linq;

namespace QuanLyThuVien.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(TaiKhoan model)
        {
        
            ModelState.Remove("MaTaiKhoan");
            ModelState.Remove("MaNguoiDung");

            if (ModelState.IsValid)
            {
               
                var taiKhoan = _context.TaiKhoan.FirstOrDefault(u =>
                    u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau);

                if (taiKhoan != null)
                {
              
                    var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == taiKhoan.MaNguoiDung);

                    if (nguoiDung != null)
                    {
                  
                        HttpContext.Session.SetString("sMaNguoiDung", nguoiDung.MaNguoiDung);
                        HttpContext.Session.SetString("sHoTen", nguoiDung.HoTen ?? string.Empty);
                        HttpContext.Session.SetString("sLoaiNguoiDung", nguoiDung.LoaiNguoiDung ?? string.Empty);

                     
                        return RedirectToAction("Index", "TaiLieu");
                    }
                    else
                    {
                       
                        ModelState.AddModelError("", "Người dùng liên kết với tài khoản này không tồn tại.");
                    }
                }
                else
                {
                
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

      
            return View(model);
        }
    }
}
