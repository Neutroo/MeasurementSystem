using MeasurementSystem.Server.Dto;
using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = userRepository.Select();
            return Ok(users);
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(POSTUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = userDto.ToDomain();
                userRepository.Insert(user);
                userRepository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">Guid пользователя</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            userRepository.Delete(id);
            userRepository.Save();
            return Ok();
        }
    }
}
