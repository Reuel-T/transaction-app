using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using transaction_api.Repositories;



namespace transaction_api_tests
{
    public class ClientRepositoryTests
    {
        private readonly Mock<IDapperContext> _mockContext;
        private readonly Mock<IClientRepository> _MockClientRepository;

        public ClientRepositoryTests()
        {
            _mockContext = new Mock<IDapperContext>();
            _MockClientRepository = new Mock<IClientRepository>();
        }

        [Fact]
        [Trait("ClientRepository", "GetClientsAsync")]
        public async Task GetClientAsync_ReturnsClients()
        {
            //Arrange
            var expectedClients = new List<Client>
            {
                new Client {ClientID = 1, ClientBalance = 1000, Name= "John", Surname = "Doe"},
                new Client {ClientID = 2, ClientBalance = 2000, Name= "Jane", Surname = "Doe"},
                new Client {ClientID = 3, ClientBalance = 3000, Name= "Joe", Surname = "Doe"}
            };

            //set up the client repository mock to return the expected list of clients
            _MockClientRepository.Setup(repo => repo.GetClientsAsync())
                .ReturnsAsync(expectedClients);

            //Act
            //run the getClientsAsync method
            var result = await _MockClientRepository.Object.GetClientsAsync();

            //Assert
            Assert.Equal(expectedClients, result);
        }     
    }
}