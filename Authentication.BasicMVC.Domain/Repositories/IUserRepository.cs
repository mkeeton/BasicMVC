using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BasicMVC.Domain.Models;

namespace Authentication.BasicMVC.Domain.Repositories
{
    public interface IUserRepository
    {

        IEnumerable<User> Users { get; }

        User Load(Guid id);

        User Save(User user);

        bool Remove(Guid id, User DeleteUser);


        IEnumerable<User> GetUsers(User.SearchParameters searchParameters = null);
    }
}
