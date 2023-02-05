namespace ProfileService.Web.Dtos;

public record Profile
{
    public Profile(string username, string firstName, string lastName)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }
    
        public string Username;
        public string FirstName;
        public string LastName;
}
    
public record PutProfileRequest
{
    public PutProfileRequest(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName;
    public string LastName;
}