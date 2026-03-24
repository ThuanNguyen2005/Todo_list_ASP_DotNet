using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.Models;
using System.Security.Claims;
namespace Todo_list.Controllers
{
    public class TodoTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoTasksController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index(string searchString)
        {
            // Tạm thời lấy TẤT CẢ nhiệm vụ, không quan tâm ai là chủ sở hữu
            var tasks = await _context.TodoTasks.ToListAsync();
            return View(tasks);
        }

        // GET: TodoTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // 1. Hàm GET: Dùng để HIỂN THỊ trang nhập liệu khi bạn bấm "Create New"
        // ĐẢM BẢO KHÔNG CÓ THẺ [HttpPost] Ở ĐÂY
        public IActionResult Create()
        {
            return View();
        }

        // 2. Hàm POST: Dùng để LƯU dữ liệu khi bạn bấm nút "Create" trong Form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,CreatedAt")] TodoTask todoTask)
        {
            try
            {
                // 1. Gán ID người dùng
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                todoTask.UserId = userId;

                // 2. ÉP BUỘC LƯU - Bỏ qua IsValid để test
                _context.Add(todoTask);
                await _context.SaveChangesAsync();

                // 3. Nếu thành công, nó PHẢI nhảy về Index
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi (ví dụ: lỗi database, thiếu cột...), nó sẽ hiện lỗi ra màn hình cho bạn thấy
                return Content("LỖI RỒI: " + ex.Message + (ex.InnerException != null ? " - Chi tiết: " + ex.InnerException.Message : ""));
            }
        } 

        // GET: TodoTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todoTask = await _context.TodoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null) return NotFound(); // Không tìm thấy hoặc không phải của mình

            return View(todoTask);
        }

        // POST: TodoTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,CreatedAt")] TodoTask todoTask)
        {
            if (id != todoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoTaskExists(todoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoTask);
        }

        // GET: TodoTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // POST: TodoTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoTask = await _context.TodoTasks.FindAsync(id);
            if (todoTask != null)
            {
                _context.TodoTasks.Remove(todoTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoTaskExists(int id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
