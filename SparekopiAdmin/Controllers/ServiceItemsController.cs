using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Data;
using SparekopiAdmin.Models;

namespace SparekopiAdmin.Controllers
{
    public class ServiceItemsController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceItems.ToListAsync());
        }

        // GET: ServiceItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceItem == null)
            {
                return NotFound();
            }

            return View(serviceItem);
        }

        // GET: ServiceItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return NotFound();
            }
            return View(serviceItem);
        }

        // POST: ServiceItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] ServiceItem serviceItem)
        {
            if (id != serviceItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceItemExists(serviceItem.Id))
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
            return View(serviceItem);
        }

        // GET: ServiceItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceItem == null)
            {
                return NotFound();
            }

            return View(serviceItem);
        }

        // POST: ServiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceItem = await _context.ServiceItems.FindAsync(id);
            if (serviceItem != null)
            {
                _context.ServiceItems.Remove(serviceItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceItemExists(int id)
        {
            return _context.ServiceItems.Any(e => e.Id == id);
        }
    }
}
