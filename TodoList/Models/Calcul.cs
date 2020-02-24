using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public enum selectoperation
    {
    plus,
    moins
    }
    public class Calcul
    {
        [DataType(DataType.Date)]
        public DateTime DateInitiale { get; set; }
        //[Range(1,9999)]
        [ValidationJours(9999)]
        public int JoursAjoutes { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateResultat { get; set; }
        public selectoperation Operation { get; set; }
    }
}
