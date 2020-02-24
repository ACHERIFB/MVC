using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class Tache : IValidatableObject
    {
        [Display(Name = "l'id de la tache")]
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Date de creation")]
        [DataType(DataType.Date)]
        public DateTime DateCreation { get; set; }
        [Display(Name = "Date d'echeance")]
        [DataType(DataType.Date)]
        public  DateTime? DateEcheance { get; set; }
        [Display(Name = "Cloture de la tache")]
        public bool Terminee { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Tache tache = (Tache)validationContext.ObjectInstance;

            if (tache.DateEcheance < tache.DateCreation)
            {
                yield return new ValidationResult("La date d’échéance est supérieure à la date de création de la tâche");
                yield return new ValidationResult("*", new string[] { "DateEcheance", "DateCreation" });
            }

        }





    }
}
