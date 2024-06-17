using System.ComponentModel.DataAnnotations;

namespace FatecSisMed.Web.Models
{
    public class RemedioViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        

        [Required]
        [Display(Name = "Marca")]
        public int MarcaId { get; set; }
        [Display(Name = "Marca")]
        public string? MarcaNome { get; set; }
    }
}
