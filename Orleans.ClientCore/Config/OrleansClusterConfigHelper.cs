using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Orleans.ClientCore.Config
{
    public class OrleansClusterConfigHelper
    {
        readonly OrleansClusterConfigOptions _Options;
        public OrleansClusterConfigHelper(IOptions<OrleansClusterConfigOptions> clusterConfigOptions)
        {
            if (clusterConfigOptions?.Value == null)
            {
                throw new Exception($"没有找到指定的集群配置信息");
            }

            this._Options = clusterConfigOptions.Value;
        }

        /// <summary>
        /// 获取集群配置
        /// </summary>
        public OrleansClusterConfigOptions ClusterConfig
        {
            get
            {
                return this._Options;
            }
        }

        /// <summary>
        /// 获取集群信息
        /// </summary>
        /// <param name="clusterID">集群编号</param>
        /// <returns></returns>
        public ClusterConfigOptions GetCluster(string clusterID = default(string))
        {
            //如果集群编号为空则使用默认的集群编号
            clusterID = string.IsNullOrWhiteSpace(clusterID) ? this._Options.DefaultCluster : clusterID;

            if (string.IsNullOrWhiteSpace(clusterID))
            {
                throw new Exception("没有找到相应的集群信息");
            }

            ClusterConfigOptions configOptions = this._Options.Cluster.LastOrDefault(a => a.Name == clusterID);

            if (configOptions == null)
            {
                throw new Exception($"没有找到相应的集群信息:{clusterID}");
            }

            return configOptions;
        }

        /// <summary>
        /// 获取集群信息
        /// </summary>
        /// <param name="tInterface">服务接口类型</param>
        /// <returns></returns>
        public ClusterConfigOptions GetCluster(Type tInterface)
        {
            string clusterId = tInterface.Assembly.GetName().Name;
            if (string.IsNullOrWhiteSpace(clusterId))
            {
                throw new Exception($"无法获取{tInterface}的程序集名称.");
            }

            ClusterConfigOptions configOptions = this._Options.Cluster.LastOrDefault(a => a.AssemblyName == clusterId);

            if (configOptions == null)
            {
                throw new Exception($"没有找到相应的集群信息:{clusterId}");
            }

            return configOptions;
        }

    }  
}
