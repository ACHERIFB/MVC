using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciqual.Models
{
    public class FamillesAlim
    {


        public List<AlimentsConsti> AlimentsConsti { get; set; }

        public List<Famille> Familles { get; set; }


    }
}
