using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Constants;

namespace Shipping.Controllers
{
    public class ProductController : Controller
    {
        [Authorize(Permissions.Products.View)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.Products.Edit)]
         public IActionResult Edit()
        {
            return View();
        }
        


    }
}
