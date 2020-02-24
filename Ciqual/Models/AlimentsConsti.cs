using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciqual.Models
{
    public class AlimentsConsti
    {
        public AlimentsConsti(int idAliment, string nom, int idFamille, int nbrConsti)
        {
            IdAliment = idAliment;
            Nom = nom;
            IdFamille = idFamille;
            NbrConsti = nbrConsti;
        }

        [Display(Name = "Id")]
        public int IdAliment { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        public int IdFamille { get; set; }
        [Display(Name = "Constituants")]
        public int NbrConsti { get; set; }

       // public virtual Famille IdFamilleNavigation { get; set; }
        //public virtual ICollection<Constituant> NbrConsti { get; set; }
    }
}
