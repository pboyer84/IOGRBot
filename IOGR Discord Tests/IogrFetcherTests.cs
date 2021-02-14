using IOGRBot;
using NUnit.Framework;
using System;

namespace IOGR_Discord_Tests
{
    class IogrFetcherTests
    {
        [Test]
        public void TestMissingConfigThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(null));
            Assert.AreEqual(ex.Message, $"Missing object configuration for type {typeof(IOGRFetcher)}. Cannot start application.");
        }

        [Test]
        public void TestMissingInputJsonFileThrowsException()
        {
            IOGRFetcherConfiguration testInput = new IOGRFetcherConfiguration()
            {
                InputJsonFile = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: InputJsonFile. Cannot start application.");
        }

        [Test]
        public void TestMissingIogrApiGenerateEndpointThrowsException()
        {
            IOGRFetcherConfiguration testInput = new IOGRFetcherConfiguration()
            {
                InputJsonFile = "some value",
                IogrApiGenerateEndpoint = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: IogrApiGenerateEndpoint. Cannot start application.");
        }

        [Test]
        public void TestMissingIogrAppPermalinkBaseUrlThrowsException()
        {
            IOGRFetcherConfiguration testInput = new IOGRFetcherConfiguration()
            {
                InputJsonFile = "some value",
                IogrApiGenerateEndpoint = "some value",
                IogrAppPermalinkBaseUrl = string.Empty
            };

            var ex = Assert.Throws<ArgumentException>(() => CallFailingConstructor(testInput));
            Assert.AreEqual(ex.Message, "Missing configuration value: IogrAppPermalinkBaseUrl. Cannot start application.");
        }

        private void CallFailingConstructor(IOGRFetcherConfiguration input)
        {
            var sut = new IOGRFetcher(new System.Net.Http.HttpClient(), input);
        }
    }
}
