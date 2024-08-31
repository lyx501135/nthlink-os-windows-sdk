using nthLink.Header.Interface;
using System.Reflection;

namespace nthLink.SDK
{
    class ModuleLoader : IModuleLoader
    {
        public bool IsLoaded { get; private set; }
        public bool IsResisted { get; private set; }

        private readonly List<IModuleInfo> modules = new List<IModuleInfo>();
        public async Task RegistryAsync(IContainerRegistry containerRegistry)
        {
            if (IsResisted)
            {
                return;
            }

            IsResisted = true;

            List<IModuleInfo> loadList = new List<IModuleInfo>();

            if (this.modules.Count > 0)
            {
                loadList.AddRange(this.modules);
            }

            if (loadList.Count != 0)
            {
                Task[] tasks = new Task[loadList.Count];
#if DEBUG
                System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
                System.Text.StringBuilder moduleNames = new System.Text.StringBuilder();
#endif
                for (int i = 0; i < loadList.Count; i++)
                {
                    tasks[i] = loadList[i].OnRegisterAsync(containerRegistry);
#if DEBUG
                    moduleNames.Append($"{loadList[i].GetType().FullName}, ");
#endif
                }

                await Task.WhenAll(tasks);
#if DEBUG
                stopwatch.Stop();
                System.Diagnostics.Debug.Print($"{moduleNames} modules init cost {stopwatch.ElapsedMilliseconds} milliseconds.");
#endif
            }
        }
        public async Task InitializeAsync(IContainerProvider containerProvider)
        {
            if (!IsResisted || IsLoaded)
            {
                return;
            }

            IsLoaded = true;

            List<IModuleInfo> loadList = new List<IModuleInfo>();

            if (this.modules.Count > 0)
            {
                loadList.AddRange(this.modules);
            }

            if (loadList.Count != 0)
            {
                Task[] tasks = new Task[loadList.Count];
#if DEBUG
                System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
                System.Text.StringBuilder moduleNames = new System.Text.StringBuilder();
#endif
                for (int i = 0; i < loadList.Count; i++)
                {
                    tasks[i] = loadList[i].OnInitializeAsync(containerProvider);
#if DEBUG
                    moduleNames.Append($"{loadList[i].GetType().FullName}, ");
#endif
                }

                await Task.WhenAll(tasks);
#if DEBUG
                stopwatch.Stop();
                System.Diagnostics.Debug.Print($"{moduleNames} modules init cost {stopwatch.ElapsedMilliseconds} milliseconds.");
#endif
            }
        }
        public async Task UninitializeAsync(IContainerProvider containerProvider)
        {
            if (!IsLoaded)
            {
                return;
            }

            IsLoaded = false;

            List<IModuleInfo> loadList = new List<IModuleInfo>();

            if (this.modules.Count > 0)
            {
                loadList.AddRange(this.modules);
            }

            if (loadList.Count != 0)
            {
                Task[] tasks = new Task[loadList.Count];

                for (int i = 0; i < loadList.Count; i++)
                {
                    tasks[i] = loadList[i].OnUninitializedAsync(containerProvider);
                }

                await Task.WhenAll(tasks);
            }
        }

        public void AddFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            AddFile(new FileInfo(path));
        }
        public void AddFile(FileInfo fileInfo)
        {
            if (CanAdd(fileInfo))
            {
                DoAddModule(fileInfo);
            }
        }
        public void AddFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            AddFolder(new DirectoryInfo(path));
        }
        public void AddFolder(DirectoryInfo directoryInfo)
        {
            if (directoryInfo != null && directoryInfo.Exists)
            {
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    AddFile(fileInfo);
                }
            }
        }
        private bool CanAdd(FileInfo fileInfo)
        {
            string extension = fileInfo.Extension.ToLower();
            return fileInfo != null
                && fileInfo.Exists
                &&
                (extension.Contains("dll") || extension.Contains("exe"));
        }
        private void DoAddModule(FileInfo fileInfo)
        {
            Assembly? assembly;

            try
            {
                assembly = Assembly.LoadFrom(fileInfo.FullName);
            }
            catch
            {
                return;
            }

            Type? moduleType = null;

            string? fullName = typeof(IModuleInfo).FullName;

            if (string.IsNullOrEmpty(fullName))
            {
                return;
            }

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterface(fullName) != null)
                {
                    moduleType = type;
                    break;
                }
            }

            if (moduleType == null ||
                string.IsNullOrEmpty(moduleType.FullName))
            {
                return;
            }

            if (assembly.CreateInstance(moduleType.FullName) is IModuleInfo module)
            {
                this.modules.Add(module);
            }
        }

        public void AddModule(IModuleInfo moduleInfo)
        {
            this.modules.Add(moduleInfo);
        }
    }
}
