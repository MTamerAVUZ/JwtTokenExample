using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ffs102_JwtTokenExam.Models
{
  public class JwtTokenHandler
  {
    public static TokenModel TokenOlustur(IConfiguration configuration)
    {
      TokenModel tokenModel = new TokenModel();

      SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenParams:IssuerSingingKey"]));
      SigningCredentials credentials =new SigningCredentials(symmetricKey,SecurityAlgorithms.HmacSha256);

      tokenModel.Expr = DateTime.Now.AddMinutes(Convert.ToInt16(configuration["JwtTokenParams:Expr"]));

      //pointer işlemine başlıyoruz //işaretçi

      JwtSecurityToken jwtSecurityToken = new(
        signingCredentials:credentials,
        issuer: configuration["JwtTokenParams:ValidIssuer"],
        audience: configuration["JwtTokenParams:ValidAudience"],
        expires:tokenModel.Expr
        );

      JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
      tokenModel.AccessToken=jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

      /*Yukarıdaki bütün yapı bearerkey oluşturmak için.
       * Aşağıdaki kod bloğuda key içindeki değerlerin yerlerinin karıştırılması için */
      byte[] degerSeti=new byte[32];

      using RandomNumberGenerator randomNumberGenerator=RandomNumberGenerator.Create();

      randomNumberGenerator.GetBytes(degerSeti);
      tokenModel.RefreshToken=Convert.ToBase64String(degerSeti);// En son oluşturlmuş (karışık dizi) olduğumuz dizi bizim şifremiz (Token).

      return tokenModel;  
      

    }

  }
}
