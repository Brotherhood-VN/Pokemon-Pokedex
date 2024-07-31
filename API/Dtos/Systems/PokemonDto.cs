namespace API.Dtos.Systems
{
    public class PokemonDto
    {
        public long Id { get; set; }
        public string Index { get; set; }
        public string FullName { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public string Note { get; set; }
        public string BeforeIndex { get; set; }
        public long? RankId { get; set; }
        public long? ConditionId { get; set; }
        public int? Level { get; set; }
        public long? StoneId { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }

        public AgainstDto Against { get; set; }
        public BreedingDto Breeding { get; set; }
        public FormDto Form { get; set; }
        public StatDto Stat { get; set; }
        public TrainingDto Training { get; set; }
        public List<PokemonAbilityDto> PokemonAbilities { get; set; }
        public List<PokemonGameVersionDto> PokemonGameVersions { get; set; }
        public List<PokemonGenDto> PokemonGens { get; set; }
        public List<PokemonClassDto> PokemonClasses { get; set; }
        public List<PokemonSkillDto> PokemonSkills { get; set; }
    }
}