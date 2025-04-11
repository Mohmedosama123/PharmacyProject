using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmactMangmentDAL.Models
{
    public class Med_Phar
    {
        public int MedicationId { get; set; }
        public int PharmacyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "يجب أن تكون الكمية 1 على الأقل.")]
        public int Quantity { get; set; }
        public bool InStock { get; set; }

       
        public Pharmacy pharmacy { get; set; }
        public Medication medican { get; set; }


    }
}
