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
    public class SimulationAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SimulationAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SimulationAdmins
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SimulationAdmin.Include(s => s.Utilisateur);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SimulationAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simulationAdmin = await _context.SimulationAdmin
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (simulationAdmin == null)
            {
                return NotFound();
            }

            return View(simulationAdmin);
        }

        // GET: SimulationAdmins/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SimulationAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateSimulation,UtilisateurId")] SimulationAdmin simulationAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(simulationAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Users, "Id", "Id", simulationAdmin.UtilisateurId);
            return View(simulationAdmin);
        }

        // GET: SimulationAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simulationAdmin = await _context.SimulationAdmin.FindAsync(id);
            if (simulationAdmin == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Users, "Id", "Id", simulationAdmin.UtilisateurId);
            return View(simulationAdmin);
        }

        // POST: SimulationAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateSimulation,UtilisateurId")] SimulationAdmin simulationAdmin)
        {
            if (id != simulationAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(simulationAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SimulationAdminExists(simulationAdmin.Id))
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
            ViewData["UtilisateurId"] = new SelectList(_context.Users, "Id", "Id", simulationAdmin.UtilisateurId);
            return View(simulationAdmin);
        }

        // GET: SimulationAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simulationAdmin = await _context.SimulationAdmin
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (simulationAdmin == null)
            {
                return NotFound();
            }

            return View(simulationAdmin);
        }

        // POST: SimulationAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var simulationAdmin = await _context.SimulationAdmin.FindAsync(id);
            if (simulationAdmin != null)
            {
                _context.SimulationAdmin.Remove(simulationAdmin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SimulationAdminExists(int id)
        {
            return _context.SimulationAdmin.Any(e => e.Id == id);
        }
    }
}
