using nthLink.Header.Interface;

namespace nthLink.SDK.Extension
{
    public static class IContainerProviderExtension
    {
        public static T? Resolve<T>(this IContainerProvider containerProvider)
        {
            if (containerProvider.Resolve(typeof(T)) is T t)
            {
                return t;
            }
            else
            {
                return default(T);
            }
        }
        public static T? Resolve<T>(this IContainerProvider containerProvider, string name)
        {
            if (containerProvider.Resolve(typeof(T), name) is T t)
            {
                return t;
            }
            else
            {
                return default(T);
            }
        }
    }
}
