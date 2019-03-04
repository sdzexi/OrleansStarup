using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Config
{
    [ConfigurationCollection(typeof(ClusterItemConfigurationElement))]
    public class OrleansConfigurationSectionCollection : ConfigurationElementCollection
    {
        new public ClusterItemConfigurationElement this[string name]
        {
            get
            {
                return (ClusterItemConfigurationElement)base.BaseGet(name);
            }
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        public IEnumerable<string> AllKeys { get { return BaseGetAllKeys().Cast<string>(); } }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ClusterItemConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClusterItemConfigurationElement)element).ClusterName;
        }
    }
}
