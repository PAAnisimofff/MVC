using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GpuStore.Domain.Entities;

namespace GpuStore.WebUI.Models
{
    public class CardsListViewModel
    {
        public IEnumerable<Card> Cards { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentManufacturer { get; set; }
    }
}