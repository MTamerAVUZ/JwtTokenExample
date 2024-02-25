using ffs102_JwtTokenExam.Context;
using ffs102_JwtTokenExam.Dto;
using ffs102_JwtTokenExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace ffs102_JwtTokenExam.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HomeController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _dataContext;

    public HomeController(DatabaseContext dataContext)
    {
      _dataContext = dataContext;
    }

    public HomeController(IConfiguration configuration) {
    _configuration = configuration;
    }

    public IActionResult Index()
    {
      return Ok();
    }

    [HttpGet]
    public IActionResult GetJwtToken()
    {
      TokenModel jwtTokenModel = JwtTokenHandler.TokenOlustur(_configuration);     

      return Ok(jwtTokenModel);
    }

    public void GetJwtToken(User user)
    {
      TokenModel jwtTokenModel = JwtTokenHandler.TokenOlustur(_configuration);


      user.TokenTimeout = jwtTokenModel.Expr;
      user.Token = jwtTokenModel.AccessToken;

      _dataContext.Update(user);
      _dataContext.SaveChanges();      
    }


    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
      if (user is null)
      {
        return NotFound();
      }

      var data = _dataContext.User.FirstOrDefault(t=> t.Username == user.Username && t.Password==user.Password);

      if (data == null)
      {
        ModelState.AddModelError(nameof(user.Password), "Kullanıcı adı veya şifre hatalı");
        return NotFound();
      }

      GetJwtToken(user);
      //return işleminde oluşturulan Token ve datetime geri dönülür.

      var userDto = new UserDto();
      userDto.Expr = user.TokenTimeout;
      userDto.AccessToken = user.Token;

      return Ok(userDto);
    }
    /*
     api: 
     -kullanıcı kontrolü :kullanıcı kodu ve şifre db den kontrol edilir. doğru ise müşteirye jwt token dönülür ve bu JwtToken bilgisi Db de saklanır. 
    -kullanıcı liste dönen fonksyon :müşteriden token bekliyoruz. geken token db deki tabloda kontrol edilir, eşleşiyorsa cevap verilir, eşleşmşyorsa cevap verilmez. 


     
     
     MVC (müşteri): 
    -kullanıcı kontrol : bu fonksyon içinden Api çağrımı yapılır. Api ye kullanıcı kodu ve şifre gönderilir. eğer token dönüyorsa db de saklanır.

-kullanıcı listesini ver: bu fonksyon sadece JwtToken ile çağrılır. Gelen data ekrana yazılır.  
     */

    

    public IActionResult KullanicilariListele(TokenModel tokenModel)
    {
      var data = _dataContext.User.FirstOrDefault(t => t.Token == tokenModel.AccessToken);

      if (data == null)
      {
        return NotFound();
      }

      return RedirectToAction(nameof(Index), "Home");
    }

  }
}
