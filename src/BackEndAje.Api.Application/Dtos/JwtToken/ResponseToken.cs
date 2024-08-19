namespace BackEndAje.Api.Application.Dtos.JwtToken
{
    public class ResponseToken
    {
        public string Token { get; set; }

        public long TokenExpiresSeg { get; set; }
    }
}
