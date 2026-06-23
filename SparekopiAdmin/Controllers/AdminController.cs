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
        await EnsureKeysAsync("phone", "email", "address", "opening_hours", "vacation_notice");
        var items = await _context.SiteContents
            .Where(x => new[] { "phone", "email", "address", "opening_hours", "vacation_notice" }.Contains(x.Key))
            .ToDictionaryAsync(x => x.Key, x => x.Value);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Kontaktinfo(string phone, string email, string address, string opening_hours, string vacation_notice)
    {
        await UpsertAsync("phone", phone);
        await UpsertAsync("email", email);
        await UpsertAsync("address", address);
        await UpsertAsync("opening_hours", opening_hours);
        await UpsertAsync("vacation_notice", vacation_notice);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Kontaktinfo er oppdatert!";
        return RedirectToAction(nameof(Kontaktinfo));
    }

    public async Task<IActionResult> Forside()
    {
        ViewData["Title"] = "Forside-tekst";
        ViewData["PageTitle"] = "Rediger forside-tekst";
        await EnsureKeysAsync("hero_subtitle", "about_index");
        var items = await _context.SiteContents
            .Where(x => new[] { "hero_subtitle", "about_index" }.Contains(x.Key))
            .ToDictionaryAsync(x => x.Key, x => x.Value);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Forside(string hero_subtitle, string about_index)
    {
        await UpsertAsync("hero_subtitle", hero_subtitle);
        await UpsertAsync("about_index", about_index);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Forside-tekst er oppdatert!";
        return RedirectToAction(nameof(Forside));
    }

    public async Task<IActionResult> OmOssTekst()
    {
        ViewData["Title"] = "Om oss-tekst";
        ViewData["PageTitle"] = "Rediger Om oss-siden";
        await EnsureKeysAsync("about_omoss");
        var items = await _context.SiteContents
            .Where(x => new[] { "about_omoss" }.Contains(x.Key))
            .ToDictionaryAsync(x => x.Key, x => x.Value);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OmOssTekst(string about_omoss)
    {
        await UpsertAsync("about_omoss", about_omoss);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Om oss-tekst er oppdatert!";
        return RedirectToAction(nameof(OmOssTekst));
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
            ["hero_subtitle"] = "Toppmoderne trykkeri midt i Oslo. Vi leverer trykk, kopiering og reklamemateriell med nesten tre tiår daglig erfaring bak oss.",
            ["about_index"]   = "Vi er et av Oslos mest erfarne trykkerier. Siden oppstarten i 1997 har vi jobbet side om side med bedrifter, studenter og privatpersoner for å levere trykk som faktisk holder mål.\n\nMed toppmoderne maskinpark kan vi ta på oss det meste innen moderne trykk og reklameproduksjon.",
            ["about_omoss"]   = "Sparekopi ble stiftet i 1997 og ligger midt i Oslo sentrum på Torggata 17B — rett ovenfor Pascal. Vi har toppmoderne maskinpark og de nyeste maskinene innen moderne teknologi.\n\nMed snart tre tiår daglig erfaring kan vi gjøre de fleste jobber innen reklamer, kopiering og trykk. Vi jobber side om side med alt fra store bedrifter til studenter og privatpersoner."
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
