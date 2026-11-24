using Core.Entitys;
using Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Data.Config;

public static class OrderConfigure
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Order)))
            return;

        BsonClassMap.RegisterClassMap<Order>(map =>
        {
            map.AutoMap();

            map.SetIgnoreExtraElements(true);

            map.GetMemberMap(_ => _.OrderId)
               .SetSerializer(new GuidSerializer(BsonType.String));

            map.GetMemberMap(_ => _.UserId)
               .SetSerializer(new GuidSerializer(BsonType.String));

            map.GetMemberMap(_ => _.Status)
               .SetSerializer(new EnumSerializer<OrderStatus>(BsonType.String));

            map.GetMemberMap(_ => _.SubTotal)
               .SetSerializer(new DecimalSerializer(BsonType.Decimal128));

            map.GetMemberMap(_ => _.ShippingCost)
               .SetSerializer(new DecimalSerializer(BsonType.Decimal128));

            map.GetMemberMap(_ => _.Discount)
               .SetSerializer(new DecimalSerializer(BsonType.Decimal128));

            map.GetMemberMap(_ => _.CreatedAt)
               .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));

            map.GetMemberMap(_ => _.UpdatedAt)
               .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));

            map.GetMemberMap(_ => _.BillingAddress)
               .SetIgnoreIfNull(true);
        });

         BsonClassMap.RegisterClassMap<OrderItem>(map =>
         {
            map.AutoMap();

            map.GetMemberMap(x => x.ProductId)
               .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            map.GetMemberMap(x => x.ProductVariantId)
               .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            map.GetMemberMap(x => x.UnitPrice)
               .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
         });
    }
}
