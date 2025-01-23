namespace Survey.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key , CancellationToken cancellationToken = default) where T : class;
        Task SetAsync<T>(string key,T Value, CancellationToken cancellationToken = default) where T : class;
        Task Remove(string key, CancellationToken cancellationToken = default);

    }
}
