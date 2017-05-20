using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GpuStore.Domain.Abstract;
using GpuStore.Domain.Entities;
using GpuStore.WebUI.Controllers;
using GpuStore.WebUI.HtmlHelpers;
using GpuStore.WebUI.Models;

namespace GpuStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card {CardId=1, Name="Карта1"},
                new Card {CardId=2, Name="Карта2"},
                new Card {CardId=3, Name="Карта3"},
                new Card {CardId=4, Name="Карта4"},
                new Card {CardId=5, Name="Карта5"},
            });
            CardController controller = new CardController(mock.Object);
            controller.pageSize = 3;
            CardsListViewModel result = (CardsListViewModel)controller.List(null, 2).Model;
            List<Card> cards = result.Cards.ToList();
            Assert.IsTrue(cards.Count == 2);
            Assert.AreEqual(cards[0].Name, "Карта4");
            Assert.AreEqual(cards[1].Name, "Карта5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;
            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card { CardId = 1, Name = "Игра1"},
                new Card { CardId = 2, Name = "Игра2"},
                new Card { CardId = 3, Name = "Игра3"},
                new Card { CardId = 4, Name = "Игра4"},
                new Card { CardId = 5, Name = "Игра5"}
            });
            CardController controller = new CardController(mock.Object);
            controller.pageSize = 3;
            // Act
            CardsListViewModel result = (CardsListViewModel)controller.List(null, 2).Model;
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Cards()
        {
            //Организация
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card { CardId = 1, Name = "Игра1", Manufacturer = "Man1"},
                new Card { CardId = 2, Name = "Игра2", Manufacturer = "Man2"},
                new Card { CardId = 3, Name = "Игра3", Manufacturer = "Man1"},
                new Card { CardId = 4, Name = "Игра4", Manufacturer = "Man2"},
                new Card { CardId = 5, Name = "Игра5", Manufacturer = "Man1"}
            });
            CardController controller = new CardController(mock.Object);
            controller.pageSize = 3;
            //Action
            List<Card> result = ((CardsListViewModel)controller.List("Man2", 1).Model).Cards.ToList();
            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Карта2" && result[0].Manufacturer == "Man2");
            Assert.IsTrue(result[1].Name == "Карта4" && result[1].Manufacturer == "Man2");
        }
        [TestMethod]
        public void Can_Create_Manufacturers()
        {
            // Организация - создание имитированного хранилища
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card { CardId = 1, Name = "Игра1", Manufacturer = "NVIDIA"},
                new Card { CardId = 2, Name = "Игра2", Manufacturer = "AMD"},
                new Card { CardId = 3, Name = "Игра3", Manufacturer = "AMD"},
                new Card { CardId = 4, Name = "Игра4", Manufacturer = "NVIDIA"},
                new Card { CardId = 5, Name = "Игра5", Manufacturer = "NVIDIA"}
            });
            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);
            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();
            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0], "AMD");
            Assert.AreEqual(results[1], "NVIDIA");
        }
        [TestMethod]
        public void Indicates_Selected_Manufacturer()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new Card[]
            {
                new Card { CardId = 1, Name = "Игра1", Manufacturer = "NVIDIA"},
                new Card { CardId = 2, Name = "Игра2", Manufacturer = "AMD"}
            });
            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);
            // Организация - определение выбранной категории
            string manufacturerToSelect = "AMD";
            // Действие
            string result = target.Menu(manufacturerToSelect).ViewBag.SelectedCategory;
            // Утверждение
            Assert.AreEqual(manufacturerToSelect, result);
        }
        [TestMethod]
        public void Generate_Manufacturer_Specific_Card_count()
        {
            Mock<ICardRepository> mock = new Mock<ICardRepository>();
            mock.Setup(m => m.Cards).Returns(new List<Card>
            {
                new Card { CardId = 1, Name = "Игра1", Manufacturer = "Man1"},
                new Card { CardId = 2, Name = "Игра2", Manufacturer = "Man2"},
                new Card { CardId = 3, Name = "Игра3", Manufacturer = "Man1"},
                new Card { CardId = 4, Name = "Игра4", Manufacturer = "Man2"},
                new Card { CardId = 5, Name = "Игра5", Manufacturer = "Man1"}
            });
            CardController controller = new CardController(mock.Object);
            controller.pageSize = 3;
            //Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((CardsListViewModel)controller.List("Man1").Model).PagingInfo.TotalItems;
            int res2 = ((CardsListViewModel)controller.List("Man2").Model).PagingInfo.TotalItems;
            int resAll = ((CardsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;
            // Утверждение
            Assert.AreEqual(res1, 3);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(resAll, 5);
        }
    }
}
