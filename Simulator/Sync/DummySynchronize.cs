using System.ComponentModel;

namespace railway_monitor.Simulator.Sync
{
    public class DummySynchronize : ISynchronizeInvoke
    {
        private readonly object _sync;

        public DummySynchronize() {
            _sync = new object();
        }


        public IAsyncResult BeginInvoke(Delegate method, object[] args) {
            var result = new SimpleAsyncResult();


            ThreadPool.QueueUserWorkItem(delegate {
                result.AsyncWaitHandle = new ManualResetEvent(false);
                try {
                    result.AsyncState = Invoke(method, args);
                }
                catch (Exception exception) {
                    result.Exception = exception;
                }
                result.IsCompleted = true;
            });


            return result;
        }


        public object EndInvoke(IAsyncResult result) {
            if (!result.IsCompleted) {
                result.AsyncWaitHandle.WaitOne();
            }


            return result.AsyncState;
        }


        public object Invoke(Delegate method, object[] args) {
            lock (_sync) {
                return method.DynamicInvoke(args);
            }
        }


        public bool InvokeRequired {
            get { return true; }
        }
    }
}
