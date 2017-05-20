using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GpuStore.Domain.Abstract;
using GpuStore.Domain.Entities;
using GpuStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace GpuStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Card card = new Card
            {
                CardId = 2,
                Name = "Карта2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            // Организация - создание имитированного хранилища
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                card,
                new Card {CardId=3, Name="Карта3"}
            }.AsQueryable());
            // Организация - создание контроллера
            CardController controller = new CardController(mock.Object);
            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);
            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(card.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"}
            }.AsQueryable());
            // Организация - создание контроллера
            CardController controller = new CardController(mock.Object);
            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);
            // Утверждение
            Assert.IsNull(result);
        }
    }
}
