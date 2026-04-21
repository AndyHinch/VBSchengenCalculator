using SchengenCalculator.Models;

namespace SchengenCalculator.Services;

/// <summary>
/// Built-in lists of common UK departure airports and Schengen-area arrival airports.
/// </summary>
public static class AirportData
{
    public static readonly IReadOnlyList<Airport> UkAirports =
    [
        // London
        new("LHR", "Heathrow",          "London",           "United Kingdom"),
        new("LGW", "Gatwick",           "London",           "United Kingdom"),
        new("STN", "Stansted",          "London",           "United Kingdom"),
        new("LTN", "Luton",             "London",           "United Kingdom"),
        new("LCY", "City Airport",      "London",           "United Kingdom"),
        new("SEN", "Southend",          "London",           "United Kingdom"),
        // England
        new("MAN", "Manchester",        "Manchester",       "United Kingdom"),
        new("BHX", "Birmingham",        "Birmingham",       "United Kingdom"),
        new("BRS", "Bristol",           "Bristol",          "United Kingdom"),
        new("NCL", "Newcastle",         "Newcastle",        "United Kingdom"),
        new("LBA", "Leeds Bradford",    "Leeds",            "United Kingdom"),
        new("LPL", "Liverpool John Lennon", "Liverpool",    "United Kingdom"),
        new("EMA", "East Midlands",     "Nottingham",       "United Kingdom"),
        new("SOU", "Southampton",       "Southampton",      "United Kingdom"),
        new("EXT", "Exeter",            "Exeter",           "United Kingdom"),
        new("NWI", "Norwich",           "Norwich",          "United Kingdom"),
        new("HUY", "Humberside",        "Humberside",       "United Kingdom"),
        new("DSA", "Doncaster Sheffield","Doncaster",       "United Kingdom"),
        new("BOH", "Bournemouth",       "Bournemouth",      "United Kingdom"),
        new("NQY", "Newquay Cornwall",  "Newquay",          "United Kingdom"),
        // Scotland
        new("EDI", "Edinburgh",         "Edinburgh",        "United Kingdom"),
        new("GLA", "Glasgow",           "Glasgow",          "United Kingdom"),
        new("ABZ", "Aberdeen",          "Aberdeen",         "United Kingdom"),
        new("INV", "Inverness",         "Inverness",        "United Kingdom"),
        new("GCI", "Guernsey",          "Guernsey",         "United Kingdom"),
        // Northern Ireland
        new("BFS", "Belfast International", "Belfast",      "United Kingdom"),
        new("BHD", "Belfast City (George Best)", "Belfast", "United Kingdom"),
        // Wales
        new("CWL", "Cardiff",           "Cardiff",          "United Kingdom"),
    ];

