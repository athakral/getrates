using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetRates.Domain.Base
{
    public interface IBaseRepository<T,TId>  where TId : struct 
    {
        IQueryable<T> All { get; }
        T Find(TId id);
        void InsertOrUpdate(T feedback);
        void Delete(TId id);
    }
}
