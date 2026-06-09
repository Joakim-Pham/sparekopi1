# Sparekopi – Nettside

Komplett nettside for Sparekopi AS, trykkeri og kopiering i Oslo sentrum.

---

## 📁 Mappestruktur

```
sparekopi/
├── index.html              ← Forside (åpne denne i nettleseren)
├── css/
│   ├── variables.css       ← Alle farger, fonter og spacing-variabler
│   ├── base.css            ← Reset og grunnleggende stiler
│   ├── nav-footer.css      ← Navigasjon og footer
│   ├── hero.css            ← Hero-komponenter (forside + innersider)
│   ├── components.css      ← Kort, tjenester, om-oss, priser, kart
│   └── forms.css           ← Kontaktskjema og skjemaelementer
├── js/
│   └── main.js             ← Mobilmeny, scroll-animasjoner, skjema
└── pages/
    ├── _template.html      ← Mal med nav/footer-kode å kopiere
    ├── tjenester.html      ← Tjenesteside
    ├── priser.html         ← Prisside
    ├── om-oss.html         ← Om oss-side
    └── kontakt.html        ← Kontaktside med skjema og kart
```

---

## 🚀 Slik bruker du prosjektet i VSCode

1. Åpne mappen `sparekopi/` i VSCode
2. Installer Live Server-utvidelsen (anbefalt):  
   `Extensions → søk "Live Server" → Install`
3. Høyreklikk på `index.html` → **"Open with Live Server"**
4. Nettsiden åpnes automatisk i nettleseren og oppdateres live ved endringer

---

## 🎨 Redigere farger og stiler

Alle design-tokens er samlet i **`css/variables.css`**.  
Endre verdiene der for å retheme hele siden på én gang:

```css
:root {
  --color-red:   #e3281b;  /* Aksentfarge */
  --color-black: #0d0d0d;  /* Bakgrunn (mørke seksjoner) */
  --color-white: #f5f2ed;  /* Bakgrunn (lyse seksjoner) */
}
```

---

## ➕ Legg til en ny side

1. Kopier `pages/_template.html`
2. Gi den nytt navn (f.eks. `pages/galleri.html`)
3. Endre `<title>` og fyll inn innhold mellom nav og footer
4. Legg til en lenke i nav-blokken i **alle** HTML-filer

---

## 📝 Stiler per fil

| Fil | Innhold |
|---|---|
| `variables.css` | Farger, fonter, spacing — **start her** |
| `base.css` | CSS reset, fade-up animasjon, section-header |
| `nav-footer.css` | Navigasjon, hamburger-meny, footer, marquee |
| `hero.css` | Forsidehero, innersidehero, knapper |
| `components.css` | Tjenestekort, om-oss, stats, priser, kartstrip |
| `forms.css` | Kontaktskjema, input, select, textarea |

---

## 📞 Bedriftsinformasjon

| | |
|---|---|
| **Navn** | Sparekopi AS |
| **Adresse** | Torggata 17B, 2. etasje, 0183 Oslo |
| **Telefon** | 47 29 34 43 |
| **Mobil** | 472 93 443 |
| **Org.nr** | 996 379 418 |
| **Åpningstider** | Man–Fre 09:00–17:00 |

---

*Nettsiden bruker Google Fonts (Bebas Neue, DM Mono, Playfair Display) og krever internettforbindelse for å laste fonter.*
