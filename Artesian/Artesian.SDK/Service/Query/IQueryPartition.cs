using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Service
{
    public interface IQueryPartition
    {
        IEnumerable<IEnumerable<T>> Partition<T>(IEnumerable<T> parameters, int batchSize);
    }
}
