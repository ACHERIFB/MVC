using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ciqual.Models
{
    public partial class Aliment : IValidatableObject
    {
        public Aliment()
        {
            Composition = new HashSet<NbrConsti>();
        }
        [Required]
        [Range(1, 99999,ErrorMessage = "L'id doit etre compris entre 1 et 99999")]
        public int IdAliment { get; set; }
        [Required,MaxLength(150)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Required]
        public int IdFamille { get; set; }

        public virtual Famille IdFamilleNavigation { get; set; }
        public virtual ICollection<NbrConsti> Composition { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CiqualContext ctx = (CiqualContext)validationContext.GetService(typeof(CiqualContext));

            Aliment aliment = (Aliment)validationContext.ObjectInstance;
            var listeid = ctx.Aliment.ToList();
            foreach (var item in listeid)
            {
                if (aliment.IdAliment == item.IdAliment )
                {
                    yield return new ValidationResult("l'id existe déja");
                    yield return new ValidationResult("*", new string[] {"IdAliment" });
                }
                if (aliment.Nom == item.Nom)
                {
                    yield return new ValidationResult("Le nom existe déja");
                    yield return new ValidationResult("*", new string[] { "Nom"});
                }

            }

        }
    }
}
