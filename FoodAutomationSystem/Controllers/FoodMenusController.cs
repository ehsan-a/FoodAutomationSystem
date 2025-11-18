using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodAutomationSystem.Data;
using FoodAutomationSystem.Models;

namespace FoodAutomationSystem.Controllers
{
    public class FoodMenusController : Controller
    {
        private readonly FoodAutomationSystemContext _context;

        public FoodMenusController(FoodAutomationSystemContext context)
        {
            _context = context;
        }

        // GET: FoodMenus
        public async Task<IActionResult> Index()
        {
            var foodAutomationSystemContext = _context.FoodMenu.Include(f => f.Food).Include(f => f.Menu);
            return View(await foodAutomationSystemContext.ToListAsync());
        }

        // GET: FoodMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodMenu = await _context.FoodMenu
                .Include(f => f.Food)
                .Include(f => f.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodMenu == null)
            {
                return NotFound();
            }

            return View(foodMenu);
        }

        // GET: FoodMenus/Create
        public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.Food, "Id", "Id");
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "Id", "Id");
            return View();
        }

        // POST: FoodMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuId,FoodId,DayOfWeek")] FoodMenu foodMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(foodMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.Food, "Id", "Id", foodMenu.FoodId);
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "Id", "Id", foodMenu.MenuId);
            return View(foodMenu);
        }

        // GET: FoodMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodMenu = await _context.FoodMenu.FindAsync(id);
            if (foodMenu == null)
            {
                return NotFound();
            }
            ViewData["FoodId"] = new SelectList(_context.Food, "Id", "Id", foodMenu.FoodId);
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "Id", "Id", foodMenu.MenuId);
            return View(foodMenu);
        }

        // POST: FoodMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuId,FoodId,DayOfWeek")] FoodMenu foodMenu)
        {
            if (id != foodMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodMenuExists(foodMenu.Id))
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
            ViewData["FoodId"] = new SelectList(_context.Food, "Id", "Id", foodMenu.FoodId);
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "Id", "Id", foodMenu.MenuId);
            return View(foodMenu);
        }

        // GET: FoodMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodMenu = await _context.FoodMenu
                .Include(f => f.Food)
                .Include(f => f.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodMenu == null)
            {
                return NotFound();
            }

            return View(foodMenu);
        }

        // POST: FoodMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodMenu = await _context.FoodMenu.FindAsync(id);
            if (foodMenu != null)
            {
                _context.FoodMenu.Remove(foodMenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodMenuExists(int id)
        {
            return _context.FoodMenu.Any(e => e.Id == id);
        }
    }
}
