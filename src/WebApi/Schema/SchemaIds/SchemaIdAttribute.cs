namespace Nova.Identity.Schema
{
    public class SchemaIdAttribute : Attribute
    {
        public string SchemaId { get; }

        public SchemaIdAttribute(string schemaId)
        {
            SchemaId = schemaId;
        }
    }
}