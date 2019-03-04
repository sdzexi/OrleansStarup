using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Leopard.Core.Client
{
    public class OrleansClientFactory : IOrleansClientProvider
    {
        private static IOrleansClient _OrleansClient;
        private static object objLock = new object();
        private static Dictionary<string, IOrleansClient> clusterClients = new Dictionary<string, IOrleansClient>();
        readonly IClusterClient _ClusterClient;
        readonly string _ClusterId;
        public OrleansClientFactory(IClusterClient clusterClient, string clusterId)
        {
            this._ClusterClient = clusterClient;
            this._ClusterId = clusterId;
           // CreateClusterClient(clusterClient, clusterId).Wait();
        }
        //~OrleansClientFactory()
        //{
        //    _OrleansClient.Close();
        //}


        /// <summary>
        /// 获取集群客户端
        /// </summary>
        /// <returns></returns>
        public IClusterClient GetClusterClient()
        {
            //IClusterClient client = _OrleansClient?.GetClusterClient();

            CreateClusterClient(this._ClusterClient, this._ClusterId);

            return _OrleansClient.GetClusterClient();

            //return client;
        }



        void CreateClusterClient(IClusterClient clusterClient, string clusterId)
        {
            IOrleansClient client = clusterClients.LastOrDefault(a => a.Key == clusterId).Value;

            if (client == null || !client.IsInitialized)
            {
                try
                {
                    lock (objLock)
                    {
                        _OrleansClient = new OrleansClient(clusterClient);
                        _OrleansClient.Connect().Wait();

                        if (clusterClients.ContainsKey(clusterId))
                        {
                            clusterClients[clusterId] = _OrleansClient;
                        }
                        else
                        {
                            clusterClients.Add(clusterId, _OrleansClient);
                        }
                    }
                }
                catch (Exception)
                {
                }
              
            }
            else
            {
                _OrleansClient = client;
            }
        }


    }
}
