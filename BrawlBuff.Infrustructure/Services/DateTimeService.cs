using BrawlBuff.Application.Common.Interfaces;

namespace BrawlBuff.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}