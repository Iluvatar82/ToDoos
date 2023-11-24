namespace ServiceJobs.Base
{
    public interface IJob
    {
        Task Execute();
    }
}
