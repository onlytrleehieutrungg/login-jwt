using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Models;
using DataAccess.Repository.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LoginApplication.Controllers;

public class MemberController : ControllerBase
{
    private readonly IMemberRepository _memberRepository;

    public MemberController(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginModel loginModel )
    {
        try
        {
            if (string.IsNullOrEmpty(loginModel.Email) ||
                string.IsNullOrEmpty(loginModel.Password))
                return BadRequest("Email and/or Password not specified");
            bool isLogin = _memberRepository.Login(loginModel.Email, loginModel.Password);
            if (isLogin)
            {
                var tokenString = GenerateJwtToken(loginModel.Email);
                return Ok(tokenString);
            }
        }
        catch (Exception e)
        {
            BadRequest(e.Message);
        }

        return Unauthorized();
    }
    
    string GenerateJwtToken(string email)
    {
  
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("tranlehieutrungnguyenthienquangphamngocanh"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var permClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),

        };

        // if (roles != null && roles.Length > 0)
        // {
        //     permClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        // }

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(permClaims),
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(60)),
            SigningCredentials = credentials
        };
        var token = jwtSecurityTokenHandler.CreateToken(tokenDescription);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}