using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Data;
using SparekopiAdmin.Models;

namespace SparekopiAdmin.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        ViewData["PageTitle"] = "Dashboard";
        ViewBag.ServiceCount = await _context.ServiceItems.CountAsync();
        ViewBag.ContentCount = await _context.SiteContents.CountAsync();
        return View();
    }

    public async Task<IActionResult> Kontaktinfo()
    {
        ViewData["Title"] = "Kontaktinfo";
        ViewData["PageTitle"] = "Rediger kontaktinfo";
        await EnsureKeysAsync("phone", "email", "address", "opening_hours");
        var items = await _context.SiteContents
            .Where(x => new[] { "phone", "email", "address", "opening_hours" }.Contains(x.Key))
            .ToDictionaryAsync(x => x.Key, x => x.Value);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Kontaktinfo(string phone, string email, string address, string opening_hours)
    {
        await UpsertAsync("phone", phone);
        await UpsertAsync("email", email);
        await UpsertAsync("address", address);
        await UpsertAsync("opening_hours", opening_hours);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Kontaktinfo er oppdatert!";
        return RedirectToAction(nameof(Kontaktinfo));
    }

    public async Task<IActionResult> Forside()
    {
        ViewData["Title"] = "Forside-tekst";
        ViewData["PageTitle"] = "Rediger forside-tekst";
        await EnsureKeysAsync("hero_title", "hero_subtitle");
        var items = await _context.SiteContents
            .Where(x => new[] { "hero_title", "hero_subtitle" }.Contains(x.Key))
            .ToDictionaryAsync(x => x.Key, x => x.Value);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Forside(string hero_title, string hero_subtitle)
    {
        await UpsertAsync("hero_title", hero_title);
        await UpsertAsync("hero_subtitle", hero_subtitle);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Forside-tekst er oppdatert!";
        return RedirectToAction(nameof(Forside));
    }

    private async Task EnsureKeysAsync(params string[] keys)
    {
        var defaults = new Dictionary<string, string>
        {
            ["phone"]         = "47 29 34 43",
            ["email"]         = "info@sparekopi.no",
            ["address"]       = "Torggata 17B, 2. etasje, 0183 Oslo",
            ["opening_hours"] = "Mandag – Fredag: 10:00 – 17:00",
            ["hero_title"]    = "Kvalitet som varer siden 1997",
            ["hero_subtitle"] = "Toppmoderne trykkeri midt i Oslo."
        };

        bool changed = false;
        foreach (var key in keys)
        {
            if (!await _context.SiteContents.AnyAsync(x => x.Key == key))
            {
                _context.SiteContents.Add(new SiteContent
                {
                    Key   = key,
                    Value = defaults.GetValueOrDefault(key, "")
                });
                changed = true;
            }
        }
        if (changed) await _context.SaveChangesAsync();
    }

    private async Task UpsertAsync(string key, string? value)
    {
        var item = await _context.SiteContents.FirstOrDefaultAsync(x => x.Key == key);
        if (item == null)
            _context.SiteContents.Add(new SiteContent { Key = key, Value = value ?? "" });
        else
            item.Value = value ?? "";
    }
}
