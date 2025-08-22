
using SQLite;

namespace Pseven.Maui.Models;
[Table("DettaglioDocumento")]
public class DettaglioDocumento
{
    [PrimaryKey, AutoIncrement]           // Se la tua PK ha altro nome, metti [PrimaryKey, Column("...")]
    public int Id { get; set; }

    public string? Articoli { get; set; }
    public string? UM { get; set; }
    public double? Quantita { get; set; }
    public double? Prezzo { get; set; }
    public double? Sconto { get; set; }
    public double? Importo { get; set; }
    public int? Iva { get; set; }
    public DateTime? Data { get; set; }
    public int? IdElencoDoc { get; set; }
    public double? L { get; set; }
    public double? H { get; set; }
    public string? NOTE { get; set; }
    public string? Rif { get; set; }
    public string? Comandi { get; set; }
    public string? Attacchi { get; set; }
    public string? Hc { get; set; }
    public string? Guide { get; set; }
    public int? Pezzi { get; set; }
    public string? Colore { get; set; }
    public string? Varie1 { get; set; }
    public string? OrdPrev { get; set; }
    public string? Agente { get; set; }
    public string? Luce_finita { get; set; }
    public string? Varie2 { get; set; }
    public string? Supplemento1 { get; set; }
    public double? H2vvD { get; set; }
    public double? L2vv { get; set; }
    public string? Varie3 { get; set; }
    public string? Supplemento2 { get; set; }
    public double? ProvvEuro { get; set; } // attenzione: se è float, metti double?
    public double? ProvvPerc { get; set; }
    public string? Ragionesociale1riga { get; set; }
    public string? L_effettiva { get; set; }
    public string? H_effettiva { get; set; }
    public string? ColoreSupplemento1 { get; set; }
    public string? Versione { get; set; }
    public int? Listino { get; set; }
    public DateTime? Modificato { get; set; }
    public string? CODICE { get; set; }
}
