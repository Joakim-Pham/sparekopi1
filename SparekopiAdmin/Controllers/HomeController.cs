using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Data;
using SparekopiAdmin.Models;

namespace SparekopiAdmin.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.HeroTitle    = _context.SiteContents.FirstOrDefault(x => x.Key == "hero_title")?.Value    ?? "Sparekopi Oslo";
        ViewBag.HeroSubtitle = _context.SiteContents.FirstOrDefault(x => x.Key == "hero_subtitle")?.Value ?? "Profesjonell printing og design";
        ViewBag.Phone        = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value         ?? "47 29 34 43";
        return View();
    }

    public async Task<IActionResult> Tjenester()
    {
        var items = await _context.ServiceItems
            .Where(x => x.Category == "tjeneste")
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Id)
            .ToListAsync();
        return View(items);
    }

    public IActionResult Priser()
    {
        return View();
    }

    public IActionResult OmOss()
    {
        ViewBag.Phone        = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value        ?? "47 29 34 43";
        ViewBag.Address      = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value      ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 09:00 – 17:00";
        return View();
    }

    public IActionResult Kontakt()
    {
        ViewBag.Phone        = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value        ?? "47 29 34 43";
        ViewBag.Email        = _context.SiteContents.FirstOrDefault(x => x.Key == "email")?.Value        ?? "post@sparekopi.no";
        ViewBag.Address      = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value      ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 09:00 – 17:00";
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
