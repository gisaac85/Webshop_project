using Core.Dtos;
using Core.Entities.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models.ViewModels
{
    public class OrderVM
    {
        public IReadOnlyList<DeliveryMethod> DeliveryMethods { get; set; }
        public OrderToReturnDto OrderToReturnDTO { get; set; }
    }
}
