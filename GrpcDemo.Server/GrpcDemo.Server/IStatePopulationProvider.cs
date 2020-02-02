namespace GrpcDemo.Server
{
    public interface IStatePopulationProvider
    {
        long Get(string state);
    }
}