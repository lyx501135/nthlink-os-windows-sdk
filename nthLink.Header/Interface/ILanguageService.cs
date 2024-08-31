namespace nthLink.Header.Interface
{
    /// <summary>
    /// 語系服務
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// 取得本地化顯示用字串
        /// </summary>
        /// <param name="stringKey">Key</param>
        /// <returns></returns>
        string GetString(string stringKey);

        string GetStringWithDefaultCulture(string stringKey);
    }
}
