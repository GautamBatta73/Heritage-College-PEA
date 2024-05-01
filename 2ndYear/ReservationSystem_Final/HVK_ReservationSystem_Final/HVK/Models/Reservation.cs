using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HVK.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            StartDate = DateTime.Today.Date;
            EndDate = DateTime.Today.Date;
            PetReservations = new HashSet<PetReservation>();
            ReservationDiscounts = new HashSet<ReservationDiscount>();
        }

        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Please enter the Start Date.")]
        [CustomValidation(typeof(Reservation), nameof(ValidateStartDate))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the End Date.")]
        [CustomValidation(typeof(Reservation), nameof(ValidateEndDate))]
        public DateTime EndDate { get; set; }

        public int Status { get; set; }

        public virtual ICollection<PetReservation> PetReservations { get; set; }
        public virtual ICollection<ReservationDiscount> ReservationDiscounts { get; set; }

        // Custom validation method
        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Reservation;
            if (instance == null) return ValidationResult.Success;

            if (endDate < instance.StartDate)
            {
                return new ValidationResult("End date must be after start date.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Reservation;
            if (instance == null) return ValidationResult.Success;

            if (startDate < DateTime.Today.Date)
            {
                return new ValidationResult("You can't make a reservation starting before today.");
            }
            return ValidationResult.Success;
        }
    }
}
