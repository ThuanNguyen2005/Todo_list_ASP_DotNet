using Todo_list.Repositories;
using Todo_list.Models;

namespace Todo_list.Services
{
    public class AuthService
    {
        private readonly UserRepository _repo;

        public AuthService(UserRepository repo)
        {
            _repo = repo;
        }

        //REGISTER
        public void Register(string username, string email, string password)
        {
            var existingUser = _repo.GetByEmail(email);
            if (existingUser != null)
                throw new Exception("Email đã tồn tại");

            var user = new User
            {
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                
            };

            _repo.Add(user);
        }

        //LOGIN
        public User Login(string email, string password)
        {
            var user = _repo.GetByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Sai tài khoản hoặc mật khẩu");

            return user;
        }
    }
}