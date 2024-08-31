using System.Reflection;

namespace nthLink.SDK.Model
{
    public class DelayAction<T>
    {
        public int DelayMilliseconds { get; set; } = 200;
        private readonly WeakReference<object>? weakReference;
        private readonly Timer timer;
        private readonly MethodInfo methodInfo;
        private T? param;
        public DelayAction(Action<T> action)
        {
            this.methodInfo = action.Method;
            if (action.Target != null)
            {
                this.weakReference = new WeakReference<object>(action.Target);
            }
            this.timer = new Timer(OnTick);
        }

        private void OnTick(object? state)
        {
            CancelAction();

            if (this.weakReference != null &&
                this.weakReference.TryGetTarget(out object? target))
            {
                if (this.param == null)
                {
                    methodInfo.Invoke(target, null);
                }
                else
                {
                    methodInfo.Invoke(target, new object[] { this.param });
                }
            }
        }

        public void DoAction(T param)
        {
            this.param = param;
            CancelAction();
            this.timer.Change(DelayMilliseconds, Timeout.Infinite);
        }

        public void CancelAction()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
    public class DelayAction
    {
        public int DelayMilliseconds { get; set; } = 200;
        private readonly WeakReference<object>? weakReference;
        private readonly Timer timer;
        private readonly MethodInfo methodInfo;
        public DelayAction(Action action)
        {
            this.methodInfo = action.Method;
            if (action.Target != null)
            {
                this.weakReference = new WeakReference<object>(action.Target);
            }
            this.timer = new Timer(OnTick);
        }

        private void OnTick(object? state)
        {
            CancelAction();

            if (this.weakReference != null && 
                this.weakReference.TryGetTarget(out object? target))
            {
                methodInfo.Invoke(target, null);
            }
        }

        public void DoAction()
        {
            CancelAction();
            this.timer.Change(DelayMilliseconds, Timeout.Infinite);
        }

        public void CancelAction()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
