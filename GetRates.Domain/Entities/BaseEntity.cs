using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetRates.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id=new Guid();
            LastUpdatedOn = DateTime.Now;
            CreatedOn = DateTime.Now;
        }
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public Guid LastUpdatedBy { get; set; }
    }
}

