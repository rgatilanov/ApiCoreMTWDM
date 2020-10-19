using System;
using System.Collections.Generic;
using System.Text;

namespace APIUsers.Library.Interfaces
{
    public interface IUser:IDisposable
    {
        List<Models.User> GetUsers();
    }
}
