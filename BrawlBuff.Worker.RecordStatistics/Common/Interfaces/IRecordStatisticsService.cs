using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Worker.RecordStatistics
{
    internal interface IRecordStatisticsService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
