using Confluent.Kafka;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AspnetcoreGrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            this.logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            logger.LogInformation(
                "Received a GRPC request: {Request}", request.Name);

            PublishKafkaMessage();

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        private void PublishKafkaMessage()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "bootstrap.kafka.svc.cluster.local:9092",
                ClientId = Dns.GetHostName()
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var t = producer.ProduceAsync("services-topic", new Message<Null, string> { Value = $"Kafka GRPC Response from { Environment.MachineName }" });
                t.ContinueWith(task => {
                    if (task.IsFaulted)
                    {
                        logger.LogWarning("Not able to publish message to Kafka.");
                    }
                    else
                    {
                        logger.LogInformation($"Wrote to offset: {task.Result.Offset}");
                    }
                });
            }
        }
    }
}
