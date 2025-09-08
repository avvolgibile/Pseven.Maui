using SQLite;

namespace Pseven.Models
{
    [Table("InvioFornit")]
   public class StoricoOrdineAFornitore
    {

        [PrimaryKey, AutoIncrement]           // Se la tua PK ha altro nome, metti [PrimaryKey, Column("...")]
        public int Id { get; set; }
        public string Email { get; set; }
        public string? TextOrd { get; set; }
        public DateTime? Data { get; set; }
      

    }
    
    
       

 }
