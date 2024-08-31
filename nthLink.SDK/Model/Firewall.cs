using nthLink.Header;
using nthLink.Header.Enum;
using nthLink.Header.Interface;
using WindowsFirewallHelper;
using WindowsFirewallHelper.FirewallRules;

namespace nthLink.SDK.Model
{
    public class Firewall : Header.Interface.IFirewall
    {
        private const string RuleName = Const.String.ProductName;

        private readonly ILogger logger;

        public Firewall(ILogger logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Create firewall rule
        /// </summary>
        public void Open()
        {
            if (!FirewallWAS.IsLocallySupported)
            {
                this.logger.Log(LogLevelEnum.Warn, "Windows Firewall Locally Unsupported");
                return;
            }

            try
            {
                IFirewallRule? rule = FirewallManager.Instance.Rules
                    .FirstOrDefault(r => r.Name == RuleName);

                if (rule != null)
                {
                    if (rule.ApplicationName.StartsWith(Util.GetWorkDirectory()))
                        return;

                    Close();
                }

                foreach (var path in Directory.GetFiles(Util.GetWorkDirectory(), "*.exe",
                    SearchOption.AllDirectories))
                {
                    AddFireWallRule(RuleName, path);
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevelEnum.Warn, "Create Firewall rules error", e);
            }
        }
        /// <summary>
        /// Remove firewall rule
        /// </summary>
        public void Close()
        {
            if (!FirewallWAS.IsLocallySupported)
                return;

            try
            {
                foreach (var rule in FirewallManager.Instance.Rules.Where(
                    r => r.ApplicationName?.StartsWith(Util.GetWorkDirectory(),
                    StringComparison.OrdinalIgnoreCase) ?? r.Name == RuleName))
                {
                    FirewallManager.Instance.Rules.Remove(rule);
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevelEnum.Warn, "Remove Firewall rules error", e);
            }
        }
        private static void AddFireWallRule(string ruleName, string exeFullPath)
        {
            var rule = new FirewallWASRule(ruleName,
                exeFullPath,
                FirewallAction.Allow,
                FirewallDirection.Inbound,
                FirewallProfiles.Private | FirewallProfiles.Public | FirewallProfiles.Domain);

            FirewallManager.Instance.Rules.Add(rule);
        }
    }
}
