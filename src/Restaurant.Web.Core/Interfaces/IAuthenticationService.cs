using Restaurant.Web.Core.Helpers.Autofac;
using Restaurant.Web.Data;
using Restaurant.Web.Data.Models.Authentication;
using System.Threading.Tasks;

namespace Restaurant.Web.Core.Interfaces
{
    public interface IAuthenticationService : IAutoDependencyCore
    {
        /// <summary>
        /// Logs In User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Response<LoginResponse>> Authenticate(string email, string password);
    }
}