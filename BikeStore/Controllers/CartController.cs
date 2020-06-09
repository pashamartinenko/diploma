using BikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BikeStore.Controllers
{
    public class CartController : Controller
    {

        private DatabaseShopEntities1 db = new DatabaseShopEntities1();
        public ViewResult Index()
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),              
            });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public RedirectToRouteResult AddToCart(int productId)
        {
            Bikes product = db.Bikes.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                GetCart().AddItem(productId, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult RemoveFromCart(int productId)
        {
            Bikes product = db.Bikes.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult OrderView()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Save(Orders order)
        {
            Cart cart = GetCart(); 

            db.Orders.Add(order);
            Bikes_Orders purchase;
            foreach (CartLine line in cart.Lines)
            {
                purchase = new Bikes_Orders
                {
                    BikeId = line.Product.Id,
                    OrderId = order.Id,
                    Quantity = line.Quantity
                };
                db.Bikes_Orders.Add(purchase);
            }

            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");           
        }
    }
}