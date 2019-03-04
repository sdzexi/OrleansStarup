using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    public class OrleansConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("clusterConfiguration",IsRequired = false)]
        [ConfigurationCollection(typeof(OrleansConfigurationSectionCollection), AddItemName = "add")]
        public ClusterConfigurationElement ClusterConfigurationElement
        {
            get
            {
                return this["clusterConfiguration"] as ClusterConfigurationElement;
            }
            set
            {
                this["clusterConfiguration"] = value;
            }
        }
    }
}
