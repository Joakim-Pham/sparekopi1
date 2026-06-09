namespace SparekopiAdmin.Models;

public class ServiceItem
{
    public int Id { get; set; }

    public string Category { get; set; } = "pris";         // "tjeneste" | "pris"
    public string Section { get; set; } = "";              // grouping header on page
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";    // card tagline
    public string Description { get; set; } = "";         // modal / long description
    public string Features { get; set; } = "";            // newline-separated bullet points
    public decimal Price { get; set; }
    public string PricePrefix { get; set; } = "";         // "Fra " for approximate prices
    public string PriceUnit { get; set; } = "";           // e.g. "per A4-ark"
    public string ImagePath { get; set; } = "";           // e.g. /assets/images/flyer.jpg
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}
