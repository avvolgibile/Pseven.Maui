using SQLite;

namespace Pseven.Models
{
    [Table("ElencoTotale")]

    public class DocumentoAperto
    {
  
            [PrimaryKey, AutoIncrement]           // Se la tua PK ha altro nome, metti [PrimaryKey, Column("...")]
            public int Id { get; set; }
            public string? Ragionesociale1riga { get; set; }
            public string? Citta { get; set; }
            public double? Importo { get; set; }
            public DateTime? Data { get; set; }
            public string? Telefono { get; set; }
            public string? cell { get; set; }
            public string? NoteDocumento { get; set; }
            public string? NoteCliente { get; set; }
            public string? Indirizzo { get; set; }
            public string? Prov { get; set; }
            public string? OrdPrev { get; set; }
            public string? Agente { get; set; }
            public string? Email { get; set; }
            public string? Zona { get; set; }
            public double? ProvvEuro { get; set; }
            public double? ProvvPerc { get; set; }
            public string? OrdineSospeso { get; set; }
            public int? Listino { get; set; }
            public int? NumeroDocumento { get; set; }
            public int? Aperto { get; set; }
            public string? Ord_a_fornitore { get; set; }
        }
}
