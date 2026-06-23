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

    private string VacationNotice() =>
        _context.SiteContents.FirstOrDefault(x => x.Key == "vacation_notice")?.Value ?? "";

    public IActionResult Index()
    {
        ViewBag.HeroTitle       = _context.SiteContents.FirstOrDefault(x => x.Key == "hero_title")?.Value    ?? "Sparekopi Oslo";
        ViewBag.HeroSubtitle    = _context.SiteContents.FirstOrDefault(x => x.Key == "hero_subtitle")?.Value ?? "Profesjonell printing og design";
        ViewBag.Phone           = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value         ?? "47 29 34 43";
        ViewBag.Address         = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value       ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours    = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 10:00 – 17:00";
        ViewBag.AboutIndex      = _context.SiteContents.FirstOrDefault(x => x.Key == "about_index")?.Value ?? "";
        ViewBag.VacationNotice  = VacationNotice();
        ViewBag.ServiceNames    = _context.ServiceItems
            .Where(x => x.Category == "tjeneste")
            .OrderBy(x => x.SortOrder)
            .Select(x => x.Name)
            .ToList();
        return View();
    }

    public async Task<IActionResult> Tjenester()
    {
        ViewBag.VacationNotice = VacationNotice();
        var items = await _context.ServiceItems
            .Where(x => x.Category == "tjeneste")
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Id)
            .ToListAsync();
        return View(items);
    }

    public IActionResult Priser()
    {
        ViewBag.Phone          = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value        ?? "47 29 34 43";
        ViewBag.Address        = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value      ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours   = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 10:00 – 17:00";
        ViewBag.VacationNotice = VacationNotice();
        return View();
    }

    public IActionResult OmOss()
    {
        ViewBag.Phone          = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value        ?? "47 29 34 43";
        ViewBag.Address        = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value      ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours   = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 10:00 – 17:00";
        ViewBag.AboutOmOss     = _context.SiteContents.FirstOrDefault(x => x.Key == "about_omoss")?.Value ?? "";
        ViewBag.VacationNotice = VacationNotice();
        return View();
    }

    public IActionResult Kontakt()
    {
        ViewBag.Phone          = _context.SiteContents.FirstOrDefault(x => x.Key == "phone")?.Value        ?? "47 29 34 43";
        ViewBag.Email          = _context.SiteContents.FirstOrDefault(x => x.Key == "email")?.Value        ?? "info@sparekopi.no";
        ViewBag.Address        = _context.SiteContents.FirstOrDefault(x => x.Key == "address")?.Value      ?? "Torggata 17B, 2. etasje, 0183 Oslo";
        ViewBag.OpeningHours   = _context.SiteContents.FirstOrDefault(x => x.Key == "opening_hours")?.Value ?? "Man–Fre 10:00 – 17:00";
        ViewBag.VacationNotice = VacationNotice();
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
