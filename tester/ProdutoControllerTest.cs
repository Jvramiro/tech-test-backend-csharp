using TestJrAPI.Controllers;
using TestJrAPI.Services;
using Moq;
using TestJrAPI.DTO.Produtos;
using TestJrAPI.Enums;
using TestJrAPI.Models;
using Microsoft.AspNetCore.Mvc;
using TestJrAPI.Data;
using TestJrAPI.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace tester {
    [TestClass]
    public class ProdutoControllerTest {

        private Mock<IDatabaseService> mockDatabaseService;
        private ProdutosController produtoController;

        [TestInitialize]
        public void Init() {
            mockDatabaseService = new Mock<IDatabaseService>();
            produtoController = new ProdutosController(mockDatabaseService.Object);
        }

        #region Create Produto

        [TestMethod]
        public async Task Create_ReturnCreated() {

            // Arrange
            var request = new ProdutoRequest ("Mouse" , 45, 20);

            // Act
            mockDatabaseService
                .Setup(svc => svc.CreateProduto(It.IsAny<Produto>(), It.IsAny<DatabaseSelection>()))
                .Verifiable();
            var result = await produtoController.Create(request, DatabaseSelection.SqlServer);
            var response = ((CreatedResult)result).Value as ProdutoResponse;

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(request.Nome, response.Nome);

            Assert.AreEqual(request.Preco*request.Quantidade, response.ValorTotal);

            mockDatabaseService.Verify(v =>
                v.CreateProduto(It.Is<Produto>(p => p.Nome == request.Nome),
                DatabaseSelection.SqlServer), 
                Times.Once
            );
        }

        [TestMethod]
        public async Task Create_ReturnBadRequest_PrecoNegative() {

            // Arrange
            var request = new ProdutoRequest("TestProduct", -10, 5);

            // Act
            mockDatabaseService
                .Setup(svc => svc.CreateProduto(It.IsAny<Produto>(), It.IsAny<DatabaseSelection>()))
                .Verifiable();
            var result = await produtoController.Create(request, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public async Task Create_ReturnBadRequest_InvalidData() {

            // Arrange
            var request = new ProdutoRequest("", 0, -10);

            // Act
            mockDatabaseService
                .Setup(svc => svc.CreateProduto(It.IsAny<Produto>(), It.IsAny<DatabaseSelection>()))
                .Verifiable();
            var result = await produtoController.Create(request, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        #endregion
        #region Get Produto

        [TestMethod]
        public async Task GetById_ReturnOk_ExistingId() {

            // Arrange
            var existingId = Guid.NewGuid();
            var expectedProduto = new Produto("Mouse", 45, 20);
            expectedProduto.Id = existingId;

            mockDatabaseService.Setup(svc => svc.GetById(existingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync(expectedProduto);

            // Act
            var result = await produtoController.GetById(existingId, DatabaseSelection.SqlServer);
            var objectResult = (OkObjectResult)result;
            var response = objectResult.Value as Produto;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(objectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(existingId, response.Id);
            Assert.AreEqual(expectedProduto.Nome, response.Nome);
        }

        [TestMethod]
        public async Task GetById_ReturnNotFound_NonExistingId() {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            mockDatabaseService.Setup(svc => svc.GetById(nonExistingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync((Produto)null);

            // Act
            var result = await produtoController.GetById(nonExistingId, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetById_ReturnBadRequest_InvalidIdFormat() {
            // Arrange
            var invalidId = "invalid-guid-format";

            // Act
            try {
                var result = await produtoController.GetById(Guid.Parse(invalidId), DatabaseSelection.SqlServer);
                Assert.Fail("Expected FormatException");
            }
            catch (Exception exception) {
                // Assert
                Assert.IsInstanceOfType(exception, typeof(FormatException));
            }
        }

        [TestMethod]
        public async Task GetAll_ReturnOk() {
            // Arrange
            var expectedProduto_01 = new Produto("Mouse", 45, 20);
            var expectedProduto_02 = new Produto("Keyboard", 70, 10);
            expectedProduto_01.Id = new Guid();
            expectedProduto_02.Id = new Guid();

            var mockProdutos = new List<Produto> { expectedProduto_01, expectedProduto_02 };
            var expectedProdutos = mockProdutos.Select(p => new ProdutoResponse(
                p.Id, p.Nome, p.Preco, p.Quantidade, p.ValorTotal)
            ).ToList();

            mockDatabaseService.Setup(svc => svc.GetAllProduto(DatabaseSelection.SqlServer, 1, 10))
                      .ReturnsAsync(mockProdutos);

            // Act
            var result = await produtoController.GetAll(1, 10, DatabaseSelection.SqlServer);

            var objectResult = (OkObjectResult)result;
            var response = objectResult.Value as IEnumerable<ProdutoResponse>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(objectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(mockProdutos.Count(), response.Count());

            Assert.IsTrue(expectedProdutos.SequenceEqual(response.ToList()));

        }

        [TestMethod]
        public async Task GetAll_BadRequest_ExceededRows() {
            // Arrange
            var expectedProduto_01 = new Produto("Mouse", 45, 20);
            var expectedProduto_02 = new Produto("Keyboard", 70, 10);
            int exceededRowsNumber = 100;

            var mockProdutos = new List<Produto> { expectedProduto_01, expectedProduto_02 };

            // Act
            var result = await produtoController.GetAll(1, exceededRowsNumber, DatabaseSelection.SqlServer);


            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }

        #endregion
        #region Update Produto

        [TestMethod]
        public async Task Update_ReturnOk_ExistingId() {
            // Arrange
            var existingId = Guid.NewGuid();
            var updatedRequest = new ProdutoRequest("Mouse", 30, 10);
            var existingProduto = new Produto("Mose", 20, 5);
            existingProduto.Id = existingId;

            mockDatabaseService.Setup(svc => svc.GetById(existingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync(existingProduto);

            // Act
            var result = await produtoController.Update(existingId, updatedRequest, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnNotFound_NonExistingId() {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var updatedRequest = new ProdutoRequest("Mouse", 30, 10);

            mockDatabaseService.Setup(svc => svc.GetById(nonExistingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync((Produto)null);

            // Act
            var result = await produtoController.Update(nonExistingId, updatedRequest, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnBadRequest_InvalidData() {
            // Arrange
            var existingId = Guid.NewGuid();
            var invalidRequest = new ProdutoRequest("", -10, 0);
            var existingProduto = new Produto("Mouse", 20, 5);
            existingProduto.Id = existingId;

            mockDatabaseService.Setup(svc => svc.GetById(existingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync(existingProduto);

            // Act
            var result = await produtoController.Update(existingId, invalidRequest, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnBadRequest_InvalidIdFormat() {
            // Arrange
            var invalidId = "invalid-guid-format";
            var updatedRequest = new ProdutoRequest("Mouse", 30, 10);

            // Act
            try {
                var result = await produtoController.Update(Guid.Parse(invalidId), updatedRequest, DatabaseSelection.SqlServer);
                Assert.Fail("Expected FormatException");
            }
            catch (Exception exception) {
                // Assert
                Assert.IsInstanceOfType(exception, typeof(FormatException));
            }
        }

        #endregion
        #region Delete Produto

        [TestMethod]
        public async Task Delete_ReturnOk_ExistingId() {
            // Arrange
            var existingId = Guid.NewGuid();
            var existingProduto = new Produto("Mouse", 50, 3);
            existingProduto.Id = existingId;

            mockDatabaseService.Setup(svc => svc.GetById(existingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync(existingProduto);

            // Act
            var result = await produtoController.Delete(existingId, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnNotFound_NonExistingId() {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            mockDatabaseService.Setup(svc => svc.GetById(nonExistingId, DatabaseSelection.SqlServer))
                              .ReturnsAsync((Produto)null);

            // Act
            var result = await produtoController.Delete(nonExistingId, DatabaseSelection.SqlServer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnBadRequest_InvalidIdFormat() {
            // Arrange
            var invalidId = "invalid-guid-format";

            // Act
            try {
                var result = await produtoController.Delete(Guid.Parse(invalidId), DatabaseSelection.SqlServer);
                Assert.Fail("Expected FormatException");
            }
            catch (Exception exception) {
                // Assert
                Assert.IsInstanceOfType(exception, typeof(FormatException));
            }
        }

        #endregion
    }
}