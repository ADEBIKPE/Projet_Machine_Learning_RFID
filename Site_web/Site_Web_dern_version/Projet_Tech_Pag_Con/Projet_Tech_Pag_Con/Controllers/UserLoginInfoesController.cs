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
    public class UserLoginInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserLoginInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserLoginInfoes
        public async Task<IActionResult> Index()
        {
            var users = await (from user in _context.Users
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.Id
                               select new UserViewModel
                               {
                                   Email = user.Email,
                                   UserId = user.Id,
                                   PhoneNumber = user.PhoneNumber,
                                   Role = role.Name
                               }).ToListAsync();

            return View(users);
        }

        // GET: UserLoginInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLoginInfo = await _context.UserLoginInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLoginInfo == null)
            {
                return NotFound();
            }

            return View(userLoginInfo);
        }

        // GET: UserLoginInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserLoginInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,UserId,PhoneNumber,Role")] UserLoginInfo userLoginInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLoginInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userLoginInfo);
        }

        // GET: UserLoginInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLoginInfo = await _context.UserLoginInfo.FindAsync(id);
            if (userLoginInfo == null)
            {
                return NotFound();
            }
            return View(userLoginInfo);
        }

        // POST: UserLoginInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,UserId,PhoneNumber,Role")] UserLoginInfo userLoginInfo)
        {
            if (id != userLoginInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLoginInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLoginInfoExists(userLoginInfo.Id))
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
            return View(userLoginInfo);
        }

        // GET: UserLoginInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLoginInfo = await _context.UserLoginInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLoginInfo == null)
            {
                return NotFound();
            }

            return View(userLoginInfo);
        }

        // POST: UserLoginInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userLoginInfo = await _context.UserLoginInfo.FindAsync(id);
            if (userLoginInfo != null)
            {
                _context.UserLoginInfo.Remove(userLoginInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLoginInfoExists(int id)
        {
            return _context.UserLoginInfo.Any(e => e.Id == id);
        }
    }
}
