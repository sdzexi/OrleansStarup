using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leopard.Core.Hosting
{
    public class SiloHost : IOrleansHost
    {
         readonly ManualResetEvent _siloStopped = new ManualResetEvent(false);

         bool _siloStopping = false;
         readonly object syncLock = new object();

        public SiloHost(ISiloHost siloHost)
        {
            this._SiloHost = siloHost;
            SetupApplicationShutdown();
        }

        public ISiloHost _SiloHost { get; set; }

        public IOrleansHost Start(CancellationToken cancellationToken = default(CancellationToken))
        {
            this._SiloHost.StartAsync(cancellationToken).Wait();
           
            return (IOrleansHost)this;
        }

        public IOrleansHost Stop(CancellationToken cancellationToken = default(CancellationToken))
        {
           this._SiloHost.StopAsync(cancellationToken).Wait();
            return (IOrleansHost)this;
        }

        public void Wait()
        {
            _siloStopped.WaitOne();
        }


        void SetupApplicationShutdown()
        {
            /// Capture the user pressing Ctrl+C
            Console.CancelKeyPress += (s, a) => {
                /// Prevent the application from crashing ungracefully.
                a.Cancel = true;
                /// Don't allow the following code to repeat if the user presses Ctrl+C repeatedly.
                lock (syncLock)
                {
                    if (!_siloStopping)
                    {
                        _siloStopping = true;
                        Task.Run(StopSilo).Ignore();
                    }
                }
                /// Event handler execution exits immediately, leaving the silo shutdown running on a background thread,
                /// but the app doesn't crash because a.Cancel has been set = true
            };
        }

        Task StopSilo()
        {
            this.Stop();
            _siloStopped.Set();
            return Task.CompletedTask;
        }

    }
}
