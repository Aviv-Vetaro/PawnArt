using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PawnArt.DataAccess
{
    internal class TickDateTimeOffsetConverter : ValueConverter<DateTimeOffset, long>
    {
        public TickDateTimeOffsetConverter() : base(time => time.Ticks, ticks => new DateTimeOffset(ticks, new TimeSpan())) { }
    }
}
