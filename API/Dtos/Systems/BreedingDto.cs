namespace API.Dtos.Systems
{
    public class BreedingDto
    {
        public long Id { get; set; }
        public string EggGroup { get; set; }
        public string GenderDistribution { get; set; }
        public string EggCycle { get; set; }
        public long PokemonId { get; set; }

    }
}