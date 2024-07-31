namespace API.Dtos.Systems
{
    public class StatDto
    {
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long? AreaId { get; set; }
        public long? RegionId { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Speed { get; set; }
        public int SpeedAttack { get; set; }
        public int SpeedDefence { get; set; }
        public long StatTypeId { get; set; }

        public string AreaCode { get; set; }
        public string AreaTitle { get; set; }
        public string RegionCode { get; set; }
        public string RegionTitle { get; set; }
        public string StatTypeCode { get; set; }
        public string StatTypeTitle { get; set; }

    }
}