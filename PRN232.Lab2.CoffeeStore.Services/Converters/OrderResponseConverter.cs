using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Converters
{
    public class OrderResponseConverter : JsonConverter<OrderResponse>
    {
        public override OrderResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization is not supported for OrderResponse");
        }

        public override void Write(Utf8JsonWriter writer, OrderResponse value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Nếu không có SelectedFields hoặc rỗng -> serialize tất cả fields
            if (value.SelectedFields == null || !value.SelectedFields.Any())
            {
                WriteAllFields(writer, value, options);
            }
            else
            {
                WriteSelectedFields(writer, value, options);
            }

            writer.WriteEndObject();
        }

        private void WriteAllFields(Utf8JsonWriter writer, OrderResponse value, JsonSerializerOptions options)
        {
            writer.WriteNumber("orderId", value.OrderId);
            writer.WriteString("orderDate", value.OrderDate.ToString("HH:mm dd-MM-yyyy"));

            if (!string.IsNullOrEmpty(value.Status))
                writer.WriteString("status", value.Status);

            //if (!string.IsNullOrEmpty(value.UserId))
            //    writer.WriteString("userId", value.UserId);

            if (value.UserResponse != null)
            {
                writer.WritePropertyName("userResponse");
                JsonSerializer.Serialize(writer, value.UserResponse, options);
            }

            if (value.PaymentResponse != null)
            {
                writer.WritePropertyName("paymentResponse");
                JsonSerializer.Serialize(writer, value.PaymentResponse, options);
            }

            if (value.OrderDetailResponses != null)
            {
                writer.WritePropertyName("orderDetailResponses");
                JsonSerializer.Serialize(writer, value.OrderDetailResponses, options);
            }
        }

        private void WriteSelectedFields(Utf8JsonWriter writer, OrderResponse value, JsonSerializerOptions options)
        {
            if (value.SelectedFields.Contains("orderid"))
                writer.WriteNumber("orderId", value.OrderId);

            if (value.SelectedFields.Contains("orderdate"))
                writer.WriteString("orderDate", value.OrderDate.ToString("HH:mm dd-MM-yyyy"));

            if (value.SelectedFields.Contains("status") && !string.IsNullOrEmpty(value.Status))
                writer.WriteString("status", value.Status);

            //if (value.SelectedFields.Contains("userid") && !string.IsNullOrEmpty(value.UserId))
            //    writer.WriteString("userId", value.UserId);

            if ((value.SelectedFields.Contains("user") || value.SelectedFields.Contains("userresponse")) && value.UserResponse != null)
            {
                writer.WritePropertyName("userResponse");
                JsonSerializer.Serialize(writer, value.UserResponse, options);
            }

            if ((value.SelectedFields.Contains("payment") || value.SelectedFields.Contains("paymentresponse")) && value.PaymentResponse != null)
            {
                writer.WritePropertyName("paymentResponse");
                JsonSerializer.Serialize(writer, value.PaymentResponse, options);
            }

            if ((value.SelectedFields.Contains("orderdetails") || value.SelectedFields.Contains("orderdetailresponses")) && value.OrderDetailResponses != null)
            {
                writer.WritePropertyName("orderDetailResponses");
                JsonSerializer.Serialize(writer, value.OrderDetailResponses, options);
            }
        }
    }
}
