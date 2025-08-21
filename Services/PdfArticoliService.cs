using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pseven.Models;

namespace Pseven.Services
{
    public static class PdfArticoliService
    {

public static byte[] GeneraPdfA4DallaLista(List<StoricoArticolo> articoli)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.Content().Table(table =>
                {
                    // Recupera tutte le proprietà pubbliche della classe StoricoArticolo
                    var properties = typeof(StoricoArticolo).GetProperties();/////

                    // Definizione delle colonne
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in properties)
                            columns.RelativeColumn();
                    });

                    // Intestazione della tabella
                    table.Header(header =>
                    {
                        foreach (var prop in properties)
                        {
                            header.Cell().Element(container =>
                                container
                                    .Padding(3)
                                    .Border(1)
                                    .BorderColor(QuestPDF.Helpers.Colors.Black)
                                    .Shrink() // lo mettiamo prima del .Text()
                                    .Text(prop.Name).Bold().FontSize(8)
                            );
                        }
                    });

                    // Righe dati
                    foreach (var articolo in articoli)
                    {
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(articolo)?.ToString() ?? string.Empty;

                            table.Cell().Element(container =>
                                container
                                    .Padding(3)
                                    .Border(1)
                                    .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                                    .Shrink()
                                    .Text(value).FontSize(8)
                            );
                        }
                    }
                });
            });
        });

        return document.GeneratePdf();
    }
    public static async Task MostraAnteprimaPdfAsync(List<StoricoArticolo> articoli)
        {
            try
            {
                var pdfBytes = GeneraPdfA4DallaLista(articoli);
                var filePath = Path.Combine(FileSystem.CacheDirectory, "AnteprimaArticoli.pdf");
                File.WriteAllBytes(filePath, pdfBytes);

                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Errore PDF", ex.Message, "OK");
            }
        }

    }
}