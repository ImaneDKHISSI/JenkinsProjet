using System.ComponentModel.DataAnnotations;

namespace ProjetJenkins.ViewModels
{
    public class ClientUpdateVM
    {
        public int Id {  get; set; }
        [Required(ErrorMessage = "Le Nom est Obligatoire")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Le Prenom est Obligatoire")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Le Tel est Obligatoire")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "Le CIN est Obligatoire")]
        public string CIN { get; set; }
    }
}
