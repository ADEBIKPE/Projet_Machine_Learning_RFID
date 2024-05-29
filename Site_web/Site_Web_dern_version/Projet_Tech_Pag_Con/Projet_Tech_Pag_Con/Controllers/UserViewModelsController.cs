using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;
using Projet_Tech_Pag_Con.Models;

namespace Projet_Tech_Pag_Con.Controllers
{
    public class UserViewModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserViewModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserViewModels
        public async Task<IActionResult> Index()
        {
            // Vérifier si les détails des utilisateurs existent déjà dans UserViewModel
            var existingUsers = await (from userViewModel in _context.UserViewModel
                                       select userViewModel.Email).ToListAsync();

            // Récupérer les détails des utilisateurs à partir des tables Users, UserRoles et Roles
            var userDetails = await (from user in _context.Users
                                     join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                     join role in _context.Roles on userRole.RoleId equals role.Id
                                     where !existingUsers.Contains(user.Email)  // Filtrer les utilisateurs déjà enregistrés
                                     select new UserViewModel
                                     {
                                         Id = user.Id,
                                         Email = user.Email,
                                         UserId = user.Id,
                                         PhoneNumber = "0258784542"/* user.PhoneNumber*/,
                                         Role = role.Name
                                     }).ToListAsync();

            // Ajouter les nouveaux utilisateurs à la table UserViewModel
            _context.UserViewModel.AddRange(userDetails);
            await _context.SaveChangesAsync();

            // Récupérer tous les utilisateurs de la table UserViewModel
            var users = await _context.UserViewModel.ToListAsync();

            // Retourner la vue avec les détails des utilisateurs
            return View(users);
        }

        // GET: UserViewModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // GET: UserViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Vous n'avez pas besoin de définir manuellement UserId, laissez la base de données le générer automatiquement
                _context.Add(userViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }


        // GET: UserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel.FindAsync(id);
            if (userViewModel == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        // POST: UserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: UserViewModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,UserId,PhoneNumber,Role")] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = await _context.Users.FindAsync(userViewModel.UserId);
                        if (user == null)
                        {
                            return NotFound();
                        }

                        user.Email = userViewModel.Email;
                        user.PhoneNumber = userViewModel.PhoneNumber;
                        _context.Users.Update(user);

                        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userViewModel.UserId);
                        if (userRole != null)
                        {
                            _context.UserRoles.Remove(userRole);
                            await _context.SaveChangesAsync();

                            var newRoleId = userViewModel.Role == "Admin" ? "1" : userViewModel.Role == "Guest" ? "2" : null;
                            if (newRoleId != null)
                            {
                                var newUserRole = new IdentityUserRole<string>
                                {
                                    UserId = userViewModel.UserId,
                                    RoleId = newRoleId
                                };
                                _context.UserRoles.Add(newUserRole);
                            }
                        }

                        _context.Update(userViewModel);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        await transaction.RollbackAsync();

                        if (!UserViewModelExists(userViewModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }


        // GET: UserViewModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // POST: UserViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userViewModel = await _context.UserViewModel.FindAsync(id);
            if (userViewModel != null)
            {
                _context.UserViewModel.Remove(userViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserViewModelExists(string id)
        {
            return _context.UserViewModel.Any(e => e.Id == id);
        }
    }
}
