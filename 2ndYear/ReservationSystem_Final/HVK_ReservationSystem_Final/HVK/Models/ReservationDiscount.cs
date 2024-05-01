using System;
using System.Collections.Generic;

namespace HVK.Models
{
    public partial class ReservationDiscount
    {
        public int DiscountId { get; set; }
        public int ReservationId { get; set; }
        public string? NullHelper { get; set; }

        public virtual Discount Discount { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
