using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWorkFlow
{
    public class ExchangeRate
    {
        public decimal Value { get; set; }

        public ExchangeRate(decimal value)
        {
            Value = value;
        }
    }
}
