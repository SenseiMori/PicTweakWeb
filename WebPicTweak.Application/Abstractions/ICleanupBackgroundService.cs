using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Application.Abstractions
{
    public interface ICleanupBackgroundService
    {
        ValueTask EnqueueAsync(FolderJobDTO job, CancellationToken ct);
        ValueTask<FolderJobDTO> DequeueAsync(CancellationToken ct);
    }
}
