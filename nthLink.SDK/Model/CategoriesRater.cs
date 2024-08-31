using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public class DefaultCategoriesRater : ICategoriesRater
    {
        public int GetRate(
            IDictionary<string, int> favoriteCategories,
            IEnumerable<string> categories)
        {
            if (favoriteCategories.Count == 0)
            {
                return 0;
            }

            int rate = 0;

            foreach (var item in categories)
            {
                if (favoriteCategories.ContainsKey(item))
                {
                    rate += favoriteCategories[item];
                }
            }

            return rate;
        }
    }
}
