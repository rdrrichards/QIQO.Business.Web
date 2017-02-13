using QIQO.Business.Client.Entities;
using QIQO.Business.ViewModels.Api;

namespace QIQO.Business.Services
{
    public interface IEntityService
    {
        AccountViewModel Map(Account account);
        Account Map(AccountViewModel account_vm);
        AddressViewModel Map(Address address);
        OrderItemViewModel Map(OrderItem order_item);
        OrderItem Map(OrderItemViewModel order_item_vm);
        OrderViewModel Map(Order order);
        Order Map(OrderViewModel order_vm);
        Product Map(ProductViewModel product_vm);
        ProductViewModel Map(Product product);
        EntityAttributeViewModel Map(EntityAttribute att);
        AccountPersonViewModel Map(AccountPerson emp);


        InvoiceItemViewModel Map(InvoiceItem invoice_item);
        InvoiceItem Map(InvoiceItemViewModel invoice_item_vm);
        InvoiceViewModel Map(Invoice invoice);
        Invoice Map(InvoiceViewModel invoice_vm);
    }
}