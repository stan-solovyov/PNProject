using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Messages;

namespace BLL.Services.ProductMessageService
{
    public interface IProductMessageService
    {
        void SendProduct(ProductMessage message);
    }
}
