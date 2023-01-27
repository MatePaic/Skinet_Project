using Core.Entities.OrderAggregate;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecifiaction : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecifiaction(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecifiaction(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
