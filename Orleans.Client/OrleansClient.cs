using Leopard.Core.Client;
using Microsoft.Extensions.Logging;
using Orleans.Client.Config;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
namespace Orleans.Client
{
    /// <summary>
    /// Orleans客户端
    /// </summary>
    public class OrleansClient
    {
        private OrleansClient(string clusterId)
        {
            this.ConfigHelper = new ConfigHelper();
            ClusterConfigurationElement clusterInfo = this.ConfigHelper.GetClusterConfiguration();
            if (clusterInfo == null)
            {
                throw new Exception($"没有找到指定的集群配置信息:{(string.IsNullOrWhiteSpace(clusterId) ? "Defalut" : clusterId)}");
            }

            if (string.IsNullOrWhiteSpace(clusterId))
            {
                clusterId = clusterInfo.DefaultCluster;
            }

            ClusterItemConfigurationElement clusterItem = this.ConfigHelper.GetItem(clusterId);
           
            if (clusterItem == null || clusterInfo == null)
            {
                throw new Exception($"没有找到指定的集群配置信息:{(string.IsNullOrWhiteSpace(clusterId) ? "Defalut":clusterId)}");
            }

            this.OrleansClientFactory = new OrleansClientFactory(new ClientBuilder()
                 .Configure<ClusterOptions>(options =>
                 {
                     options.ClusterId = clusterItem.ClusterName;
                     options.ServiceId = clusterInfo.ServiceId;
                 })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = clusterItem.AdoNetClustering.Invariant;
                    options.ConnectionString = clusterItem.AdoNetClustering.ConnectionString;
                })
                .ConfigureLogging((logging) =>
                {
                    logging.ClearProviders(); //移除已经注册的其他日志处理程序
                    logging.AddNLog();
                })
                
                .Build(), clusterItem.ClusterName);
        }

        private OrleansClient(Type tInterface)
        {
            this.ConfigHelper = new ConfigHelper();
            ClusterConfigurationElement clusterInfo = this.ConfigHelper.GetClusterConfiguration();
            if (clusterInfo == null)
            {
                throw new Exception($"没有找到指定的集群配置信息");
            }

            ClusterItemConfigurationElement clusterItem = this.ConfigHelper.GetItem(tInterface);

            if (clusterItem == null || clusterInfo == null)
            {
                throw new Exception($"没有找到指定的集群配置信息:{tInterface}");
            }

            this.OrleansClientFactory = new OrleansClientFactory(new ClientBuilder()
                 .Configure<ClusterOptions>(options =>
                 {
                     options.ClusterId = clusterItem.ClusterName;
                     options.ServiceId = clusterInfo.ServiceId;
                 })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = clusterItem.AdoNetClustering.Invariant;
                    options.ConnectionString = clusterItem.AdoNetClustering.ConnectionString;
                })
                .ConfigureLogging((logging) =>
                {
                    logging.ClearProviders(); //移除已经注册的其他日志处理程序
                    logging.AddNLog();
                })

                .Build(), clusterItem.ClusterName);

        }

        /// <summary>
        /// 获取客户端连接
        /// </summary>
        /// <param name="clusterId">集群编号,传NULL读取默认的配置</param>
        /// <returns></returns>
        public static IClusterClient ClusterClientProxy(string clusterId = default(string))
        {
            _OrleansClient = new OrleansClient(clusterId);

            return _OrleansClient.OrleansClientFactory.GetClusterClient();
        }

        /// <summary>
        /// 获取客户端连接
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static IClusterClient ClusterClientProxy<TInterface>() where TInterface: IGrain
        {
            _OrleansClient = new OrleansClient(typeof(TInterface));

            return _OrleansClient.OrleansClientFactory.GetClusterClient();
        }

        public readonly IOrleansClientProvider OrleansClientFactory;

        private static object objLock = new object();

        readonly ConfigHelper ConfigHelper;

        static OrleansClient _OrleansClient { get; set; }

    }
}
