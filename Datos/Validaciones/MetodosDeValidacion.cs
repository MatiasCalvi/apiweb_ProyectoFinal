using Configuracion;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using Datos.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Datos.Validaciones
{
    public class MetodosDeValidacion : IMetodosDeValidacion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDaoBDUsuarios _daoBDUsuarios;
        private readonly IDaoBDUsuarioAcceso _daoBDUsuarioAcceso;
        private JwtConfiguration _jwtConfiguration;

        public MetodosDeValidacion(IHttpContextAccessor httpContextAccessor, IDaoBDUsuarioAcceso daoBDUsuarioAcceso, IOptions<JwtConfiguration> jwtConfiguration, IDaoBDUsuarios daoBDUsuarios)
        {
            _httpContextAccessor = httpContextAccessor;
            _daoBDUsuarios = daoBDUsuarios;
            _daoBDUsuarioAcceso = daoBDUsuarioAcceso;
            _jwtConfiguration = jwtConfiguration.Value;
        }


        public async Task<string> HashContra(string pContra)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pContra.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string contraHasheada = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return BCrypt.Net.BCrypt.HashPassword(contraHasheada, 4);
        }

        public async Task <bool> VerificarContra(string pUsuario, string pContraHasheada)
        {

            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pUsuario.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string contraHasheada = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return BCrypt.Net.BCrypt.Verify(contraHasheada, pContraHasheada);
        }

        public async Task<bool> VerificarUsuario(string pEmail, string pContra)
        {
            try
            {
                UsuarioModif usuario = await _daoBDUsuarios.ObtenerUsuarioPorEmailU(pEmail);
                
                if (usuario == null) return false;    
               
                bool passwordMatch = await VerificarContra(pContra, usuario.Usuario_Contra);
                
                if (!passwordMatch) return false;

                return true;
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException("Se produjo un error al verificar la contraseña", ex);
            }
        }

        public async Task<string> ObtenerRefreshToken(int pUsuarioId)
        {
            try
            {
                return await _daoBDUsuarioAcceso.ObtenerRefreshToken(pUsuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el token de actualización del servicio.", ex);
            }
        }

        public async Task<int> ObtenerUsuarioIDToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid);
            int userId;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
            {
                return userId;
            }
            throw new ApplicationException("No se pudo extraer Usuario_Id del token.");
        }

        public async Task<string> ObtenerUsuarioEstadoToken()
        {
            var usuarioEstadoClaim = _httpContextAccessor.HttpContext.User.FindFirst("estado");
            string usuarioEstado;

            if (usuarioEstadoClaim != null)
            {
                usuarioEstado = usuarioEstadoClaim.Value;
                return usuarioEstado;
            }

            throw new ApplicationException("No se pudo extraer Usuario_Estado del token.");
        }

        public async Task<int> ObtenerUsuarioIDRefreshToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(refreshToken) as JwtSecurityToken;

            if (jsonToken != null)
            {
                var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            throw new ApplicationException("No se pudo extraer el Id del usuario del refresh token.");
        }

        public async Task<string> ObtenerUsuarioRoleToken()
        {
            var userRoleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userRoleClaim))
            {
                return userRoleClaim;
            }

            throw new ApplicationException("No se pudo extraer el reclamo de rol del token.");
        }

        public string GenerarTokenAcceso(UsuarioSalida pUsuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, pUsuario.Usuario_ID.ToString()),
                    new Claim(ClaimTypes.Role, pUsuario.Usuario_Role.ToString()),
                    new Claim(ClaimTypes.Name, pUsuario.Usuario_Nombre),
                    new Claim(ClaimTypes.Email, pUsuario.Usuario_Email),
                    new Claim("estado", pUsuario.Usuario_Estado)
                };
            var now = DateTime.Now;
            var expiration = now.AddMinutes(4);

            var sectoken = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = now,
                NotBefore = now,
                Expires = expiration,
                SigningCredentials = credentials,
            });

            return new JwtSecurityTokenHandler().WriteToken(sectoken);
        }

        private string GenerarRefreshToken(int pUserId, string pUsuarioRol)
        {
            var refreshTokenSecret = Guid.NewGuid().ToString();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, pUserId.ToString()),
                new Claim(ClaimTypes.Role, pUsuarioRol),
            };

            var now = DateTime.Now;
            var expiration = now.AddHours(10);

            var refreshToken = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: now,
                expires: expiration,
                signingCredentials: credentials
            );

            var refreshTokenHandler = new JwtSecurityTokenHandler();
            return refreshTokenHandler.WriteToken(refreshToken);
        }

        public async Task<string> GenerarYGuardarRefreshToken(int pUsuarioId, string pUsuarioRole)
        {
            string refreshToken = GenerarRefreshToken(pUsuarioId, pUsuarioRole);
            await _daoBDUsuarioAcceso.GuardarRefreshToken(pUsuarioId, refreshToken);

            EstablecerCookie(refreshToken);

            return refreshToken;
        }
        public async Task EliminarRefreshToken(int pUsuarioId)
        {
            try
            {
                await _daoBDUsuarioAcceso.EliminarRefreshToken(pUsuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el token de actualización.", ex);
            }
        }

        public async Task<bool> EsRefreshTokenValido(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
            var expiration = jwtToken.ValidTo;

            if (expiration < DateTime.Now) return false;
            
            return true;     
        }

        private void EstablecerCookie(string pRefreshToken)
        {
            var cookieOptions = ConstruirCookieOptions();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", pRefreshToken, cookieOptions);
        }

        private CookieOptions ConstruirCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(10)
            };
        }

        public void EliminarCookie(string pNombreCookie)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(-1)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(pNombreCookie, "", cookieOptions);
        }

        public void ActualizarCookie(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(refreshToken);
            var expiration = token.ValidTo;

            var cookie = new CookieOptions();
            cookie.Expires = expiration;

            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookie);
        }

    }
}
