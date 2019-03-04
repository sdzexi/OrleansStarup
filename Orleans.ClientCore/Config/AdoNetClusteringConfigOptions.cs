using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.ClientCore.Config
{
    public class AdoNetClusteringConfigOptions
    {
        /// <summary>
        /// Ado数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库处理程序
        /// </summary>
        public string Invariant { get; set; }
    }
}
