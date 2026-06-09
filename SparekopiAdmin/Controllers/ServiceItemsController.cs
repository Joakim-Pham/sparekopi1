using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Data;
using SparekopiAdmin.Models;

namespace SparekopiAdmin.Controllers;

[Authorize]
public class ServiceItemsController : Controller
{
    private readonly AppDbContext _context;

    public ServiceItemsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? category = null)
    {
        ViewBag.Category = category;
        var query = _context.ServiceItems.AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(x => x.Category == category);
        return View(await query.OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ServiceItems.FirstOrDefaultAsync(m => m.Id == id);
        return item == null ? NotFound() : View(item);
    }

    public IActionResult Create(string? category = null)
    {
        ViewBag.Category = category ?? "pris";
        return View(new ServiceItem { Category = category ?? "pris" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Category,Section,Name,ShortDescription,Description,Features,Price,PricePrefix,PriceUnit,ImagePath,IsFeatured,SortOrder")] ServiceItem item)
    {
        if (ModelState.IsValid)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { category = item.Category });
        }
        ViewBag.Category = item.Category;
        return View(item);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ServiceItems.FindAsync(id);
        if (item == null) return NotFound();
        ViewBag.Category = item.Category;
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Category,Section,Name,ShortDescription,Description,Features,Price,PricePrefix,PriceUnit,ImagePath,IsFeatured,SortOrder")] ServiceItem item)
    {
        if (id != item.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ServiceItems.Any(e => e.Id == item.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index), new { category = item.Category });
        }
        ViewBag.Category = item.Category;
        return View(item);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ServiceItems.FirstOrDefaultAsync(m => m.Id == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.ServiceItems.FindAsync(id);
        string category = item?.Category ?? "pris";
        if (item != null) _context.ServiceItems.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { category });
    }
}