    public static readonly IReadOnlyList<Airport> SchengenAirports =
    [
        // ── Spain ──
        new("MAD", "Adolfo Suárez Madrid–Barajas", "Madrid",       "Spain"),
        new("BCN", "Barcelona–El Prat",             "Barcelona",    "Spain"),
        new("PMI", "Palma de Mallorca",             "Mallorca",     "Spain"),
        new("AGP", "Málaga–Costa del Sol",          "Málaga",       "Spain"),
        new("ALC", "Alicante–Elche",                "Alicante",     "Spain"),
        new("TFS", "Tenerife South",                "Tenerife",     "Spain"),
        new("TFN", "Tenerife North",                "Tenerife",     "Spain"),
        new("LPA", "Gran Canaria",                  "Las Palmas",   "Spain"),
        new("FUE", "Fuerteventura",                 "Fuerteventura","Spain"),
        new("ACE", "Lanzarote",                     "Lanzarote",    "Spain"),
        new("IBZ", "Ibiza",                         "Ibiza",        "Spain"),
        new("VLC", "Valencia",                      "Valencia",     "Spain"),
        new("SVQ", "Seville",                       "Seville",      "Spain"),
        new("BIO", "Bilbao",                        "Bilbao",       "Spain"),
        new("GRX", "Federico García Lorca Granada", "Granada",      "Spain"),
        new("SDR", "Santander",                     "Santander",    "Spain"),
        new("MLN", "Melilla",                       "Melilla",      "Spain"),
        new("MAH", "Menorca",                       "Menorca",      "Spain"),

        // ── France ──
        new("CDG", "Charles de Gaulle",  "Paris",      "France"),
        new("ORY", "Orly",               "Paris",      "France"),
        new("NCE", "Côte d'Azur",        "Nice",       "France"),
        new("MRS", "Marseille Provence", "Marseille",  "France"),
        new("LYS", "Saint-Exupéry",      "Lyon",       "France"),
        new("TLS", "Toulouse-Blagnac",   "Toulouse",   "France"),
        new("NTE", "Nantes Atlantique",  "Nantes",     "France"),
        new("BOD", "Bordeaux-Mérignac",  "Bordeaux",   "France"),
        new("MPL", "Montpellier",        "Montpellier","France"),
        new("SXB", "Strasbourg",         "Strasbourg", "France"),
        new("BIA", "Bastia–Poretta",     "Bastia",     "France"),
        new("AJA", "Ajaccio",            "Ajaccio",    "France"),

        // ── Italy ──
        new("FCO", "Leonardo da Vinci–Fiumicino", "Rome",    "Italy"),
        new("MXP", "Malpensa",                     "Milan",   "Italy"),
        new("LIN", "Linate",                       "Milan",   "Italy"),
        new("BGY", "Orio al Serio",                "Bergamo", "Italy"),
        new("VCE", "Marco Polo",                   "Venice",  "Italy"),
        new("NAP", "Naples",                       "Naples",  "Italy"),
        new("PSA", "Galileo Galilei",              "Pisa",    "Italy"),
        new("BLQ", "Guglielmo Marconi",            "Bologna", "Italy"),
        new("FLR", "Peretola",                     "Florence","Italy"),
        new("CTA", "Catania-Fontanarossa",         "Catania", "Italy"),
        new("PMO", "Falcone–Borsellino",           "Palermo", "Italy"),
        new("BRI", "Bari Karol Wojtyła",           "Bari",    "Italy"),
        new("VRN", "Valerio Catullo",              "Verona",  "Italy"),

        // ── Germany ──
        new("FRA", "Frankfurt",       "Frankfurt",  "Germany"),
        new("MUC", "Munich",          "Munich",     "Germany"),
        new("BER", "Brandenburg",     "Berlin",     "Germany"),
        new("DUS", "Düsseldorf",      "Düsseldorf", "Germany"),
        new("HAM", "Hamburg",         "Hamburg",    "Germany"),
        new("STR", "Stuttgart",       "Stuttgart",  "Germany"),
        new("CGN", "Cologne/Bonn",    "Cologne",    "Germany"),
        new("NUE", "Nuremberg",       "Nuremberg",  "Germany"),

        // ── Netherlands ──
        new("AMS", "Schiphol",   "Amsterdam", "Netherlands"),
        new("EIN", "Eindhoven",  "Eindhoven", "Netherlands"),
        new("RTM", "Rotterdam",  "Rotterdam", "Netherlands"),

        // ── Portugal ──
        new("LIS", "Humberto Delgado", "Lisbon", "Portugal"),
        new("OPO", "Francisco Sá Carneiro", "Porto", "Portugal"),
        new("FAO", "Faro",              "Faro",   "Portugal"),
        new("FNC", "Madeira",           "Madeira","Portugal"),
        new("PDL", "João Paulo II",     "Azores", "Portugal"),

        // ── Greece ──
        new("ATH", "Eleftherios Venizelos", "Athens",      "Greece"),
        new("HER", "Nikos Kazantzakis",     "Heraklion",   "Greece"),
        new("SKG", "Makedonia",             "Thessaloniki","Greece"),
        new("CFU", "Ioannis Kapodistrias",  "Corfu",       "Greece"),
        new("RHO", "Diagoras",              "Rhodes",      "Greece"),
        new("KGS", "Hippocrates",           "Kos",         "Greece"),
        new("ZTH", "Dionysios Solomos",     "Zakynthos",   "Greece"),
        new("JMK", "Mykonos",               "Mykonos",     "Greece"),
        new("JSI", "Skiathos",              "Skiathos",    "Greece"),
        new("SMI", "Samos",                 "Samos",       "Greece"),
        new("KLX", "Kalamata",              "Kalamata",    "Greece"),
        new("EFL", "Cephalonia",            "Kefalonia",   "Greece"),

        // ── Croatia ──
        new("ZAG", "Franjo Tuđman", "Zagreb",    "Croatia"),
        new("SPU", "Split",         "Split",     "Croatia"),
        new("DBV", "Dubrovnik",     "Dubrovnik", "Croatia"),
        new("PUY", "Pula",          "Pula",      "Croatia"),
        new("ZAD", "Zadar",         "Zadar",     "Croatia"),

        // ── Austria ──
        new("VIE", "Vienna",     "Vienna",    "Austria"),
        new("INN", "Innsbruck",  "Innsbruck", "Austria"),
        new("SZG", "Salzburg",   "Salzburg",  "Austria"),
        new("GRZ", "Graz",       "Graz",      "Austria"),

        // ── Switzerland ──
        new("ZRH", "Zurich",   "Zurich",  "Switzerland"),
        new("GVA", "Geneva",   "Geneva",  "Switzerland"),
        new("BSL", "EuroAirport Basel-Mulhouse", "Basel", "Switzerland"),

        // ── Belgium ──
        new("BRU", "Brussels",   "Brussels", "Belgium"),
        new("CRL", "Charleroi",  "Charleroi","Belgium"),

        // ── Denmark ──
        new("CPH", "Copenhagen", "Copenhagen", "Denmark"),
        new("BLL", "Billund",    "Billund",    "Denmark"),
        new("AAL", "Aalborg",    "Aalborg",    "Denmark"),

        // ── Sweden ──
        new("ARN", "Stockholm Arlanda", "Stockholm", "Sweden"),
        new("GOT", "Gothenburg Landvetter", "Gothenburg", "Sweden"),
        new("MMX", "Malmö",             "Malmö",     "Sweden"),

        // ── Norway (Schengen) ──
        new("OSL", "Oslo Gardermoen", "Oslo",    "Norway"),
        new("BGO", "Bergen",          "Bergen",  "Norway"),
        new("SVG", "Stavanger",       "Stavanger","Norway"),
        new("TRD", "Trondheim",       "Trondheim","Norway"),

        // ── Finland ──
        new("HEL", "Helsinki-Vantaa", "Helsinki", "Finland"),

        // ── Poland ──
        new("WAW", "Frederic Chopin",  "Warsaw",  "Poland"),
        new("KRK", "John Paul II",     "Krakow",  "Poland"),
        new("GDN", "Lech Wałęsa",      "Gdansk",  "Poland"),
        new("WRO", "Copernicus",       "Wroclaw", "Poland"),

        // ── Czech Republic ──
        new("PRG", "Václav Havel", "Prague", "Czech Republic"),

        // ── Hungary ──
        new("BUD", "Budapest Ferenc Liszt", "Budapest", "Hungary"),

        // ── Slovakia ──
        new("BTS", "Milan Rastislav Štefánik", "Bratislava", "Slovakia"),

        // ── Slovenia ──
        new("LJU", "Ljubljana Jože Pučnik", "Ljubljana", "Slovenia"),

        // ── Malta ──
        new("MLA", "Malta", "Valletta", "Malta"),

        // ── Cyprus ──
        new("LCA", "Larnaca", "Larnaca", "Cyprus"),
        new("PFO", "Paphos",  "Paphos",  "Cyprus"),

        // ── Iceland (Schengen) ──
        new("KEF", "Keflavík", "Reykjavik", "Iceland"),

        // ── Luxembourg ──
        new("LUX", "Luxembourg", "Luxembourg", "Luxembourg"),
    ];
}
