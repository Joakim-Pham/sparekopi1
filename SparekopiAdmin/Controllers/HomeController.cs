using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        ViewBag.HeroTitle = _context.SiteContents
            .FirstOrDefault(x => x.Key == "hero_title")?.Value ?? "Sparekopi Oslo";

        ViewBag.HeroSubtitle = _context.SiteContents
            .FirstOrDefault(x => x.Key == "hero_subtitle")?.Value ?? "Profesjonell printing og design";

        ViewBag.Phone = _context.SiteContents
            .FirstOrDefault(x => x.Key == "phone")?.Value ?? "47 29 34 43";

        return View();
    }

    public IActionResult Tjenester()
    {
        return View();
    }

    public IActionResult Priser()
    {
        return View();
    }

    public IActionResult OmOss()
    {
        return View();
    }

    public IActionResult Kontakt()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}