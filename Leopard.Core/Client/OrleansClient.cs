using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Core.Client
{
    internal class OrleansClient : IOrleansClient
    {
        readonly IClusterClient ClusterClient;

        const int initializeAttemptsBeforeFailing = 5;

        public bool IsInitialized => this.ClusterClient.IsInitialized;

        public OrleansClient(IClusterClient clusterClient)
        {
            this.ClusterClient = clusterClient ?? throw new OrleansException("为实现的集群客户端！");
        }

        public OrleansClient(Func<IClusterClient> clusterClient)
        {
            this.ClusterClient = clusterClient() ?? throw new OrleansException("为实现的集群客户端！");
        }

        #region 接口实现
        public void Close()
        {
            this.ClusterClient.Close();
        }

        public async Task Connect(Func<Exception, Task<bool>> retryFilter = null)
        {
            await Task.Run(async()=> {
                if (retryFilter == null)
                {
                    int attempt = 0;
                    retryFilter = async (Exception exception) =>
                    {
                        if (exception.GetType() != typeof(SiloUnavailableException))
                        {
                            Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                            return false;
                        }
                        attempt++;
                        Console.WriteLine($"Cluster client attempt {attempt} of {initializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
                        if (attempt > initializeAttemptsBeforeFailing)
                        {
                            return false;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(4));
                        return true;
                    };
                }

                await this.ClusterClient.Connect(retryFilter);
            }).ConfigureAwait(false);
           
        }

        public IClusterClient GetClusterClient()
        {
            return this.ClusterClient; 
        }
        #endregion
    }
}
