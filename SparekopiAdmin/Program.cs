using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Data;
using SparekopiAdmin.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var dbPath = Environment.GetEnvironmentVariable("DB_PATH") ?? "sparekopi.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Seed default content
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedContent(db);
    SeedServices(db);
    SeedPrices(db);
    PatchServiceNames(db);
    RemovePricesFromFeatures(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static void SeedContent(AppDbContext db)
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
    foreach (var kv in defaults)
    {
        if (!db.SiteContents.Any(x => x.Key == kv.Key))
            db.SiteContents.Add(new SiteContent { Key = kv.Key, Value = kv.Value });
    }
    db.SaveChanges();
}

static void SeedServices(AppDbContext db)
{
    if (db.ServiceItems.Any(x => x.Category == "tjeneste")) return;

    // Remove any legacy records with empty category
    var legacy = db.ServiceItems.Where(x => x.Category == "").ToList();
    db.ServiceItems.RemoveRange(legacy);

    db.ServiceItems.AddRange(
        new ServiceItem { Category="tjeneste", Section="Trykk & Kopiering", SortOrder=1,
            Name="Digitalt trykk og oppsett",
            ShortDescription="Rask og presis utskrift i høy kvalitet — fra enkeltark til store opplag.",
            Description="Rask og presis digital utskrift i høy kvalitet — fra enkeltark til store opplag. Vi bruker vår Xerox PrimeLink C9200 for skarp detalj og ekte farger på alt vi produserer.",
            Features="A4, A3, A2 og storformat\nSvart-hvitt og farge\nFotokvalitet fargegjengivelse\nRask levering\nMengderabatt ved store opplag\nEnkelt- og dobbeltsidig",
            ImagePath="/assets/images/digital_print.jpg" },
        new ServiceItem { Category="tjeneste", Section="Trykk & Kopiering", SortOrder=2,
            Name="Kopiering & Flyere",
            ShortDescription="Profesjonelle flyere og rask kopiering i svart-hvitt og farge.",
            Description="Profesjonelle flyere og lynrask kopiering i svart-hvitt og farge. Vi kopierer dokumenter, tegninger og bilder raskt og effektivt med nøyaktig fargegjengivelse.",
            Features="Svart-hvitt fra kr 2,- per ark\nFarge fra kr 8,- per ark\nA4 og A3 format\nEnkelt- og dobbeltsidig\nMengderabatt tilgjengelig\nOfte samme dag",
            ImagePath="/assets/images/flyer.jpg" },
        new ServiceItem { Category="tjeneste", Section="Trykk & Kopiering", SortOrder=3,
            Name="Brosjyrer & Kataloger",
            ShortDescription="Fra enkle trefoldede brosjyrer til flersiders kataloger med profesjonell innbinding.",
            Description="Fra enkle trefoldede brosjyrer til flersiders kataloger med profesjonell innbinding. Vi hjelper deg gjennom hele prosessen — fra design til ferdig produkt.",
            Features="Enkelt- og trefoldede brosjyrer\nKataloger opp til 100+ sider\nA4, A5 og egendefinerte formater\nMatt eller blank finish\nMengderabatt fra 100 stk\nRask levering",
            ImagePath="/assets/images/brosjyrer.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=4,
            Name="Reklameplakater & Skilt",
            ShortDescription="Store og små plakater for events og kampanjer. Alle standardformater og egendefinerte størrelser.",
            Description="Store og små plakater, kundestoppere og skilt for butikker, salonger og events. Vi trykker i alle standardformater.",
            Features="Alle standardformater\nA2, A1, A0 storformat\nKundestoppere\nInnendørs og utendørs\nLaminering tilgjengelig\nKlar på 1–3 virkedager",
            ImagePath="/assets/images/skilt.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=5,
            Name="Bannere & Roll-ups",
            ShortDescription="Profesjonelle bannere og roll-ups for messer og konferanser.",
            Description="Profesjonelle roll-ups og bannere for messer, konferanser og butikker. Holdbart materiale og klart trykk — inkluderer stativ og bag.",
            Features="Roll-ups 85x200cm og 100x200cm\nInkluderer stativ og bag\nBannere i alle størrelser\nHoldbart materiale\nFlerfarget høykvalitetstrykk\nKlar på 1–3 virkedager",
            ImagePath="/assets/images/ROLLUP.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=6,
            Name="Beach Flags",
            ShortDescription="Beach flags og flaggstenger — synes fra lang avstand, perfekt utendørs.",
            Description="Beach flags og flaggstenger er perfekte for å skille seg ut utendørs — foran butikk, på messer eller ved events.",
            Features="Beach flags i ulike størrelser\nTeardrop og rektangulær form\nInkluderer stativ og bag\nUV-bestandig materiale\nTåler vær og vind\nKlar på 2–4 virkedager",
            ImagePath="/assets/images/beachflagg.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=7,
            Name="Veggdekorasjon",
            ShortDescription="Imponerende veggdekor i storformat — perfekt for salonger, butikker og kontorer.",
            Description="Imponerende veggdekorasjon i storformat — perfekt for salonger, butikker, kontorer og restauranter.",
            Features="Egendefinerte størrelser\nHøyoppløselig trykk\nKlar til montering\nPerfekt for salonger og butikker\nHoldbart materiale\nProfesjonell finish",
            ImagePath="/assets/images/veggdekor.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=8,
            Name="Vindusdekor",
            ShortDescription="Vindusdekor og skiltdekorasjon som gjør butikkfronten din til en magnet.",
            Description="Vindusdekor og skiltdekorasjon som gjør butikkfronten din til en magnet. Fargerikt, profesjonelt og holdbart.",
            Features="Egendefinerte størrelser\nKlar til montering\nPerfekt for butikkfronter\nUV-bestandig\nHoldbart materiale\nProfesjonell finish",
            ImagePath="/assets/images/vindusdekor.jpg" },
        new ServiceItem { Category="tjeneste", Section="Reklame & Skilt", SortOrder=9,
            Name="Fasadebanner og vanlig banner",
            ShortDescription="Bannere og fasadeskilt i alle størrelser — for utendørs og innendørs bruk.",
            Description="Vi produserer bannere og fasadeskilt i alle størrelser. Perfekt for butikkfronter, messer og arrangementer.",
            Features="Alle størrelser\nInnendørs og utendørs\nHoldbart materiale\nHøykvalitetstrykk\nRask levering\nKlar på 1–3 virkedager",
            ImagePath="/assets/images/banner.jpg" },
        new ServiceItem { Category="tjeneste", Section="Etterbehandling", SortOrder=10,
            Name="Visittkort",
            ShortDescription="Profesjonelle visittkort i ulike formater og papirkvaliteter.",
            Description="Et godt visittkort gjør det riktige inntrykket fra første møte. Vi trykker visittkort i ulike formater, papirkvaliteter og finish.",
            Features="Standard 85x55mm\n350g og 400g papirkvalitet\nMatt eller blank laminering\nEnsidig og tosidig trykk\nAvrundede hjørner\nFra 100 stk",
            ImagePath="/assets/images/visittkort.jpg" },
        new ServiceItem { Category="tjeneste", Section="Etterbehandling", SortOrder=11,
            Name="Spiralinnbinding",
            ShortDescription="Spiral, limrygg og stifting — rapporter og oppgaver bundet profesjonelt og raskt.",
            Description="Vi binder inn rapporter, oppgaver, presentasjoner og manualer profesjonelt og raskt.",
            Features="Plastspiral og metallspiral\nLimrygsbinding\nStifting for tynnere hefter\nInkluderer for- og bakside\nOpp til 400 sider\nRask utlevering — ofte samme dag",
            ImagePath="/assets/images/spiralinnbinding.jpg" },
        new ServiceItem { Category="tjeneste", Section="Etterbehandling", SortOrder=12,
            Name="Laminering & Menyer",
            ShortDescription="Matt eller blank laminering — perfekt for menyer, sertifikater og skilt.",
            Description="Laminering beskytter trykket ditt mot fukt, søl og slitasje — perfekt for menyer, prislister, sertifikater og skilt.",
            Features="Matt og blank laminering\nA4, A3 og egendefinerte størrelser\nEnkelt- og dobbeltsidig\nPerfekt for restaurantmenyer\nBeskytter mot fukt og flekker\nFra kr 20,- per ark",
            ImagePath="/assets/images/meny.jpg" },
        new ServiceItem { Category="tjeneste", Section="Profilklær", SortOrder=13,
            Name="T-skjorter & Profilklær",
            ShortDescription="Vi trykker logo og design på t-skjorter og profilplagg for bedrifter og events.",
            Description="Vi trykker logo og design på t-skjorter, polo-skjorter og annet profilplagg — perfekt for bedrifter, restauranter og events.",
            Features="T-skjorter og polo\nAlle størrelser S–3XL\nLogo og flerfarget trykk\nMinimum 5 stk\nHoldbart og vaskefast trykk\nLeveringstid 5–10 virkedager",
            ImagePath="/assets/images/tskjorter.jpg" },
        new ServiceItem { Category="tjeneste", Section="Profilklær", SortOrder=14,
            Name="Caps & Hodeplagg",
            ShortDescription="Caps med logo — perfekt som profileringsgave eller uniformsdel i mange farger.",
            Description="Caps med brodert eller trykt logo — perfekt som profileringsgave eller uniformsdel. Tilgjengelig i mange farger og størrelser.",
            Features="Mange farger og varianter\nBrodert eller trykt logo\nOne size med justerbar rem\nMinimum 10 stk\nHoldbart og slitesterkt\nLeveringstid 5–10 virkedager",
            ImagePath="/assets/images/caps.jpg" },
        new ServiceItem { Category="tjeneste", Section="Profilklær", SortOrder=15,
            Name="Mer profilmateriell",
            ShortDescription="Vi tilbyr et bredt utvalg av profilmateriell — ta kontakt for å høre om mulighetene.",
            Description="Vi tilbyr et bredt utvalg av profilmateriell utover klær — ta kontakt for å høre om mulighetene.",
            Features="Vesker og bager\nPenner og kontormateriell\nNøkkelringer og pins\nEgendefinerte produkter\nKontakt oss for tilbud\nLeveringstid varierer",
            ImagePath="/assets/images/digital_print.jpg" }
    );
    db.SaveChanges();
}

