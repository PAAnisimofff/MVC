using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GpuStore.Domain.Entities;
using GpuStore.Domain.Abstract;
using GpuStore.WebUI.Models;

namespace GpuStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ICardRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(ICardRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
                return View(shippingDetails);
        }
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewVodel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart, int cardId, string returnUrl)
        {
            Card card = repository.Cards.FirstOrDefault(g => g.CardId == cardId);
            if (card != null)
            {
                cart.AddItem(card, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int cardId, string returnUrl)
        {
            Card card = repository.Cards.FirstOrDefault(g => g.CardId == cardId);
            if (card != null)
            {
               cart.RemoveLine(card);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    }
}