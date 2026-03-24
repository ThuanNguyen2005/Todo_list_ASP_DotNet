using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.Models;

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

            var todoTask = await _context.TodoTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // GET: TodoTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,CreatedAt")] TodoTask todoTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoTask);
        }

        // GET: TodoTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTask.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }
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

            var todoTask = await _context.TodoTask
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
            var todoTask = await _context.TodoTask.FindAsync(id);
            if (todoTask != null)
            {
                _context.TodoTask.Remove(todoTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoTaskExists(int id)
        {
            return _context.TodoTask.Any(e => e.Id == id);
        }
    }
}
