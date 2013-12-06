using System;
using Moq;
using Xunit;

namespace IS24RestApi.Tests
{
    public class ImportExportClientTest
    {
        private readonly Mock<IIS24Connection> connectionMock;
        private readonly IIS24Connection connection;

        public ImportExportClientTest()
        {
            connectionMock = new Mock<IIS24Connection>();
            connection = connectionMock.Object;
        }

        [Fact]
        public void Ctor_null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ImportExportClient(null));
        }

        [Fact]
        public void Ctor_null_ThrowsArgumentNullExceptionParamNameConnection()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ImportExportClient(null));
            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public void Ctor_ConnectionMock_ConnectionConnectionMock()
        {
            var client = new ImportExportClient(connection);
            Assert.Equal(connection, client.Connection);
        }

        [Fact]
        public void Ctor_ConnectionMock_RealEstatesConnectionIsSet()
        {
            var client = new ImportExportClient(connection);
            Assert.Equal(connection, client.RealEstates.Connection);
        }

        [Fact]
        public void Ctor_ConnectionMock_ContactsConnectionIsSet()
        {
            var client = new ImportExportClient(connection);
            Assert.Equal(connection, client.Contacts.Connection);
        }

        [Fact]
        public void Ctor_ConnectionMock_PublishConnectionIsSet()
        {
            var client = new ImportExportClient(connection);
            Assert.Equal(connection, client.Publish.Connection);
        }

        [Fact]
        public void Ctor_ConnectionMock_PublishChannelsConnectionIsSet()
        {
            var client = new ImportExportClient(connection);
            Assert.Equal(connection, client.PublishChannels.Connection);
        }
    }
}
