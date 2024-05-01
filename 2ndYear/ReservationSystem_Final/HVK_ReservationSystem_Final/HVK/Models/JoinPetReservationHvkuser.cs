namespace HVK.Models
{
    public class JoinPetReservationHvkuser
    {
        public List<Pet> Pets { get; set; }
        public Reservation Reservation { get; set; }
        public Hvkuser HvkUser { get; set; }
    }
}
