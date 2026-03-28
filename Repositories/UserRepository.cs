using System.Linq;
using Todo_list.Models;
using Todo_list.Data;

namespace Todo_list.Repositories
{
    public class UserRepository
    {
            private readonly ApplicationDbContext _context;

            // Constructor để nhận DbContext
            public UserRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            // Lấy user theo email (dùng cho login)
            public User GetByEmail(string email)
            {
                return _context.Users.FirstOrDefault(x => x.Email == email);
            }

            // Thêm user (dùng cho register)
            public void Add(User user)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }
    }

