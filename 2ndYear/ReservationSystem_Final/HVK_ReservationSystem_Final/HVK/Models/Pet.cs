using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HVK.Models
{
    public partial class Pet
    {
        public Pet()
        {
            PetReservations = new HashSet<PetReservation>();
            PetVaccinations = new HashSet<PetVaccination>();
        }

        public int PetId { get; set; }

        [RegularExpression("^[A-Za-z\\'\\-\\s]+$", ErrorMessage = "The Dog Name May Only Contain Letters, Apostrophees, and Hyphen.")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "The Pet's Name Must Be in Between 1 and 25 Characters.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; } = null!;

        [DataType(DataType.Text)]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "The Gender Can Only Be F For Female or M For Male.")]
        [RegularExpression("^([Mm]|[Ff]){1}$", ErrorMessage = "The Gender Must Be A \"F\" For Female Or A \"M\" For Male.)")]
        [Required]
        public string Gender { get; set; } = null!;
 
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Breed Must Be In Between 1 and 50 Characters.")]
        [RegularExpression("^[A-Za-z\\'\\-]+$", ErrorMessage = "The Dog Breed Can Only Contain Letters (Uppercase case or Lowercase), Hyphen and Apostrophees.")]
        [DataType(DataType.Text)]
        public string? Breed { get; set; }

        [DisplayName("Birth Year")]
        [Range(1994, 2024)]
        public int? Birthyear { get; set; }
        
        public int HvkuserId { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "The Gender May Not Contain More Than One Character.")]
        [RegularExpression("^([Ss]|[Mm]|[Ll]){1}$", ErrorMessage = "The Dog Size Must Be \"S\" For Small Pets, or \"M\" For Medium Pets, or \"L\" For Large Pets.)")]
        [DisplayName("Dog Size")]
        public string? DogSize { get; set; }

        public bool Climber { get; set; }

        public bool Barker { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Special Notes")]
        //[RegularExpression("^[A-Za-z\\-\\?\\.\\'\\,\\*\\!]*$", ErrorMessage = "The Comments May Only Contain Letters, Dots, Question Marks, Exclamation Point, Commas, Hyphen and Apostrophees, Numbers,  And Asterisk .")]
        [MaxLength(200, ErrorMessage = "The Special Notes Cannot Exceed 200 Characters")]
        public string? SpecialNotes { get; set; }

        public bool Sterilized { get; set; }

        public virtual Hvkuser Hvkuser { get; set; } = null!;
        public virtual ICollection<PetReservation> PetReservations { get; set; }
        public virtual ICollection<PetVaccination> PetVaccinations { get; set; }
    }
}