static void SeedPrices(AppDbContext db)
{
    if (db.ServiceItems.Any(x => x.Category == "pris")) return;

    db.ServiceItems.AddRange(
        new ServiceItem { Category="pris", Section="Kopiering", SortOrder=1,
            Name="Svart-hvitt", Price=2, PriceUnit="per A4-ark",
            Features="Enkeltsidig\nA4 og A3\nMengderabatt tilgjengelig" },
        new ServiceItem { Category="pris", Section="Kopiering", SortOrder=2,
            Name="Farge", Price=8, PriceUnit="per A4-ark", IsFeatured=true,
            Features="Enkeltsidig\nA4 og A3\nFotokvalitet\nMengderabatt tilgjengelig" },
        new ServiceItem { Category="pris", Section="Kopiering", SortOrder=3,
            Name="Storformat", Price=40, PricePrefix="Fra ", PriceUnit="per ark (A2–A0)",
            Features="A2, A1, A0\nFarge eller s/h\nTekniske tegninger" },
        new ServiceItem { Category="pris", Section="Etterbehandling", SortOrder=4,
            Name="Laminering", Price=20, PriceUnit="per A4-ark",
            Features="Matt eller blank\nA4 og A3" },
        new ServiceItem { Category="pris", Section="Etterbehandling", SortOrder=5,
            Name="Spiralinnbinding", Price=45, PriceUnit="per dokument", IsFeatured=true,
            Features="Plastspiral eller metall\nInkluderer forside\nOpp til 200 sider" },
        new ServiceItem { Category="pris", Section="Etterbehandling", SortOrder=6,
            Name="Limrygsbinding", Price=80, PricePrefix="Fra ", PriceUnit="per dokument",
            Features="Profesjonell bok-finish\nOpp til 400 sider" }
    );
    db.SaveChanges();
}

