using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange
{
    class ExchangeRate
    {
        public decimal Value{ get; set;}

        public ExchangeRate(string value)
        {
            Value = Convert.ToDecimal(value);
        }
    }
}
