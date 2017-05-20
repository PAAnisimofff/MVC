using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GpuStore.Domain.Abstract;
using GpuStore.Domain.Entities;

namespace GpuStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        ICardRepository repository;
        public AdminController (ICardRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Cards);
        }
        public ViewResult Edit(int cardId)
        {
            Card card = repository.Cards.FirstOrDefault(g => g.CardId == cardId);
            return View(card);
        }
        [HttpPost]
        public ActionResult Edit(Card card, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    card.ImageMimeType = image.ContentType;
                    card.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(card.ImageData, 0, image.ContentLength);
                }
                repository.SaveCard(card);
                TempData["message"] = string.Format("Изменения в карте \"{0}\" были сохранены", card.Name);
                return RedirectToAction("Index");
            }
            else
                return View(card);
        }
        public ViewResult Create()
        {
            return View("Edit", new Card());
        }
        [HttpPost]
        public ActionResult Delete(int cardId)
        {
            Card deletedCard = repository.DeleteCard(cardId);
            if (deletedCard != null)
                TempData["message"] = string.Format("Карта \"{0}\" была удалена", deletedCard.Name);
            return RedirectToAction("Index");
        }
    }
}