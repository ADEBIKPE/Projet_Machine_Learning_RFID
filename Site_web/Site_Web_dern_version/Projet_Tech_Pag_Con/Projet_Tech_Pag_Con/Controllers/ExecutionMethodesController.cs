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
    public class ExecutionMethodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExecutionMethodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExecutionMethodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExecutionMethode.ToListAsync());
        }

        // GET: ExecutionMethodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethode = await _context.ExecutionMethode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionMethode == null)
            {
                return NotFound();
            }

            return View(executionMethode);
        }

        // GET: ExecutionMethodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExecutionMethodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomMethode,Details,Performance,MatriceConfusion,Temps_Execution,SimulationId")] ExecutionMethode executionMethode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(executionMethode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(executionMethode);
        }

        // GET: ExecutionMethodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethode = await _context.ExecutionMethode.FindAsync(id);
            if (executionMethode == null)
            {
                return NotFound();
            }
            return View(executionMethode);
        }

        // POST: ExecutionMethodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomMethode,Details,Performance,MatriceConfusion,Temps_Execution,SimulationId")] ExecutionMethode executionMethode)
        {
            if (id != executionMethode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(executionMethode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutionMethodeExists(executionMethode.Id))
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
            return View(executionMethode);
        }

        // GET: ExecutionMethodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionMethode = await _context.ExecutionMethode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionMethode == null)
            {
                return NotFound();
            }

            return View(executionMethode);
        }

        // POST: ExecutionMethodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var executionMethode = await _context.ExecutionMethode.FindAsync(id);
            if (executionMethode != null)
            {
                _context.ExecutionMethode.Remove(executionMethode);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExecutionMethodeExists(int id)
        {
            return _context.ExecutionMethode.Any(e => e.Id == id);
        }
    }
}
