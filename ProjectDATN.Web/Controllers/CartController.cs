using AspNetCoreHero.ToastNotification.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using ProjectDATN.Data.EF;
using ProjectDATN.Data.Entities;
using ProjectDATN.Web.Helpers;
using ProjectDATN.Web.Models;

namespace ProjectDATN.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _db;
        public INotyfService _notityService { get; }
        public CartController(ApplicationDBContext db, INotyfService notyfService)
        {
            _db = db;
            _notityService = notyfService;
        }


        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        public IActionResult Index()
        {
            return View(Carts);
        }
        public IActionResult CheckOut()
        {
            return View(Carts);
        }
        //public IActionResult Partial_View_Cart()
        //{
        //    return PartialView();
        //}
        public IActionResult AddToCart(int id, int quantity)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                var product = _db.Products.SingleOrDefault(x => x.Id == id);
                item = new CartItem
                {
                    ProductId = id,
                    ProductName = product.ProName,
                    Image = product.Image,
                    Price = product.PerchasePrice,
                    Quantity = quantity
                };
                myCart.Add(item);

            }
            else
            {
                item.Quantity += quantity;

            }
            HttpContext.Session.Set("GioHang", myCart);
            _notityService.Success("Thêm vào giỏ hàng thành công!");
            return Json(new { success = true, count = Carts.Count });
        }
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(x=>x.ProductId == id);
            try
            {
                if (item != null)
                {
                    item.Quantity= quantity;
                }
                HttpContext.Session.Set("GioHang", myCart);
                _notityService.Success("Cập nhật thành công!");
                return Json(new { success = true , quantity });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public IActionResult DeleteAll()
        {
            List<Cart> cartItems = _db.Carts.ToList();
            if (cartItems.Count > 0)
            {
                cartItems.Clear();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]

        public IActionResult Remove(int id)
        {
            try
            {
                List<CartItem> cart = Carts;
                CartItem item = Carts.SingleOrDefault(c => c.ProductId == id);
                if (item != null)
                {
                    cart.Remove(item);
                }

                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notityService.Success("Xóa thành công!");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }


    }


}
