using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ciqual.Models
{
    public partial class Famille
    {
        public Famille()
        {
            Aliment = new HashSet<Aliment>();
        }
        [Display(Name = "Id")]
        public int IdFamille { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        public virtual ICollection<Aliment> Aliment { get; set; }
    }
}
