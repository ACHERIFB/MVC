using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class StatistiquesViewModel
    {
        public int NbrTachesTerminee { get; set; }
        public int NbrTachesEnCours { get; set; }
        public int NbrTachesRetard { get; set; }
        public double DelaiMoyen { get; set; }
    }
}
