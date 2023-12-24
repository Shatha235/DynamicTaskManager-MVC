using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class MytasksController : Controller
    {
        private readonly ModelContext _context;

        public MytasksController(ModelContext context)
        {
            _context = context;
        }

        // GET: Mytasks
        public async Task<IActionResult> Index()
        {
              return _context.Mytasks != null ? 
                          View(await _context.Mytasks.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Mytasks'  is null.");
        }

        // GET: Mytasks/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Mytasks == null)
            {
                return NotFound();
            }

            var mytask = await _context.Mytasks
                .FirstOrDefaultAsync(m => m.Taskid == id);
            if (mytask == null)
            {
                return NotFound();
            }

            return View(mytask);
        }

        // GET: Mytasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mytasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Taskid,Taskname,Taskdescription,Duedate,Priority,Status,Createddate")] Mytask mytask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mytask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mytask);
        }

        // GET: Mytasks/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Mytasks == null)
            {
                return NotFound();
            }

            var mytask = await _context.Mytasks.FindAsync(id);
            if (mytask == null)
            {
                return NotFound();
            }
            return View(mytask);
        }

        // POST: Mytasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Taskid,Taskname,Taskdescription,Duedate,Priority,Status,Createddate")] Mytask mytask)
        {
            if (id != mytask.Taskid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mytask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MytaskExists(mytask.Taskid))
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
            return View(mytask);
        }

        // POST: Mytasks/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Mytasks == null)
            {
                return Problem("Entity set 'ModelContext.Mytasks'  is null.");
            }
            var mytask = await _context.Mytasks.FindAsync(id);
            if (mytask != null)
            {
                _context.Mytasks.Remove(mytask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult EditStatus(int id, string status)
        {
            try
            {
                decimal decimalId = Convert.ToDecimal(id);
                Mytask task = _context.Mytasks.Find(decimalId);
                if (task == null)
                {
                    return Json(new { success = false, message = "Task not found." });
                }

                task.Status = status;
                _context.Entry(task).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new { success = true, message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
                return Json(new { success = false, message = "Error updating status: " + ex.Message });
            }
        }


        private bool MytaskExists(decimal id)
        {
          return (_context.Mytasks?.Any(e => e.Taskid == id)).GetValueOrDefault();
        }
    }
}
