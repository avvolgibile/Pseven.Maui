using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Models;

public class Etichetta15CGPT
{
    public double Larghezza { get; set; } = 330;
    public double Altezza { get; set; } = 135;

    public string Alias { get; set; } = "";
    public string Colore { get; set; } = "";
    public string? Comandi { get; set; }
    public string? Attacchi { get; set; }
    public string? PiuGuide { get; set; }

    public int H { get; set; }
    public int LuceLPerEtichetta { get; set; }
    public int LuceHPerEtichetta { get; set; }

    public double CalcoloFloat { get; set; }
    public double[] CordiniVeneziane { get; set; } = Array.Empty<double>();

    public string MisuraPrimaStazione { get; set; } = "m";
    public string MisuraSecondaStazione { get; set; } = "m";
    public string MisuraForoGuida { get; set; } = "m";

    public string? Supplemento1 { get; set; }
    public string? Note { get; set; }
    public string? Rif { get; set; }
    public bool EvidenziaTS { get; set; }
    public bool PiuGuideFlag { get; set; }   // se ti serve anche come bool
}

