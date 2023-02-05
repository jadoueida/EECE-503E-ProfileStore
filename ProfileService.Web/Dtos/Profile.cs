namespace ProfileService.Web.Dtos;

public record Profile
{
    public Profile(string username, string firstName, string lastName)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }
    
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
}
    
public record PutProfileRequest
{
    public PutProfileRequest(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }
}