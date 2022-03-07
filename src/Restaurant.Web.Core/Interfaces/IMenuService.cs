using Restaurant.Web.Core.Helpers.Autofac;
using Restaurant.Web.Data;
using Restaurant.Web.Data.Models.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Web.Core.Interfaces
{
    public interface IMenuService : IAutoDependencyCore
    {
        Task<Response<IEnumerable<MenuResponse>>> GetMenu(string token);
    }
}