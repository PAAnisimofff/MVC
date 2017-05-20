using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GpuStore.Domain.Entities
{
    public class Card
    {
        [HiddenInput(DisplayValue = false)]
        public int CardId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название карты")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание карты")]
        public string Description { get; set; }
        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Пожалуйста, введите производителя карты")]
        public string Manufacturer { get; set; }
        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
