using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using transaction_api.Interfaces;
using transaction_api.Repositories;



namespace transaction_api_tests
{
    public class ClientRepositoryTests
    {
        private readonly Mock<IDapperContext> _mockContext;
        private readonly Mock<ILogger<ClientRepository>> _mockLogger;

        public ClientRepositoryTests()
        {
            _mockContext = new Mock<IDapperContext>();
            _mockLogger = new Mock<ILogger<ClientRepository>>();
        }

        [Fact]
        public async Task GetClientAsync_ReturnsClient()
        {
            // Arrange
            int clientId = 1; // Example client ID
            var expectedClient = new Client { ClientID = clientId, Name = "John", Surname = "Doe", ClientBalance = 100.00M };
            
            _mockContext.Setup(ctx => ctx.QuerySingleOrDefaultAsync<Client>(
                It.IsAny<string>(), 
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(expectedClient);


            ClientRepository _clientRepository = new ClientRepository(_mockLogger.Object, _mockContext.Object);

            // Act
            var result = await _clientRepository.GetClientAsync(clientId);

            // Assert
            Assert.Equal(expectedClient, result);
        }
    }
}