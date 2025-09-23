using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PRN232.Lab2.CoffeeStore.Services
{
    public class CustomDateTimeSchemaFilter : ISchemaFilter
    {
    
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DateTime) || context.Type == typeof(DateTime?))
            {
                //schema.Type = "string";
                //schema.Format = "date-time";
                schema.Example = new OpenApiString("14:30 12-09-2025");
            }
        }
    }
}
