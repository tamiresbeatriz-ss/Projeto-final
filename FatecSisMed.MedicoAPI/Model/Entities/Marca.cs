namespace FatecSisMed.MedicoAPI.Model.Entities
{
    public class Marca
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public ICollection<Remedio>? Remedios { get; set; }
    }
}
