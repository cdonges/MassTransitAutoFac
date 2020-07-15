namespace MassTransitAutoFac
{
    public class SampleService : ISampleService
    {
        public int AddNumbers(int first, int second)
        {
            return first + second;
        }
    }
}