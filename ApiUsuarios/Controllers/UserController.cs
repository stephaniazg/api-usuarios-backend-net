using ApiUsuarios.Interfaz;
using ApiUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ApiUsuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserScoreService _userScoreService;
        private readonly IEncryptionService _encryptionService;
        private readonly IClassificationService _classificationService;




        public UserController(IUserRepository userRepository, IUserScoreService userScoreService, IEncryptionService encryptionService, IClassificationService classificationService)
        {
            _userRepository = userRepository;
            _userScoreService = userScoreService;
            _encryptionService = encryptionService;
            _classificationService = classificationService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                return Ok(new
                {
                    status = true,
                    message = "Usuarios consultados",
                    data = users,
                    code = 200
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al consultar usuarios",
                    data = ex.Message,
                    code = 500
                });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Usuario no encontrado",
                        data = (object)null,
                        code = 404
                    });
                }
                return Ok(new
                {
                    status = true,
                    message = "Usuario consultado",
                    data = user,
                    code = 200
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al consultar el usuario",
                    data = ex.Message,
                    code = 500
                });
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Modelo inválido",
                    data = (object)null,
                    code = 400
                });
            }

            try
            {
                //user.Contrasena = _encryptionService.Encrypt(user.Contrasena);
                user.Puntaje = _userScoreService.CalculateScore(user.Nombre, user.Apellidos, user.CorreoElectronico);

                await _userRepository.CreateUserAsync(user);

                // Se asume que MongoDB asigna un Id automáticamente
                if (string.IsNullOrEmpty(user.Id))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        status = false,
                        message = "Error al crear el usuario",
                        data = (object)null,
                        code = 500
                    });
                }

                return Ok(new
                {
                    status = true,
                    message = "usuario creado",
                    data = user,
                    code = 200
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al crear el usuario",
                    data = ex.Message,
                    code = 500
                });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Modelo inválido",
                    data = (object)null,
                    code = 400
                });
            }

            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Usuario no encontrado",
                        data = (object)null,
                        code = 404
                    });
                }

                // Aseguramos que el id se mantenga igual
                user.Id = existingUser.Id;
                user.Puntaje = _userScoreService.CalculateScore(user.Nombre, user.Apellidos, user.CorreoElectronico);

                //if (!string.IsNullOrEmpty(user.Contrasena))
                //{
                //    existingUser.Contrasena = _encryptionService.Encrypt(user.Contrasena);
                //} else
                //{
                //    user.Contrasena = user.Contrasena;
                //}

                    bool updated = await _userRepository.UpdateUserAsync(id, user);
                if (!updated)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        status = false,
                        message = "Error al actualizar el usuario",
                        data = (object)null,
                        code = 500
                    });
                }

                // Retornamos el usuario actualizado
                var updatedUser = await _userRepository.GetUserByIdAsync(id);
                return Ok(new
                {
                    status = true,
                    message = "Usuario actualizado",
                    data = updatedUser,
                    code = 200
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al actualizar el usuario",
                    data = ex.Message,
                    code = 500
                });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Usuario no encontrado",
                        data = (object)null,
                        code = 404
                    });
                }

                bool deleted = await _userRepository.DeleteUserAsync(id);
                if (!deleted)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        status = false,
                        message = "Error al eliminar el usuario",
                        data = (object)null,
                        code = 500
                    });
                }

                return Ok(new
                {
                    status = true,
                    message = "Usuario eliminado",
                    data = (object)null,
                    code = 200
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al eliminar el usuario",
                    data = ex.Message,
                    code = 500
                });
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Modelo inválido",
                    data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    code = 400
                });
            }

            try
            {
                // Se asume que el front envía la contraseña ya encriptada.
                var users = await _userRepository.GetUsersAsync();
                var user = users.FirstOrDefault(u =>
                    u.CorreoElectronico.Equals(loginModel.CorreoElectronico, StringComparison.OrdinalIgnoreCase) &&
                    u.Contrasena == loginModel.Contrasena);

                if (user == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Credenciales inválidas",
                        data = (object)null,
                        code = 404
                    });
                }

                // Calcular la clasificación basada en la nueva fecha de acceso
                user.Clasificacion = _classificationService.GetClassification(user.FechaUltimoAcceso);
                
                // Actualizar FechaUltimoAcceso con la fecha y hora actual
                user.FechaUltimoAcceso = DateTime.UtcNow;

                // Actualiza el usuario en la base de datos
                bool updated = await _userRepository.UpdateUserAsync(user.Id, user);
                if (!updated)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        status = false,
                        message = "Error al actualizar FechaUltimoAcceso y Clasificacion",
                        data = (object)null,
                        code = 500
                    });
                }

                return Ok(new
                {
                    status = true,
                    message = "Inicio de sesión exitoso",
                    data = user,
                    code = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = "Error al iniciar sesión",
                    data = ex.Message,
                    code = 500
                });
            }
        }
    }
}
