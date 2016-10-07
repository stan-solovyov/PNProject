using System;
using System.Collections.Generic;

namespace Messages
{
    [Serializable]
    public class UpdatedPricesMessage
    {
        public List<UpdatedPrice> UpdatedPricesList { get; set; }
    }
}
