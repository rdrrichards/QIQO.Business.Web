using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using QIQO.Business.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QIQO.Business.Services
{
    public class EntityService : IEntityService
    {
        private readonly IServiceFactory _service_fact;
        public EntityService(IServiceFactory services)
        {
            _service_fact = services;
        }
        public Order Map(OrderViewModel order_vm)
        {
            IAccountService account_proxy = _service_fact.CreateClient<IAccountService>();
            IOrderService order_proxy = _service_fact.CreateClient<IOrderService>();
            IEmployeeService employee_service = _service_fact.CreateClient<IEmployeeService>();

            Account account = account_proxy.GetAccountByCode(order_vm.AccountCode, "TCF");
            int acct_cnt_key = account.Employees.Where(ct => ct.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault().EntityPersonKey;

            List<Representative> account_rep_list = new List<Representative>(employee_service.GetAccountRepsByCompany(1));
            List<Representative> sales_rep_list = new List<Representative>(employee_service.GetSalesRepsByCompany(1));

            Order order = new Order()
            {
                OrderKey = order_vm.OrderKey,
                OrderEntryDate = order_vm.OrderEntryDate,
                AccountKey = account.AccountKey,
                AccountContactKey = acct_cnt_key,
                //OrderNumber = order_vm.OrderNum,
                OrderCompleteDate = order_vm.OrderCompleteDate,
                OrderItemCount = order_vm.OrderItemCount,
                OrderValueSum = order_vm.OrderValueSum,
                OrderStatusDate = order_vm.OrderStatusDate,
                OrderShipDate = order_vm.OrderShipDate,
                //OrderStatus = (QIQOOrderStatus)order_vm.OrderStatus,
                DeliverByDate = order_vm.OrderDeliverByDate,

                AccountRepKey = account_rep_list[0].EntityPersonKey,
                SalesRepKey = sales_rep_list[0].EntityPersonKey
            };

            foreach (OrderItemViewModel item in order_vm.OrderItems)
            {
                order.OrderItems.Add(Map(item));
            }

            order.OrderItemCount = order_vm.OrderItems.Sum(item => item.Quantity);
            order.OrderValueSum = order_vm.OrderItems.Sum(item => item.OrderLineTotal);
            //int order_header_key = order_proxy.CreateOrder(order);

            return order;
        }

        public OrderViewModel Map(Order order)
        {
            //IAccountService account_proxy = _service_fact.CreateClient<IAccountService>();
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderViewModel order_data = new OrderViewModel()
            {
                OrderKey = order.OrderKey,
                OrderEntryDate = order.OrderEntryDate,
                AccountCode = order.Account.AccountCode,
                //AccountKey = order.Account.AccountKey,
                Account = Map(order.Account),
                //AccountContactName = order.Account.Employees.Where(item => item.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault().PersonFullNameFML,
                OrderNumber = order.OrderNumber,
                OrderCompleteDate = order.OrderCompleteDate,
                OrderItemCount = order.OrderItemCount,
                OrderValueSum = order.OrderValueSum,
                OrderStatusDate = order.OrderStatusDate,
                OrderShipDate = order.OrderShipDate,
                OrderStatus = order.OrderStatus.ToString(),
                OrderDeliverByDate = order.DeliverByDate,
                AccountRepName = order.AccountRep.PersonFullNameFML,
                SalesRepName = order.SalesRep.PersonFullNameFML
            };

            AccountPerson contact = order.Account.Employees.Where(item => item.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault();
            if (contact != null)
                order_data.AccountContactName = contact.PersonFullNameFML;

            return order_data;
        }

        public OrderItem Map(OrderItemViewModel order_item_vm)
        {
            IAddressService address_proxy = _service_fact.CreateClient<IAddressService>();
            IProductService product_proxy = _service_fact.CreateClient<IProductService>();
            IOrderService order_proxy = _service_fact.CreateClient<IOrderService>();
            IEmployeeService employee_service = _service_fact.CreateClient<IEmployeeService>();
            ITypeService type_service = _service_fact.CreateClient<ITypeService>();

            OrderItem orderItem = new OrderItem()
            {
                OrderItemKey = order_item_vm.OrderItemKey,
                OrderKey = order_item_vm.OrderKey,
                OrderItemSeq = order_item_vm.OrderItemSeq,
                ProductKey = order_item_vm.ProductKey,
                ProductName = order_item_vm.ProductName,
                ProductDesc = order_item_vm.ProductDesc,
                OrderItemQuantity = order_item_vm.Quantity,
                //OrderItemShipToAddress = order_item_data.ShiptoAddrKey,
                OrderItemShipToAddress = address_proxy.GetAddress(order_item_vm.OrderItemShipAddress.AddressKey),
                //OrderItemBillToAddress = order_item_data.BilltoAddrKey,
                OrderItemBillToAddress = address_proxy.GetAddress(order_item_vm.OrderItemBillAddress.AddressKey),
                //OrderItemShipDate = order_item_vm.OrderItemShipDate,
                //OrderItemCompleteDate = order_item_vm.OrderItemCompleteDate,
                ItemPricePer = order_item_vm.OrderItemPrice,
                OrderItemLineSum = order_item_vm.OrderLineTotal,
                OrderItemStatus = (QIQOOrderItemStatus)type_service.GetOrderItemStatusList()
                    .Where(key => key.OrderItemStatusName == order_item_vm.OrderItemStatus)
                    .FirstOrDefault().OrderItemStatusKey,
                //Product
                OrderItemProduct = product_proxy.GetProduct(order_item_vm.ProductKey),

                //Account Rep
                AccountRep = employee_service.GetAccountRepsByCompany(1)[0],
                //Sales Rep
                SalesRep = employee_service.GetSalesRepsByCompany(1)[0]
            };

            return orderItem;
        }

        public OrderItemViewModel Map(OrderItem order_item)
        {
            //Console.WriteLine("MapOrderItemToOrderItemData begin");
            OrderItemViewModel order_item_data = new OrderItemViewModel()
            {
                OrderItemKey = order_item.OrderItemKey,
                OrderKey = order_item.OrderKey,
                OrderItemSeq = order_item.OrderItemSeq,
                ProductKey = order_item.ProductKey,
                ProductName = order_item.ProductName,
                ProductDesc = order_item.ProductDesc,
                Quantity = order_item.OrderItemQuantity,
                OrderItemShipAddress = Map(order_item.OrderItemShipToAddress),
                OrderItemBillAddress = Map(order_item.OrderItemBillToAddress),
                //OrderItemShipDate = order_item.OrderItemShipDate,
                //OrderItemCompleteDate = order_item.OrderItemCompleteDate,
                OrderItemPrice = order_item.ItemPricePer,
                OrderLineTotal = order_item.OrderItemLineSum,
                OrderItemStatus = order_item.OrderItemStatus.ToString(),

                //Account Rep
                AccountRepName = order_item.AccountRep.PersonFullNameFML,
                SalesRepName = order_item.SalesRep.PersonFullNameFML
            };
            //Console.WriteLine("MapOrderItemToOrderItemData");
            return order_item_data;
        }

        public InvoiceItem Map(InvoiceItemViewModel invoice_item_vm)
        {
            IAddressService address_proxy = _service_fact.CreateClient<IAddressService>();
            IProductService product_proxy = _service_fact.CreateClient<IProductService>();
            IInvoiceService invoice_proxy = _service_fact.CreateClient<IInvoiceService>();
            IEmployeeService employee_service = _service_fact.CreateClient<IEmployeeService>();
            ITypeService type_service = _service_fact.CreateClient<ITypeService>();

            InvoiceItem orderItem = new InvoiceItem()
            {
                InvoiceItemKey = invoice_item_vm.InvoiceItemKey,
                InvoiceKey = invoice_item_vm.InvoiceKey,
                InvoiceItemSeq = invoice_item_vm.InvoiceItemSeq,
                ProductKey = invoice_item_vm.ProductKey,
                ProductName = invoice_item_vm.ProductName,
                ProductDesc = invoice_item_vm.ProductDesc,
                InvoiceItemQuantity = invoice_item_vm.Quantity,
                //InvoiceItemShipToAddress = order_item_data.ShiptoAddrKey,
                OrderItemShipToAddress = address_proxy.GetAddress(invoice_item_vm.InvoiceItemShipAddress.AddressKey),
                //InvoiceItemBillToAddress = order_item_data.BilltoAddrKey,
                OrderItemBillToAddress = address_proxy.GetAddress(invoice_item_vm.InvoiceItemBillAddress.AddressKey),
                //InvoiceItemShipDate = invoice_item_vm.InvoiceItemShipDate,
                //InvoiceItemCompleteDate = invoice_item_vm.InvoiceItemCompleteDate,
                ItemPricePer = invoice_item_vm.InvoiceItemPrice,
                InvoiceItemLineSum = invoice_item_vm.InvoiceLineTotal,
                InvoiceItemStatus = (QIQOInvoiceItemStatus)type_service.GetInvoiceItemStatusList()
                    .Where(key => key.InvoiceItemStatusName == invoice_item_vm.InvoiceItemStatus)
                    .FirstOrDefault().InvoiceItemStatusKey,
                //Product
                InvoiceItemProduct = product_proxy.GetProduct(invoice_item_vm.ProductKey),

                //Account Rep
                AccountRep = employee_service.GetAccountRepsByCompany(1)[0],
                //Sales Rep
                SalesRep = employee_service.GetSalesRepsByCompany(1)[0]
            };

            return orderItem;
        }

        public Invoice Map(InvoiceViewModel invoice_vm)
        {
            IAccountService account_proxy = _service_fact.CreateClient<IAccountService>();
            IInvoiceService invoice_proxy = _service_fact.CreateClient<IInvoiceService>();
            IEmployeeService employee_service = _service_fact.CreateClient<IEmployeeService>();

            Account account = account_proxy.GetAccountByCode(invoice_vm.AccountCode, "TCF");
            int acct_cnt_key = account.Employees.Where(ct => ct.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault().EntityPersonKey;

            List<Representative> account_rep_list = new List<Representative>(employee_service.GetAccountRepsByCompany(1));
            List<Representative> sales_rep_list = new List<Representative>(employee_service.GetSalesRepsByCompany(1));

            Invoice invoice = new Invoice()
            {
                InvoiceKey = invoice_vm.InvoiceKey,
                InvoiceEntryDate = invoice_vm.InvoiceEntryDate,
                AccountKey = account.AccountKey,
                AccountContactKey = acct_cnt_key,
                //InvoiceNumber = invoice_vm.InvoiceNum,
                InvoiceCompleteDate = invoice_vm.InvoiceCompleteDate,
                InvoiceItemCount = invoice_vm.InvoiceItemCount,
                InvoiceValueSum = invoice_vm.InvoiceValueSum,
                InvoiceStatusDate = invoice_vm.InvoiceStatusDate,
                OrderShipDate = invoice_vm.InvoiceShipDate,
                //InvoiceStatus = (QIQOInvoiceStatus)invoice_vm.InvoiceStatus,
                //DeliverByDate = invoice_vm.InvoiceDeliverByDate,

                AccountRepKey = account_rep_list[0].EntityPersonKey,
                SalesRepKey = sales_rep_list[0].EntityPersonKey
            };

            foreach (InvoiceItemViewModel item in invoice_vm.InvoiceItems)
            {
                invoice.InvoiceItems.Add(Map(item));
            }

            invoice.InvoiceItemCount = invoice_vm.InvoiceItems.Sum(item => item.Quantity);
            invoice.InvoiceValueSum = invoice_vm.InvoiceItems.Sum(item => item.InvoiceLineTotal);
            //int order_header_key = invoice_proxy.CreateInvoice(order);

            return invoice;
        }

        public InvoiceViewModel Map(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            InvoiceViewModel invoice_data = new InvoiceViewModel()
            {
                InvoiceKey = invoice.InvoiceKey,
                InvoiceEntryDate = invoice.InvoiceEntryDate,
                AccountCode = invoice.Account.AccountCode,
                //AccountKey = invoice.Account.AccountKey,
                Account = Map(invoice.Account),
                //AccountContactName = invoice.Account.Employees.Where(item => item.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault().PersonFullNameFML,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceCompleteDate = invoice.InvoiceCompleteDate,
                InvoiceItemCount = invoice.InvoiceItemCount,
                InvoiceValueSum = invoice.InvoiceValueSum,
                InvoiceStatusDate = invoice.InvoiceStatusDate,
                InvoiceShipDate = invoice.OrderShipDate,
                InvoiceStatus = invoice.InvoiceStatus.ToString(),
                //InvoiceDeliverByDate = invoice.OrderEntryDate,
                AccountRepName = invoice.AccountRep.PersonFullNameFML,
                SalesRepName = invoice.SalesRep.PersonFullNameFML
            };

            AccountPerson contact = invoice.Account.Employees.Where(item => item.CompanyRoleType == QIQOPersonType.AccountContact).FirstOrDefault();
            if (contact != null)
                invoice_data.AccountContactName = contact.PersonFullNameFML;

            return invoice_data;
        }

        public InvoiceItemViewModel Map(InvoiceItem invoice_item)
        {
            InvoiceItemViewModel invoice_item_data = new InvoiceItemViewModel()
            {
                InvoiceKey = invoice_item.InvoiceKey,
                InvoiceItemSeq = invoice_item.InvoiceItemSeq,
                ProductKey = invoice_item.ProductKey,
                ProductName = invoice_item.ProductName,
                ProductDesc = invoice_item.ProductDesc,
                Quantity = invoice_item.InvoiceItemQuantity,
                InvoiceItemShipAddress = Map(invoice_item.OrderItemShipToAddress),
                InvoiceItemBillAddress = Map(invoice_item.OrderItemBillToAddress),
                //InvoiceItemShipDate = invoice_item.InvoiceItemShipDate,
                //InvoiceItemCompleteDate = invoice_item.InvoiceItemCompleteDate,
                InvoiceItemPrice = invoice_item.ItemPricePer,
                InvoiceLineTotal = invoice_item.InvoiceItemLineSum,
                InvoiceItemStatus = invoice_item.InvoiceItemStatus.ToString(),

                //Account Rep
                AccountRepName = invoice_item.AccountRep.PersonFullNameFML,
                SalesRepName = invoice_item.SalesRep.PersonFullNameFML
            };
            //Console.WriteLine("MapInvoiceItemToInvoiceItemData");
            return invoice_item_data;
        }

        public AddressViewModel Map(Address address)
        {
            AddressViewModel address_data = new AddressViewModel()
            {
                AddressKey = address.AddressKey,
                //AddressTypeKey = (int)address.AddressType,
                AddressType = address.AddressType.ToString(),
                EntityKey = address.EntityKey,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                AddressCity = address.AddressCity,
                AddressState = address.AddressState,
                AddressPostal = address.AddressPostalCode,
                AddressCounty = address.AddressCounty,
                AddressCountry = address.AddressCountry,
                //AddressActiveFlg = Convert.ToInt32(address.AddressActiveFlag),
                //AddressDefaultFlg = Convert.ToInt32(address.AddressDefaultFlag),
                AddressNotes = address.AddressNotes
            };

            return address_data;
        }

        public AccountViewModel Map(Account account)
        {
            AccountViewModel account_data = new AccountViewModel()
            {
                AccountKey = account.AccountKey,
                CompanyKey = account.CompanyKey,
                //AccountTypeKey = (int)account.AccountType,
                AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                AccountDBA = account.AccountDBA,
                AccountDesc = account.AccountDesc,
                AccountStartDate = account.AccountStartDate,
                AccountEndDate = account.AccountEndDate,
                AccountLastUpdateDate = account.UpdateDateTime
            };

            return account_data;
        }

        public Account Map(AccountViewModel account_vm)
        {
            Account account_data = new Account()
            {
                AccountKey = account_vm.AccountKey,
                CompanyKey = account_vm.CompanyKey,
                //AccountTypeKey = (int)account.AccountType,
                AccountCode = account_vm.AccountCode,
                AccountName = account_vm.AccountName,
                AccountDBA = account_vm.AccountDBA,
                AccountDesc = account_vm.AccountDesc,
                AccountStartDate = account_vm.AccountStartDate,
                AccountEndDate = account_vm.AccountEndDate
            };

            return account_data;
        }

        public Product Map(ProductViewModel product_vm)
        {
            return new Product()
            {
                ProductKey = product_vm.ProductKey,
                ProductCode = product_vm.ProductCode,
                ProductName = product_vm.ProductName,
                ProductDesc = product_vm.ProductDesc,
                ProductNameShort = product_vm.ProductShortDesc,
                ProductNameLong = product_vm.ProductLongDesc,
                ProductImagePath = product_vm.ProductImagePath,
                ProductType = QIQOProductType.Sweet6
            };
        }

        public ProductViewModel Map(Product product)
        {
            return new ProductViewModel()
            {
                ProductKey = product.ProductKey,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDesc = product.ProductDesc,
                ProductShortDesc = product.ProductNameShort,
                ProductLongDesc = product.ProductNameLong,
                ProductImagePath = product.ProductImagePath,
                ProductType = product.ProductType.ToString(),
                ProductBasePrice = decimal.Parse(product.ProductAttributes.Where(att => att.AttributeType == QIQOAttributeType.Product_PRODBASE).FirstOrDefault().AttributeValue),
                ProductBaseQuantity = int.Parse(product.ProductAttributes.Where(att => att.AttributeType == QIQOAttributeType.Product_PRODDFQTY).FirstOrDefault().AttributeValue),
                ProductLastUpdated = product.UpdateDateTime
            };
        }

        public EntityAttributeViewModel Map(EntityAttribute att)
        {
            return new EntityAttributeViewModel()
            {
                AttributeKey = att.AttributeKey,
                EntityKey = att.EntityKey,
                EntityType = att.EntityType.ToString(),
                AttributeDataType = att.AttributeTypeData.AttributeTypeName,
                AttributeDisplayFormat = att.AttributeDisplayFormat,
                AttributeType = att.AttributeType.ToString(),
                AttributeValue = att.AttributeValue
            };
        }

        public AccountPersonViewModel Map(AccountPerson emp)
        {
            return new AccountPersonViewModel()
            {
                RoleInCompany = emp.RoleInCompany,
                CompanyRoleType = emp.CompanyRoleType.ToString(),
                EntityPersonKey = emp.EntityPersonKey,
                StartDate = emp.StartDate,
                EndDate = emp.EndDate,
                Comment = emp.Comment,
                PersonKey = emp.PersonKey,
                PersonCode = emp.PersonCode,
                PersonFirstName = emp.PersonFirstName,
                PersonMI = emp.PersonMI,
                PersonLastName = emp.PersonLastName
            };
        }
    }
}
