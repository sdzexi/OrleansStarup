using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leopard.Core.Hosting
{
    public interface IOrleansHost
    {
        IOrleansHost Start(CancellationToken cancellationToken = default(CancellationToken));
        IOrleansHost Stop(CancellationToken cancellationToken = default(CancellationToken));

        void Wait();
    }
}
