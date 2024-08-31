using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public class HttpObfuscatorImp : IHttpObfuscator
    {
        private int times;
        private int interval;
        private List<string>? list;
        private Timer? timer;

        public event EventHandler<HttpObfuscatorUrlEventArgs>? ObfuscatorUrl;

        public void Start(IEnumerable<string> urls, int times, int intervalSecond)
        {
            if (this.timer == null)
            {
                this.list = urls.ToList();
                this.times = times;
                this.interval = intervalSecond;

                this.timer = new Timer(OnTicks);

                this.timer.Change(TimeSpan.Zero, TimeSpan.Zero);
            }
        }

        private async void OnTicks(object? state)
        {
            if (this.list != null &&
                this.list.Count > 0 &&
                this.times > 0)
            {
                Random r = new Random(DateTime.Now.Second);

                int index = r.Next(0, this.list.Count);

                string url = this.list[index];

                if (!string.IsNullOrEmpty(url))
                {
                    HttpObfuscatorUrlEventArgs args = new HttpObfuscatorUrlEventArgs(url);

                    if (ObfuscatorUrl != null)
                    {
                        ObfuscatorUrl.Invoke(this, args);
                    }

                    if (args.IsRequestGet)
                    {
                        HttpClientHandler httpClientHandler = new HttpClientHandler()
                        {
                            UseCookies = false
                        };

                        HttpClient httpClient = new HttpClient(httpClientHandler);

                        await httpClient.GetAsync(url);
                    }
                }

                this.times--;

                Timer? t = this.timer;

                if (t != null)
                {
                    //50%
                    int range = this.interval >> 1;

                    int randomSec = r.Next(-range, range);

                    TimeSpan timeSpan = TimeSpan.FromSeconds(this.interval + randomSec);

                    const int minimum = 10;

                    if (timeSpan.TotalSeconds < minimum)
                    {
                        timeSpan = TimeSpan.FromSeconds(minimum);
                    }

                    t.Change(timeSpan, TimeSpan.Zero);
                }
            }
            else
            {
                Stop();
            }
        }

        public void Stop()
        {
            if (this.timer != null)
            {
                this.timer.Change(Timeout.Infinite, Timeout.Infinite);
                this.timer = null;
            }
        }
    }
}
