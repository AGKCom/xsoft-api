namespace xsoft.models
{
    public class Authentication
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string identityJson { get; set; } //email,role serilized as json
    }
}
