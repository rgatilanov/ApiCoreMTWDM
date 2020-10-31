using System;
using System.Collections.Generic;
using System.Text;

namespace APIUsers.Library.Interfaces
{
    public interface ILogin : IDisposable
    {
        Models.User EstablecerLogin(string nick, string pass);
        List<Models.User> ObtenerUsers();
    }
}
