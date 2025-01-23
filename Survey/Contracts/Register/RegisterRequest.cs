namespace Survey.Contracts.Register
{
    public record registerRequest
        (
        string Email , 
        string Password,
        string FirstName,
        string LastName
        );
}
