using AspnetcoreGrpcService;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspnetcoreService.Services
{
    public class GrpcHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<GrpcHostedService> logger;
        
        public GrpcHostedService(ILogger<GrpcHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("GRPC Hosted Service running.");

            Task.Run(async () => {

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    await PerformGrpcGreet();
                }
            
            }, stoppingToken);

            return Task.CompletedTask;
        }

        private async Task PerformGrpcGreet()
        {
            var count = Interlocked.Increment(ref executionCount);

            logger.LogInformation(
                "GRPC Hosted Service is working. Count: {Count}", count);

            // This switch must be set before creating the GrpcChannel/ HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            try
            {
                // The port number(5001) must match the port of the gRPC server.
                using var channel = GrpcChannel.ForAddress("http://aspnetcoregrpcservice:5001");
                var client = new Greeter.GreeterClient(channel);
                var reply = await client.SayHelloAsync(
                                    new HelloRequest { Name = "GreeterClient" });

                logger.LogInformation(
                    "Got a reply: {Reply}", reply.Message);
            }
            catch(Exception e)
            {
                logger.LogWarning(e, "Not able to perform GRPC call.");
            }
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
