using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Core.Interfaces;
using System;
using System.Linq;
using System.Net;

namespace Restaurant.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMenuService _menuService;        
        public OrderController(IMenuService menuService)
        {
            _menuService = menuService;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategory()
        {
            var _token = HttpContext.Session.GetString("_token");
            var result = _menuService.GetMenu(_token).Result;
            if (result.ResponseCode == (int)HttpStatusCode.OK)
            {
                var dataList = result.Data.ToList();
                return Json(dataList);
            }
            return Json(null);
        }

        public ActionResult GetMenu(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return Json(null);

            var _token = HttpContext.Session.GetString("_token");
            var result = _menuService.GetMenu(_token).Result;
            if (result.ResponseCode == (int)HttpStatusCode.OK)
            {
                var dataList = result.Data.Where(x => x.CategoryId == Guid.Parse(categoryId)).ToList();
                return Json(dataList);
            }
            return Json(null);
        }
    }
}