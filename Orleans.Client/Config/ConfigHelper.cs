using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    public class ConfigHelper
    {
        public ConfigHelper()
        {
            this.OrleansCollection = LoadConfig();
        }

        public ClusterItemConfigurationElement GetItem(string ClusterId)
        {
            foreach (string item in this.OrleansCollection.ClusterConfigurationElement.KeyValues.AllKeys)
            {
                ClusterItemConfigurationElement cluster =  this.OrleansCollection.ClusterConfigurationElement.KeyValues[item];
                if (cluster?.ClusterName == ClusterId)
                {
                    return cluster;
                }
            }

            throw new Exception("没有找到配置的集群信息。");
        }

        public ClusterItemConfigurationElement GetItem(Type tInterface)
        {
            string clusterId = tInterface.Assembly.GetName().Name;
            if (string.IsNullOrWhiteSpace(clusterId))
            {
                throw new Exception($"无法获取{tInterface}的程序集名称.");
            }

            foreach (string item in this.OrleansCollection.ClusterConfigurationElement.KeyValues.AllKeys)
            {
                ClusterItemConfigurationElement cluster = this.OrleansCollection.ClusterConfigurationElement.KeyValues[item];
                if (cluster?.ClusterAssemblyName == clusterId)
                {
                    return cluster;
                }
            }

            throw new Exception("没有找到配置的集群信息。");

        }


        public ClusterConfigurationElement GetClusterConfiguration()
        {
            return this.OrleansCollection.ClusterConfigurationElement;
        }

        OrleansConfigurationSection LoadConfig()
        {
            OrleansConfigurationSection collection = ConfigurationManager.GetSection("orleans") as OrleansConfigurationSection;

            if (collection == null || collection.ClusterConfigurationElement == null || string.IsNullOrWhiteSpace(collection.ClusterConfigurationElement.DefaultCluster) ||collection.ClusterConfigurationElement.KeyValues?.Count < 1)
            {
                throw new Exception("没有找到Orleans的配置信息或者配置信息不完整。");
            }

            return collection;
        }

        readonly OrleansConfigurationSection OrleansCollection;
    }
}