static void PatchServiceNames(AppDbContext db)
{
    var digitalTrykk = db.ServiceItems.FirstOrDefault(x => x.Name == "Digitalt trykk");
    if (digitalTrykk != null)
    {
        digitalTrykk.Name = "Digitalt trykk og oppsett";
        db.SaveChanges();
    }

    var digital = db.ServiceItems.FirstOrDefault(x => x.Name == "Digitale bannere");
    if (digital != null)
    {
        digital.Name = "Fasadebanner og vanlig banner";
        digital.ShortDescription = "Bannere og fasadeskilt i alle størrelser — for utendørs og innendørs bruk.";
        digital.Description = "Vi produserer bannere og fasadeskilt i alle størrelser. Perfekt for butikkfronter, messer og arrangementer.";
        digital.Features = "Alle størrelser\nInnendørs og utendørs\nHoldbart materiale\nHøykvalitetstrykk\nRask levering\nKlar på 1–3 virkedager";
        db.SaveChanges();
    }

    var skilt = db.ServiceItems.FirstOrDefault(x => x.Name == "Reklameplakater & Skilt");
    if (skilt != null && !skilt.ImagePath.Contains("~"))
    {
        skilt.ImagePath = "/assets/images/skilt.jpg~/assets/images/fasadeskilt.jpg";
        db.SaveChanges();
    }
}

static void RemovePricesFromFeatures(AppDbContext db)
{
    var items = db.ServiceItems.Where(x => x.Category == "tjeneste" && x.Features.Contains("kr")).ToList();
    foreach (var item in items)
    {
        var lines = item.Features.Split('\n')
            .Where(l => !l.Contains("kr"))
            .ToArray();
        item.Features = string.Join('\n', lines);
    }
    if (items.Count > 0) db.SaveChanges();
}
