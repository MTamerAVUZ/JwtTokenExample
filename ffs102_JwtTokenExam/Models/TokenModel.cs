namespace ffs102_JwtTokenExam.Models
{
  public class TokenModel
  {
        public string AccessToken { get; set; }
        public DateTime Expr { get; set; }
        public string RefreshToken { get; set; }

    }
}
