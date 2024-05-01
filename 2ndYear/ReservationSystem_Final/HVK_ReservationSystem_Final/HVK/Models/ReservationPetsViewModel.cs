namespace HVK.Models
{
    public class ReservationPetsViewModel
    {
        public Reservation Reservation { get; set; }
        public List<int> SelectedPetIds { get; set; }
        public Dictionary<int, List<PetVaccination>> PetVaccinations { get; set; }
        public List<Pet> Pets { get; set; }
    }


}
