using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Core.Client
{
    public interface IOrleansClientProvider
    {
        /// <summary>
        /// 获取集群客户端
        /// </summary>
        /// <returns></returns>
        IClusterClient GetClusterClient();
    }
}
