namespace Book1.Interfaces
{
    public interface IHttpRequestService
    {
        Task<string> SendRequestAsync(string url);

    }
}
