using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GpuStore.Domain.Entities;

namespace GpuStore.WebUI.Models
{
    public class CartIndexViewVodel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}