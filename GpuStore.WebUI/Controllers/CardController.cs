using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GpuStore.Domain.Abstract;
using GpuStore.Domain.Entities;
using GpuStore.WebUI.Models;

namespace GpuStore.WebUI.Controllers
{
    public class CardController : Controller
    {
        private ICardRepository repository;
        public int pageSize = 4;
        public CardController(ICardRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string manufacturer, int page = 1)
        {
            CardsListViewModel model = new CardsListViewModel
            {
                Cards = repository.Cards.Where(p => manufacturer == null || p.Manufacturer == manufacturer).OrderBy(card => card.CardId).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = manufacturer == null ? repository.Cards.Count() : repository.Cards.Where(card=>card.Manufacturer==manufacturer).Count()
                },
                CurrentManufacturer = manufacturer
            };
            return View(model);
        }
        public FileContentResult GetImage(int cardId)
        {
            Card card = repository.Cards.FirstOrDefault(g => g.CardId == cardId);
            if (card != null)
                return File(card.ImageData, card.ImageMimeType);
            else
                return null;
        }
    }
}