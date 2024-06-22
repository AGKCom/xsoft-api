namespace xsoft.Dtos.Configuration
{
    public class ConfigurationDto
    {
        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public int MaxUsers { get; set; }
        public DateTime experationDate { get; set; }

    }
}
