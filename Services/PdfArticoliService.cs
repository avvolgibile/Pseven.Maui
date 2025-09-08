using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestContainer = QuestPDF.Infrastructure.IContainer;
using Pseven.Models;    

namespace Pseven.Services;

public static class PdfArticoliService
{
    // <-- CAMBIA QUI i nomi delle proprietà secondo il tuo modello StoricoArticoli
    // Header = titolo colonna nel PDF
    // Prop   = nome della proprietà nel Model (rispetta maiuscole/minuscole)
    private static readonly (string Header, string Prop, float? ConstWidth, int Relative, string? Format)[] ColSpec =
    {
        ("Codice",      "Codice",      70, 0,  null),
        ("Descrizione", "Descrizione", null, 3, null),    // colonna elastica
        ("Q.tà",        "Quantita",    45, 0,  "0.##"),
        ("Prezzo",      "Prezzo",      55, 0,  "0.00"),
        // Aggiungi/rimuovi righe secondo le colonne che vuoi nel PDF
        // ("L", "L", 45, 0, "0.##"),
        // ("H", "H", 45, 0, "0.##"),
    };

    public static byte[] GeneraPdfDallaLista(List<StoricoArticolo> articoli)
    {
        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(16);
                page.DefaultTextStyle(x => x.FontSize(8));

                page.Content().Table(table =>
                {
                    // Definizione colonne: numeriche strette (Constant), testo elastico (Relative)
                    table.ColumnsDefinition(cols =>
                    {
                        foreach (var c in ColSpec)
                        {
                            if (c.ConstWidth.HasValue)
                                cols.ConstantColumn(c.ConstWidth.Value);
                            else
                                cols.RelativeColumn(c.Relative > 0 ? c.Relative : 1);
                        }
                    });

                    QuestContainer Cell(QuestContainer c) => c
                        .Padding(1)
                        .Border(0.25f)
                        .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                        .MinWidth(10);

                    // Header
                    table.Header(h =>
                    {
                        foreach (var c in ColSpec)
                            h.Cell().Element(Cell).Text(c.Header);
                    });

                    // Righe
                    var t = typeof(StoricoArticolo);
                    foreach (var a in articoli)
                    {
                        foreach (var c in ColSpec)
                        {
                            var prop = t.GetProperty(c.Prop);
                            var raw = prop?.GetValue(a);

                            // formattazione basica numeri/date
                            string value = raw switch
                            {
                                null => string.Empty,
                                IFormattable f when !string.IsNullOrEmpty(c.Format) => f.ToString(c.Format, null),
                                DateTime dt => dt.ToString("dd/MM/yyyy"),
                                _ => raw.ToString() ?? string.Empty
                            };

                            table.Cell().Element(Cell).Text(tw =>
                            {
                                tw.Span(value).WrapAnywhere();
                                // se vuoi troncare a 1 riga: .MaxLines(1).Ellipsis();
                            });
                        }
                    }
                });
            });
        })
        .GeneratePdf();
    }
}