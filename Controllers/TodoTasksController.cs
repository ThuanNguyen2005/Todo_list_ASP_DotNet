using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Todo_list.Data;
using Todo_list.Models;

namespace Todo_list.Controllers
{
    [Authorize]
    public class TodoTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasks = _context.TodoTasks
                                .Where(t => t.UserId == userId)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tasks = tasks.Where(t => t.Title != null &&
                    EF.Functions.Like(t.Title, $"%{searchTerm}%"));
            }

            if (!string.IsNullOrEmpty(category))
            {
                tasks = tasks.Where(t => t.danhMuc != null &&
                                         t.danhMuc == category);
            }

            var result = await tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoTask task)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                task.CreatedAt = DateTime.Now;
                task.UserId = userId;

                _context.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoTask task)
        {
            if (id != task.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingTask = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingTask == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.danhMuc = task.danhMuc;
                existingTask.trangThai = task.trangThai;

                // ✅ SỬA QUAN TRỌNG
                existingTask.Deadline = task.Deadline;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                _context.TodoTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ToggleComplete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            task.trangThai = !task.trangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}