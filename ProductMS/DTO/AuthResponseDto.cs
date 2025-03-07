namespace ProductMS.DTO
{
    public class AuthResponseDto
    {
        public bool IAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
       // public bool IsTokenRevoked { get; set; }


    }
}
