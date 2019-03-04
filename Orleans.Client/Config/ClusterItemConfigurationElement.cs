using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    public class ClusterItemConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 集群编号
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
        public string ClusterName
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// 集群服务类型
        /// </summary>
        [ConfigurationProperty("assemblyName", DefaultValue = "", IsRequired = true)]
        public string ClusterAssemblyName
        {
            get
            {
                return this["assemblyName"].ToString();
            }
            set
            {
                this["assemblyName"] = value;
            }
        }


        /// <summary>
        /// AdoNet集群配置
        /// </summary>
        [ConfigurationProperty("adoNetClustering")]
        public AdoNetClusteringConfigurationElement AdoNetClustering
        {
            get
            {
                return this["adoNetClustering"] as AdoNetClusteringConfigurationElement;
            }
            set
            {
                this["adoNetClustering"] = value;
            }
        }
    }
}
