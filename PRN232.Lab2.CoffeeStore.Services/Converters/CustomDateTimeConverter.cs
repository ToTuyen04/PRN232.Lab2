using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateTimeFormat = "HH:mm dd-MM-yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (DateTime.TryParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }

            //if custom format fails
            if (DateTime.TryParse(value, out var fallbackResult))
            {
                return fallbackResult;
            }
            throw new JsonException($"Unable to parse '{value}' as DateTime. Expected format: '{DateTimeFormat}'");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}
