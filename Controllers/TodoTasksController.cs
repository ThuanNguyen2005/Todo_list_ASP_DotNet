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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasks = from t in _context.TodoTasks
                    where t.UserId == userId
                    select t;

        // Nếu người dùng có nhập từ khóa, lọc theo tiêu đề
        if (!String.IsNullOrEmpty(searchString))
        {
            tasks = tasks.Where(s => s.Title!.Contains(searchString));
        }

        return View(await tasks.ToListAsync());
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
            // 1. Lấy ID người dùng hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Gán vào Task
            todoTask.UserId = userId;

            // 3. XÓA UserId KHỎI DANH SÁCH KIỂM TRA LỖI (Quan trọng nhất)
            ModelState.Remove("UserId");

            if (ModelState.IsValid) // Bây giờ Valid mới có thể bằng True
            {
                _context.Add(todoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu vẫn lỗi, nó sẽ quay lại trang nhập liệu để bạn sửa
            return View(todoTask);
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
