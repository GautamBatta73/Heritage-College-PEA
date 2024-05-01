using System;
using System.Collections.Generic;

namespace HVK.Models
{
    public partial class PetVaccination
    {
        public DateTime ExpiryDate { get; set; }
        public int VaccinationId { get; set; }
        public int PetId { get; set; }
        public bool VaccinationChecked { get; set; }

        public virtual Pet Pet { get; set; } = null!;
        public virtual Vaccination Vaccination { get; set; } = null!;
    }
}
