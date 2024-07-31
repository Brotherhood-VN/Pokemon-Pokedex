namespace API.Dtos.Systems
{
    public class PokemonClassDto
    {
        public long ClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassTitle { get; set; }
        public string Icon { get; set; }
        public bool IsDefault { get; set; }

    }
}