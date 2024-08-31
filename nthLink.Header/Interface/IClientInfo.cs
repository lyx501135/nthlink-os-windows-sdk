namespace nthLink.Header.Interface
{
    public interface IClientInfo
    {
        Version AppVersion { get; }
        Version SdkVersion { get; }
        string ClientGuid { get; }
        string DomainKeys { get; }
        /// <summary>
        /// Category, Rate
        /// </summary>
        IDictionary<string, int> FavoriteCategories { get; }

        void UpdateFavoriteCategories(string category, int rate);
    }
}
