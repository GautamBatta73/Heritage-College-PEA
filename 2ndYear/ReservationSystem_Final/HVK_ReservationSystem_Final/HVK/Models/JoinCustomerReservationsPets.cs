namespace HVK.Models
{
    public class JoinCustomerReservationsPets
    {
        public Hvkuser Hvkuser { get; set; }
        public List<Pet> Pets { get; set; } = new List<Pet>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public Reservation Reservation { get; set; }
        public PetReservation PetReservation { get; set; }
        public Pet Pet { get; set; }

    }
}
