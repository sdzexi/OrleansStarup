using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    public class ClusterConfigurationElement : ConfigurationElement
    {

        /// <summary>
        /// 默认集群名称
        /// </summary>
        [ConfigurationProperty("serviceId")]
        public string ServiceId
        {
            get
            {
                return this["serviceId"].ToString();
            }
            set
            {
                this["serviceId"] = value;
            }
        }

        /// <summary>
        /// 客户端名称
        /// </summary>
        [ConfigurationProperty("defaultCluster")]
        public string DefaultCluster
        {
            get
            {
                return this["defaultCluster"].ToString();
            }
            set
            {
                this["defaultCluster"] = value;
            }
        }

        [ConfigurationProperty("cluster", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(OrleansConfigurationSectionCollection), AddItemName = "add")]
        public OrleansConfigurationSectionCollection KeyValues
        {
            get { return (OrleansConfigurationSectionCollection)base["cluster"]; }
        }

    }
}
