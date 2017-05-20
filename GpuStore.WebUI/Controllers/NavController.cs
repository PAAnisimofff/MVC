using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GpuStore.Domain.Abstract;

namespace GpuStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ICardRepository repository;
        public NavController(ICardRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string manufacturer = null)
        {
            ViewBag.SelectedCategory = manufacturer;
            IEnumerable<string> manufacturers = repository.Cards.Select(card => card.Manufacturer).Distinct().OrderBy(x => x);
            return PartialView("FlexMenu", manufacturers);
        }
    }
}