using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;
using Projet_Tech_Pag_Con.Models;

namespace Projet_Tech_Pag_Con.Controllers
{
    public class ExecutionMethodesAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExecutionMethodesAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExecutionMethodesAdmins
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExecutionMethodesAdmin.ToListAsync());
        }

        // GET: ExecutionMethodesAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethodesAdmin = await _context.ExecutionMethodesAdmin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionMethodesAdmin == null)
            {
                return NotFound();
            }

            return View(executionMethodesAdmin);
        }

        // GET: ExecutionMethodesAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExecutionMethodesAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomMethode,Details,Performance,MatriceConfusion,Temps_Execution,SimulationId,UserId,UserRoleId")] ExecutionMethodesAdmin executionMethodesAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(executionMethodesAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(executionMethodesAdmin);
        }

        // GET: ExecutionMethodesAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethodesAdmin = await _context.ExecutionMethodesAdmin.FindAsync(id);
            if (executionMethodesAdmin == null)
            {
                return NotFound();
            }
            return View(executionMethodesAdmin);
        }

        // POST: ExecutionMethodesAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomMethode,Details,Performance,MatriceConfusion,Temps_Execution,SimulationId,UserId,UserRoleId")] ExecutionMethodesAdmin executionMethodesAdmin)
        {
            if (id != executionMethodesAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(executionMethodesAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutionMethodesAdminExists(executionMethodesAdmin.Id))
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
            return View(executionMethodesAdmin);
        }

        // GET: ExecutionMethodesAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethodesAdmin = await _context.ExecutionMethodesAdmin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionMethodesAdmin == null)
            {
                return NotFound();
            }

            return View(executionMethodesAdmin);
        }

        // POST: ExecutionMethodesAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var executionMethodesAdmin = await _context.ExecutionMethodesAdmin.FindAsync(id);
            if (executionMethodesAdmin != null)
            {
                _context.ExecutionMethodesAdmin.Remove(executionMethodesAdmin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExecutionMethodesAdminExists(int id)
        {
            return _context.ExecutionMethodesAdmin.Any(e => e.Id == id);
        }
    }
}
