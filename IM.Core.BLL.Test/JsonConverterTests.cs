using FluentAssertions;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.WebAPIClient;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace IM.Core.BLL.Test
{
    [TestFixture]
    internal class JsonConverterTests
    {
        [Test]
        public void ItSerializeAndDeserializeCallData()
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new []
                {
                    new NullableGuidPropertyConverter()
                }
            };
            var callData = new CallData()
            {
                ExecutorID = new NullablePropertyWrapper<Guid>()
                {
                    IsEmpty = false,
                    Value = Guid.NewGuid(),
                },
                EntityStateName = "Initialized",
                OwnerID = new NullablePropertyWrapper<Guid>() { IsEmpty = true }
            };
            var serialized = JsonConvert.SerializeObject(callData, settings);
            var deserialized = JsonConvert.DeserializeObject<CallData>(serialized, settings);

            deserialized.EntityStateName.Should().Be(callData.EntityStateName);
            deserialized.OwnerID.Should().Be(callData.OwnerID);
            deserialized.ExecutorID.Should().Be(callData.ExecutorID);
        }
    }
}