using System.Runtime.Serialization;

namespace OrderManagement.Core.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")] 
        PaymentReceived,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
