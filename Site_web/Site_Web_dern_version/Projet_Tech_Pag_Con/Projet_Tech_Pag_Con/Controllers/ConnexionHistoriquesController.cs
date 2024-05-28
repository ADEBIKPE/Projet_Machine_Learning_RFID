using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;

namespace Projet_Tech_Pag_Con.Controllers
{
    public class ConnexionHistoriquesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConnexionHistoriquesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConnexionHistoriques
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConnexionHistorique.ToListAsync());
        }

        // GET: ConnexionHistoriques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connexionHistorique = await _context.ConnexionHistorique
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connexionHistorique == null)
            {
                return NotFound();
            }

            return View(connexionHistorique);
        }

        // GET: ConnexionHistoriques/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConnexionHistoriques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,DateConnexion")] ConnexionHistorique connexionHistorique)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connexionHistorique);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connexionHistorique);
        }

        // GET: ConnexionHistoriques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connexionHistorique = await _context.ConnexionHistorique.FindAsync(id);
            if (connexionHistorique == null)
            {
                return NotFound();
            }
            return View(connexionHistorique);
        }

        // POST: ConnexionHistoriques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,DateConnexion")] ConnexionHistorique connexionHistorique)
        {
            if (id != connexionHistorique.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connexionHistorique);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnexionHistoriqueExists(connexionHistorique.Id))
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
            return View(connexionHistorique);
        }

        // GET: ConnexionHistoriques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connexionHistorique = await _context.ConnexionHistorique
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connexionHistorique == null)
            {
                return NotFound();
            }

            return View(connexionHistorique);
        }

        // POST: ConnexionHistoriques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var connexionHistorique = await _context.ConnexionHistorique.FindAsync(id);
            if (connexionHistorique != null)
            {
                _context.ConnexionHistorique.Remove(connexionHistorique);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnexionHistoriqueExists(int id)
        {
            return _context.ConnexionHistorique.Any(e => e.Id == id);
        }
    }
}
