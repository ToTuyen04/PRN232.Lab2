using System.Text.Json;
using System.Text.Json.Serialization;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;

namespace PRN232.Lab2.CoffeeStore.Services.Converters
{
    public class SelectiveProductResponseConverter : JsonConverter<ProductResponse>
    {
        public override ProductResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization not implemented");
        }

        public override void Write(Utf8JsonWriter writer, ProductResponse value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Nếu không có selected fields, serialize tất cả
            if (value.SelectedFields == null || value.SelectedFields.Count == 0)
            {
                WriteAllProperties(writer, value);
            }
            else
            {
                // Chỉ serialize những fields được select
                WriteSelectedProperties(writer, value, value.SelectedFields);
            }

            writer.WriteEndObject();
        }

        private void WriteAllProperties(Utf8JsonWriter writer, ProductResponse value)
        {
            writer.WriteNumber("productId", value.ProductId);
            writer.WriteString("name", value.Name);
            writer.WriteString("description", value.Description);
            writer.WriteNumber("price", value.Price);
            writer.WriteBoolean("isActive", value.IsActive);
            writer.WriteString("categoryName", value.CategoryName);
        }

        private void WriteSelectedProperties(Utf8JsonWriter writer, ProductResponse value, HashSet<string> selectedFields)
        {
            if (selectedFields.Contains("productid") || selectedFields.Contains("id"))
                writer.WriteNumber("productId", value.ProductId);

            if (selectedFields.Contains("name"))
                writer.WriteString("name", value.Name);

            if (selectedFields.Contains("description"))
                writer.WriteString("description", value.Description);

            if (selectedFields.Contains("price"))
                writer.WriteNumber("price", value.Price);

            if (selectedFields.Contains("isactive") || selectedFields.Contains("active"))
                writer.WriteBoolean("isActive", value.IsActive);

            if (selectedFields.Contains("categoryname") || selectedFields.Contains("category"))
                writer.WriteString("categoryName", value.CategoryName);
        }
    }
}