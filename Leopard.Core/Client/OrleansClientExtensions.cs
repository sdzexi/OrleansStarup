using Leopard.Core.Client;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orleans
{
    public static class OrleansClientExtensions
    {
        /// <summary>
        /// 获取Grain服务
        /// </summary>
        /// <typeparam name="TGrainInterface"></typeparam>
        /// <param name="orleansClient"></param>
        /// <param name="grainClassNamePrefix"></param>
        /// <returns></returns>

        public static TGrainInterface GetGrain<TGrainInterface>(this IClusterClient orleansClient, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            return orleansClient.GetGrain<TGrainInterface>(Guid.NewGuid().ToString("N"), grainClassNamePrefix);
        }

    }
}
