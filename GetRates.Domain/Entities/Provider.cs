using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetRates.Domain.Entities
{
    public class Provider : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal Rate { get; set; }
        public decimal Fee { get; set; }
        public decimal Deductions { get; set; }
    }
}
