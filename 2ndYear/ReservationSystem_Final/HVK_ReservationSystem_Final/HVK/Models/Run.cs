using System;
using System.Collections.Generic;

namespace HVK.Models
{
    public partial class Run
    {
        public Run()
        {
            PetReservations = new HashSet<PetReservation>();
        }

        public enum RunStates
        {
            Vacant = 1,
            Occupied,
            ToClean
        }

        public int RunId { get; set; }
        public string Size { get; set; } = null!;
        public bool Covered { get; set; }
        public string? Location { get; set; }
        public decimal? Status { get; set; }

        public virtual ICollection<PetReservation> PetReservations { get; set; }
    }
}
