using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PN.Firestore.Repositories;
using PN.Models;

namespace PN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public FirestoreController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            var all = await _userRepository.GetAllAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetAsync(id);
            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var user = await _userRepository.AddAsync(new Firestore.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            });
            return Ok(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user is null)
            {
                return Ok("User does not exist.");
            }

            await _userRepository.DeleteAsync(user);
            return Ok(user);
        }
    }
}
