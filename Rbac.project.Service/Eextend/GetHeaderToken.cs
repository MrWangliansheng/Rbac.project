using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rbac.project.IService.Eextend;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Rbac.project.Service.Eextend
{
    public class GetHeaderToken : IGetHeaderToken
    {
        private readonly IConfiguration configuration;
        public GetHeaderToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetHeader(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(token.Replace("Bearer ","")).Payload;
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Kestrel:key"]));
            var username = payload.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            
            return username;
        }
    }
}
