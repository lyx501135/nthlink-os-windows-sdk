namespace nthLink.Header.Interface
{
    public interface ICategoriesRater
    {
        int GetRate(IDictionary<string, int> favoriteCategories,
            IEnumerable<string> categories);
    }
}
