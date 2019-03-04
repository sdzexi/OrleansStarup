using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.ClientCore.Config
{
    public class OrleansClusterConfigOptions
    {
        /// <summary>
        /// 默认集群
        /// </summary>
        public string DefaultCluster { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 集群配置
        /// </summary>
        public List<ClusterConfigOptions> Cluster { get; set; }
    }
}
