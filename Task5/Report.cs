namespace Task5
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;

    class Report
    {
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "product-id")]
        [BsonElement("product-id")]
        public int  ProductId { get; set; }

        [JsonProperty(PropertyName = "product-name")]
        [BsonElement("product-name")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "vendor-name")]
        [BsonElement("vendor-name")]
        public string VendorName { get; set; }

        [JsonProperty(PropertyName = "total-quantity-sold")]
        [BsonElement("total-quantity-sold")]
        public int TotalQuantitySold { get; set; }

        [JsonProperty(PropertyName = "total-incomes")]
        [BsonElement("total-incomes")]
        public decimal TotalIncomes { get; set; }
    }
}