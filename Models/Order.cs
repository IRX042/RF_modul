using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace Nokedlik.Dnn.suti.Models
{
    [TableName("hcc_Order")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [Cacheable("Orders", CacheItemPriority.Default, 20)]
    public class Order
    {
        public int Id { get; set; }
        public Guid Bvin { get; set; }
        public Guid? AffiliateId { get; set; }
        public string BillingAddress { get; set; }
        public string CustomProperties { get; set; }
        public decimal FraudScore { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal HandlingTotal { get; set; }
        public string Instructions { get; set; }
        public bool IsPlaced { get; set; }
        public DateTime LastUpdated { get; set; }
        public string OrderDiscounts { get; set; }
        public string OrderDiscountDetails { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAddress { get; set; }
        public int? ShippingMethodId { get; set; }
        public string ShippingMethodDisplayName { get; set; }
        public int? ShippingProviderId { get; set; }
        public string ShippingProviderServiceCode { get; set; }
        public string ShippingStatus { get; set; }
        public decimal ShippingTotal { get; set; }
        public string ShippingDiscounts { get; set; }
        public string ShippingDiscountDetails { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public int StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ThirdPartyOrderId { get; set; }
        public int StoreId { get; set; }
        public decimal ItemsTax { get; set; }
        public decimal ShippingTax { get; set; }
        public decimal ShippingTaxRate { get; set; }
        public decimal AdjustedShippingTotal { get; set; }
        public string UserDeviceType { get; set; }
        public bool IsAbandonedEmailSent { get; set; }
        public bool IsRecurring { get; set; }
        public string UsedCulture { get; set; }
    }
}
