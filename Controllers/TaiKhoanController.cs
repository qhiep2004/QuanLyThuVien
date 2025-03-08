using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.ViewModels;
using System.Linq;

namespace QuanLyThuVien.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiKhoanController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var taiKhoanNguoiDung = _context.TaiKhoan
                .Join(_context.NguoiDung,
                      tk => tk.MaNguoiDung,
                      nd => nd.MaNguoiDung,
                      (tk, nd) => new TaiKhoanNguoiDungViewModel
                      {
                          MaTaiKhoan = tk.MaTaiKhoan,
                          TenDangNhap = tk.TenDangNhap,
                          MatKhau = tk.MatKhau,
                          MaNguoiDung = nd.MaNguoiDung,
                          HoTen = nd.HoTen,
                          SDT = nd.SDT,
                          Email = nd.Email,
                          LoaiNguoiDung = nd.LoaiNguoiDung
                      })
                .ToList();

            return View(taiKhoanNguoiDung);
        }

     
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult Create(TaiKhoanNguoiDungViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                
                var nguoiDung = new NguoiDung
                {
                    MaNguoiDung = viewModel.MaNguoiDung,
                    HoTen = viewModel.HoTen,
                    SDT = viewModel.SDT,
                    Email = viewModel.Email,
                    LoaiNguoiDung = viewModel.LoaiNguoiDung
                };

             
                _context.NguoiDung.Add(nguoiDung);

             
                var taiKhoan = new TaiKhoan
                {
                    MaTaiKhoan = viewModel.MaTaiKhoan,
                    TenDangNhap = viewModel.TenDangNhap,
                    MatKhau = viewModel.MatKhau,
                    MaNguoiDung = viewModel.MaNguoiDung,
                    NguoiDung = nguoiDung
                };

               
                _context.TaiKhoan.Add(taiKhoan);

               
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

      
        public IActionResult Edit(string id)
        {
            var taiKhoan = _context.TaiKhoan
                .Join(_context.NguoiDung,
                      tk => tk.MaNguoiDung,
                      nd => nd.MaNguoiDung,
                      (tk, nd) => new TaiKhoanNguoiDungViewModel
                      {
                          MaTaiKhoan = tk.MaTaiKhoan,
                          TenDangNhap = tk.TenDangNhap,
                          MatKhau = tk.MatKhau,
                          MaNguoiDung = nd.MaNguoiDung,
                          HoTen = nd.HoTen,
                          SDT = nd.SDT,
                          Email = nd.Email,
                          LoaiNguoiDung = nd.LoaiNguoiDung
                      })
                .FirstOrDefault(tk => tk.MaTaiKhoan == id);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

      
        [HttpPost]
        public IActionResult Edit(TaiKhoanNguoiDungViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == viewModel.MaNguoiDung);
                if (nguoiDung != null)
                {
                    nguoiDung.HoTen = viewModel.HoTen;
                    nguoiDung.SDT = viewModel.SDT;
                    nguoiDung.Email = viewModel.Email;
                    nguoiDung.LoaiNguoiDung = viewModel.LoaiNguoiDung;
                }

                var taiKhoan = _context.TaiKhoan.FirstOrDefault(tk => tk.MaTaiKhoan == viewModel.MaTaiKhoan);
                if (taiKhoan != null)
                {
                    taiKhoan.TenDangNhap = viewModel.TenDangNhap;
                    taiKhoan.MatKhau = viewModel.MatKhau;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(string id)
        {
            var taiKhoan = _context.TaiKhoan.FirstOrDefault(tk => tk.MaTaiKhoan == id);
            if (taiKhoan != null)
            {
                var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == taiKhoan.MaNguoiDung);
                if (nguoiDung != null)
                {
                    _context.NguoiDung.Remove(nguoiDung);
                }

                _context.TaiKhoan.Remove(taiKhoan);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}