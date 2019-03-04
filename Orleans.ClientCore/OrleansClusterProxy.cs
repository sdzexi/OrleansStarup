using Leopard.Core.Client;
using Microsoft.Extensions.Logging;
using Orleans.ClientCore.Config;
using Orleans.Configuration;
using Orleans.Hosting;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.ClientCore
{
    public class OrleansClusterProxy
    {
        readonly OrleansClusterConfigHelper _OrleansClusterConfigHelper;

        IOrleansClientProvider OrleansClientFactory;

        /// <summary>
        /// 锁
        /// </summary>
        static object _LockObj = new object();

        public OrleansClusterProxy(OrleansClusterConfigHelper orleansClusterConfig)
        {
            this._OrleansClusterConfigHelper = orleansClusterConfig;
        }

        /// <summary>
        /// 获取集群连接客户端
        /// </summary>
        /// <param name="clusterID">集群编号</param>
        /// <returns></returns>
        public IClusterClient GetClusterClient(string clusterID = default(string))
        {
            CreateClusterClientProxy(clusterID);
            return OrleansClientFactory.GetClusterClient();
        }

        /// <summary>
        /// 获取集群连接客户端
        /// </summary>
        /// <param name="clusterID">集群编号</param>
        /// <returns></returns>
        public IClusterClient GetClusterClient<TInterface>() where TInterface : IGrain
        {
            CreateClusterClientProxy<TInterface>();
            return OrleansClientFactory.GetClusterClient();
        }

        /// <summary>
        /// 创建集群代理
        /// </summary>
        void CreateClusterClientProxy(string clusterID = default(string))
        {
            ClusterConfigOptions configOptions = this._OrleansClusterConfigHelper.GetCluster(clusterID);

            this.OrleansClientFactory = new OrleansClientFactory(new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = configOptions.Name;
                    options.ServiceId = this._OrleansClusterConfigHelper.ClusterConfig.ServiceId;
                })
               .UseAdoNetClustering(options =>
               {
                   options.Invariant = configOptions.AdoNetClustering.Invariant;
                   options.ConnectionString = configOptions.AdoNetClustering.ConnectionString;
               })
               .ConfigureLogging((logging) =>
               {
                   logging.ClearProviders(); //移除已经注册的其他日志处理程序
                    logging.AddNLog();
               })
               .Build(), configOptions.Name);
        }

        void CreateClusterClientProxy<TInterface>() where TInterface : IGrain
        {
            ClusterConfigOptions configOptions = this._OrleansClusterConfigHelper.GetCluster(typeof(TInterface));

            this.OrleansClientFactory = new OrleansClientFactory(new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = configOptions.Name;
                    options.ServiceId = this._OrleansClusterConfigHelper.ClusterConfig.ServiceId;
                })
               .UseAdoNetClustering(options =>
               {
                   options.Invariant = configOptions.AdoNetClustering.Invariant;
                   options.ConnectionString = configOptions.AdoNetClustering.ConnectionString;
               })
               .ConfigureLogging((logging) =>
               {
                   logging.ClearProviders(); //移除已经注册的其他日志处理程序
                   logging.AddNLog();
               })
               .Build(), configOptions.Name);
        }
    }
}
