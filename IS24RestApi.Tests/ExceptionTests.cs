using IS24RestApi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class ExceptionTests
    {
        const string Message = "The widget has unavoidably blooped out.";
        const HttpStatusCode StatusCode = HttpStatusCode.BadGateway;
        readonly Messages Msgs = new Messages
        {
            Message =
            {
                new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" },
                new Message { MessageCode = MessageCode.ERROR_COMMON_BAD_REQUEST, MessageProperty = "Bad Request", Id = "4712" }
            }
        };

        [Fact]
        public void TestSerializableExceptionWithCustomProperties()
        {
            var ex = new IS24Exception(Message) { StatusCode = StatusCode, Messages = Msgs };

            // Sanity check: Make sure custom properties are set before serialization
            Assert.Equal(Message, ex.Message);
            Assert.Equal(StatusCode, ex.StatusCode);
            Assert.Equal(Msgs.Message.Count, ex.Messages.Message.Count);
            Assert.Equal(Msgs.ToMessage(), ex.Messages.ToMessage());

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = ex.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, ex);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                ex = (IS24Exception)bf.Deserialize(ms);
            }

            // Make sure custom properties are preserved after serialization
            Assert.Equal(Message, ex.Message);
            Assert.Equal(StatusCode, ex.StatusCode);
            Assert.Equal(Msgs.Message.Count, ex.Messages.Message.Count);
            Assert.Equal(Msgs.ToMessage(), ex.Messages.ToMessage());

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.Equal(exceptionToString, ex.ToString());
        }
    }
}
