using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyThuVien.Data; 
using QuanLyThuVien.Models;
using System.Data;
using System.Linq;

namespace QuanLyThuVien.Controllers
{
    public class AccountController : Controller
    {
// private readonly string _connectionString;

// public AccountController(IConfiguration configuration)
// {
//     _connectionString = configuration.GetConnectionString("DefaultConnection") 
//                         ?? throw new InvalidOperationException("Connection string is not configured.");
// }

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
    ModelState.Remove("MaTaiKhoan"); // Bỏ qua kiểm tra MaTaiKhoan
    if (ModelState.IsValid)
    {
        // Kiểm tra thông tin người dùng dựa trên tên đăng nhập và mật khẩu
        var user = _context.TaiKhoan.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau);
        if (user != null)
        {
            // Chuyển hướng về Home nếu đăng nhập thành công
            return RedirectToAction("Index", "Home");
        }

        // Nếu đăng nhập thất bại, thêm lỗi vào ModelState
        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
    }

    // Trả về View với model và lỗi đã thêm
    return View(model);
}
// [HttpPost]
//         public IActionResult Login(TaiKhoan model)
//         {
//             ModelState.Remove("MaTaiKhoan");
//             if (ModelState.IsValid)
//             {
//                 string result;

//                 // Kết nối tới SQL Server và gọi thủ tục
//                 using (var connection = new SqlConnection(_connectionString))
//                 {
//                     using (var command = new SqlCommand("sp_Login", connection))
//                     {
//                         command.CommandType = CommandType.StoredProcedure;
//                         command.Parameters.AddWithValue("@TenDangNhap", model.TenDangNhap);
//                         command.Parameters.AddWithValue("@MatKhau", model.MatKhau);

//                         connection.Open();
//                       result = command.ExecuteScalar()?.ToString() ?? string.Empty;
//  // Lấy kết quả trả về (Success/Fail)
//                     }
//                 }

//                 if (result == "Success")
//                 {
//                     // Đăng nhập thành công
//                     return RedirectToAction("Index", "Home");
//                 }

//                 // Đăng nhập thất bại
//                 ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
//             }

//             return View(model);
//         }
    }
}
