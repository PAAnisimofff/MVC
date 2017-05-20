using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using GpuStore.Domain.Abstract;
using GpuStore.Domain.Entities;
using GpuStore.WebUI.Controllers;

namespace GpuStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Cards()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"},
                new Card {CardId=3, Name="Карта3"},
                new Card {CardId=4, Name="Карта4"},
                new Card {CardId=5, Name="Карта5"}
            });
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Действие
            List<Card> result = ((IEnumerable<Card>)controller.Index().ViewData.Model).ToList();
            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Карта1", result[0].Name);
            Assert.AreEqual("Карта2", result[1].Name);
            Assert.AreEqual("Карта3", result[2].Name);
        }
        [TestMethod]
        public void Can_Edit_Card()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"},
                new Card {CardId=3, Name="Карта3"},
                new Card {CardId=4, Name="Карта4"},
                new Card {CardId=5, Name="Карта5"}
            });
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Действие
            Card card1 = controller.Edit(1).ViewData.Model as Card;
            Card card2 = controller.Edit(2).ViewData.Model as Card;
            Card card3 = controller.Edit(3).ViewData.Model as Card;
            //Assert
            Assert.AreEqual(1, card1.CardId);
            Assert.AreEqual(2, card2.CardId);
            Assert.AreEqual(3, card3.CardId);
        }
        [TestMethod]
        public void Cannot_Edit_Nonexistent_Card()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"},
                new Card {CardId=3, Name="Карта3"},
                new Card {CardId=4, Name="Карта4"},
                new Card {CardId=5, Name="Карта5"}
            });
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Действие
            Card result = controller.Edit(6).ViewData.Model as Card;
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Организация - создание объекта Game
            Card card = new Card { Name = "Test" };
            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(card);
            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveCard(card));
            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Организация - создание объекта Game
            Card card = new Card { Name = "Test" };
            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");
            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(card);
            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveCard(It.IsAny<Card>()), Times.Never());
            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Can_Delete_Cards()
        {
            // Организация - создание объекта
            Card card = new Card { CardId = 2, Name = "Карта2" };
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"},
                new Card {CardId=3, Name="Карта3"},
                new Card {CardId=4, Name="Карта4"},
                new Card {CardId=5, Name="Карта5"}
            });
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            // Действие - удаление
            controller.Delete(card.CardId);
            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Card
            mock.Verify(m => m.DeleteCard(card.CardId));
        }
    }
}
