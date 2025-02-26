using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MemoryBox.WebAPI.Filters
{
    public class OptionalArraySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Type == "array")
            {
                schema.Nullable = true; // Cho phép giá trị null
            }
        }
    }
}
