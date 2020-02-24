using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class ValidationJoursAttribute : ValidationAttribute
    {
        private int _joursAjoutes;

        public ValidationJoursAttribute(int JoursAjoutes)
        {
            _joursAjoutes = JoursAjoutes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Calcul calcul = (Calcul)validationContext.ObjectInstance;

            if (calcul.JoursAjoutes > 9999 || calcul.JoursAjoutes < 1)
            {
                return new ValidationResult("Le jour doit etre compris entre 1 et  "+_joursAjoutes);
            }
            return ValidationResult.Success;
        }
    }


}

