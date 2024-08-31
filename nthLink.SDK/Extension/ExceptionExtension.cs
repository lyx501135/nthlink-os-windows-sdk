namespace nthLink.SDK.Extension
{
    public static class ExceptionExtension
    {
        public static string ToDisplay(this Exception self)
        {
            if (self == null)
            {
                return String.Empty;
            }

            if (self.InnerException == null)
            {
                return $"{self.Message}{Environment.NewLine}{self.StackTrace}";
            }
            else
            {
                return ToDisplay(self.InnerException);
            }
        }
    }
}
