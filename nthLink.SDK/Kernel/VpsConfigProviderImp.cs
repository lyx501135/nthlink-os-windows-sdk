using nthLink.Header.Interface;

namespace nthLink.SDK.Kernel
{
    class VpsConfigProviderImp : IVpsConfigProvider
    {
        public string GetConfig()
        {
            return @"{
                        ""servers"": [
                            {
                                ""protocol"": ""outline"",
                                ""host"": ""205.134.180.130"",
                                ""port"": 443,
                                ""password"": ""NmFqHINId7op"",
                                ""encrypt_method"": ""chacha20-ietf-poly1305""
                            }
                        ],
                        ""redirectUrl"": ""https://www.nthlink.com/"",
                        ""headlineNews"": [
                            {
                                ""title"": ""俄军攻陷阿夫迪夫卡加剧对美国援助议案的反应"",
                                ""excerpt"": """",
                                ""image"": """",
                                ""url"": ""https://www.voachinese.com/a/avdiivka-loss-intensified-debates-20240218/7493071.html?utm_medium=proxy\u0026utm_campaign=persagg\u0026utm_source=nthlink\u0026utm_content=image""
                            },
                            {
                                ""title"": ""中国对乌克兰说，不向俄罗斯出售\""致命武器\"""",
                                ""excerpt"": """",
                                ""image"": """",
                                ""url"": ""https://www.voachinese.com/a/china-not-selling-lethal-weapons-to-russia-20240218/7492718.html?utm_medium=proxy\u0026utm_campaign=persagg\u0026utm_source=nthlink\u0026utm_content=image""
                            },
                            {
                                ""title"": ""中国同意解除西班牙牛肉进口禁令"",
                                ""excerpt"": """",
                                ""image"": """",
                                ""url"": ""https://www.voachinese.com/a/china-agrees-to-lift-ban-on-spanish-beef-imports/7492716.html?utm_medium=proxy\u0026utm_campaign=persagg\u0026utm_source=nthlink\u0026utm_content=image""
                            }
                        ],
                        ""notifications"": [
                            {
                                ""title"": ""nthLink for Windows"",
                                ""url"": ""https://www.nthlink.com/""
                            },
                            {
                                ""title"": ""We have updated our Privacy Policy"",
                                ""url"": ""https://www.nthlink.com/""
                            }
                        ]
                    }";
        }
    }
}
