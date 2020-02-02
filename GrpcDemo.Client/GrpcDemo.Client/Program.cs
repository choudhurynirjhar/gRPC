using Grpc.Net.Client;
using System;

namespace GrpcDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001/");
            var client = new GrpcPopulation.PopulationProvider.PopulationProviderClient(channel);

            var population = client.GetPopulation(new GrpcPopulation.PopulationRequest { State = "NY" });
            Console.WriteLine(population.Count);
        }
    }
}
