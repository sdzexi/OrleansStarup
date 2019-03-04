using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.ClientCore.Config
{
    public class ClusterConfigOptions
    {
        /// <summary>
        /// 集群名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务所在的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// ado数据库集群配置
        /// </summary>
        public AdoNetClusteringConfigOptions AdoNetClustering { get; set; }
    }
}
