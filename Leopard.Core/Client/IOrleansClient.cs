using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Core.Client
{
    public interface IOrleansClient
    {
        /// <summary>
        /// 获取连接状态
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 获取集群链接客户端
        /// </summary>
        /// <returns></returns>
        IClusterClient GetClusterClient();

        /// <summary>
        /// 链接客户端
        /// </summary>
        /// <param name="retryFilter">过滤</param>
        /// <returns></returns>
        Task Connect(Func<Exception, Task<bool>> retryFilter = null);

        /// <summary>
        /// 停止客户端链接
        /// </summary>
        void Close();
    }
}
