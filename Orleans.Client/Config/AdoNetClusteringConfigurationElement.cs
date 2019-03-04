using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    public class AdoNetClusteringConfigurationElement : ConfigurationElement
    {

        /// <summary>
        /// 集群连接字符串
        /// </summary>
        [ConfigurationProperty("connectionString", DefaultValue = "", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["connectionString"].ToString();
            }
            set
            {
                this["connectionString"] = value;
            }
        }

        /// <summary>
        /// 使用的处理程序类型
        /// </summary>
        [ConfigurationProperty("invariant", DefaultValue = "", IsRequired = true)]
        public string Invariant
        {
            get
            {
                return this["invariant"].ToString();
            }
            set
            {
                this["invariant"] = value;
            }
        }
    }
}
