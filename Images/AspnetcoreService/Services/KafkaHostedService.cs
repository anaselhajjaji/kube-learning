using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspnetcoreService.Services
{
    public class KafkaHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<GrpcHostedService> logger;
        
        public KafkaHostedService(ILogger<GrpcHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Kafka Hosted Service running.");

            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "services-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            Task.Run(() => {
    
                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(new string[] { "services-topic" });

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        logger.LogInformation($"Consumed a message: {consumeResult.Message.Value}");
                    }

                    consumer.Close();
                }                
            
            }, stoppingToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("GRPC Hosted Service is stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
