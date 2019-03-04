using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Core.Hosting
{
    public class SiloEndPointOptions
    {
        /// <summary>
        /// Silo 到Silo 的端口
        /// </summary>
        public int SiloPort { get; set; } = 11111;

        /// <summary>
        /// 对外网关端口
        /// </summary>
        public int GatewayPort { get; set; } = 30000;

        /// <summary>
        /// 内部Silo网关监听端口
        /// </summary>
        public int GatewayListeningProt { get; set; }

        /// <summary>
        /// 内容Silo监听端口
        /// </summary>
        public int SiloListeningProt { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 监听Any地址
        /// </summary>
        public bool ListenOnAnyHostAddress { get; set; }
    }
}
