using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcDemo.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ClientStream();
        }

        private static async Task ServerStream()
        {
            var credentials = CallCredentials.FromInterceptor((c, m) => {
                m.Add("Authorization",
                    "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNTgxOTYyNzI0LCJleHAiOjE1ODE5NjYzMjQsImlhdCI6MTU4MTk2MjcyNH0.VvYln0PgZQrFwBTx0Ik3TGGI43DxdVVxzHAXma-K5P0");
                return Task.CompletedTask;
            });

            var channel = GrpcChannel.ForAddress("https://localhost:5001/",
                new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });

            var client = new GrpcClientCount.ClientCountProvider.ClientCountProviderClient(channel);

            var token = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            using var population = client.GetClientCount(
                new Empty(),
                cancellationToken: token.Token);
            try
            {
                await foreach (var item in population.ResponseStream.ReadAllAsync(token.Token))
                { Console.WriteLine(item.Count); }
            }
            catch (RpcException exc)
            {
                Console.WriteLine(exc.Message);
            }

        }

        private static async Task ClientStream()
        {
            var credentials = CallCredentials.FromInterceptor((c, m) => {
                m.Add("Authorization",
                    "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNTgxOTYyNzI0LCJleHAiOjE1ODE5NjYzMjQsImlhdCI6MTU4MTk2MjcyNH0.VvYln0PgZQrFwBTx0Ik3TGGI43DxdVVxzHAXma-K5P0");
                return Task.CompletedTask;
            });

            var channel = GrpcChannel.ForAddress("https://localhost:5001/",
                new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });

            var client = new GrpcPopulation.PopulationProvider.PopulationProviderClient(channel);

            using var populationRequest = client.GetPopulation();

            foreach (var state in new [] { "NY", "NJ", "MD", "KY"})
            {
                await populationRequest.RequestStream.WriteAsync(new GrpcPopulation.PopulationRequest
                {
                    State = state
                });
            }

            await populationRequest.RequestStream.CompleteAsync();

            var response = await populationRequest.ResponseAsync;

            Console.WriteLine(response.Count);

        }
    }
}
