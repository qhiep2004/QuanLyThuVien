using Microsoft.Data.SqlClient; // Sử dụng Microsoft.Data.SqlClient để hỗ trợ Unicode
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuVien.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

   
        public IActionResult Index()
        {
            var users = _context.NguoiDung.ToList();
            return View(users);
        }

        // GET: Trang thêm người dùng
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string newId = InsertNguoiDungUsingProcedure(model);
                    System.Diagnostics.Debug.WriteLine("Mã người dùng mới: " + newId);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi khi thêm người dùng: " + ex.Message);
                    ModelState.AddModelError("", "Lỗi khi thêm người dùng: " + ex.Message);
                }
            }
            return View(model);
        }

      
        private string InsertNguoiDungUsingProcedure(NguoiDung model)
        {
            string newMaNguoiDung = string.Empty;

         
            model.HoTen = model.HoTen.Trim().Normalize(System.Text.NormalizationForm.FormC);
            if (!string.IsNullOrEmpty(model.SDT))
                model.SDT = model.SDT.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.MatKhau = model.MatKhau.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.Email = model.Email.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.LoaiNguoiDung = model.LoaiNguoiDung.Trim().Normalize(System.Text.NormalizationForm.FormC);

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "usp_InsertNguoiDung";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@HoTen", System.Data.SqlDbType.NVarChar, 100) { Value = model.HoTen });
                    command.Parameters.Add(new SqlParameter("@SDT", System.Data.SqlDbType.NVarChar, 50)
                    { Value = string.IsNullOrEmpty(model.SDT) ? DBNull.Value : (object)model.SDT });
                    command.Parameters.Add(new SqlParameter("@MatKhau", System.Data.SqlDbType.NVarChar, 100) { Value = model.MatKhau });
                    command.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100) { Value = model.Email });
                    command.Parameters.Add(new SqlParameter("@LoaiNguoiDung", System.Data.SqlDbType.NVarChar, 50) { Value = model.LoaiNguoiDung });

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        newMaNguoiDung = result.ToString();
                    }
                }
            }
            return newMaNguoiDung;
        }

   
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, NguoiDung model)
        {
           
            if (ModelState.IsValid)
            {
                var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
                if (user == null)
                    return NotFound();

         
                user.HoTen = model.HoTen.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.SDT = string.IsNullOrEmpty(model.SDT) ? null : model.SDT.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.MatKhau = model.MatKhau.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.Email = model.Email.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.LoaiNguoiDung = model.LoaiNguoiDung.Trim().Normalize(System.Text.NormalizationForm.FormC);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


     
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

  
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            _context.NguoiDung.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
