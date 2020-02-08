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
            var channel = GrpcChannel.ForAddress("https://localhost:5001/");
            var client = new GrpcClientCount.ClientCountProvider.ClientCountProviderClient(channel);

            var token = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            using var population = client.GetClientCount(new Empty(), cancellationToken: token.Token  );
            try
            {
                await foreach (var item in population.ResponseStream.ReadAllAsync(token.Token))
                { Console.WriteLine(item.Count); }
            }
            catch(RpcException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
