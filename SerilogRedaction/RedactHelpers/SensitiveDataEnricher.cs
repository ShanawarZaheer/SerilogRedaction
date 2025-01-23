namespace SerilogRedaction.RedactHelpers
{
    public class SensitiveDataEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var property in logEvent.Properties)
            {
                if (property.Value is ScalarValue scalarValue && scalarValue.Value is string stringValue)
                {
                    // Redact sensitive data
                    var redactedValue = SensitiveDataRedactor.Redact(stringValue);
                    logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(property.Key, redactedValue));
                }
                // Redact complex objects (e.g., JSON-like structures)
                if (property.Value is StructureValue structureValue)
                {
                    var redactedStructure = RedactStructure(structureValue);
                    logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(property.Key, redactedStructure));
                }

            }
        }

        private StructureValue RedactStructure(StructureValue structureValue)
        {
            var redactedProperties = new List<LogEventProperty>();

            foreach (var property in structureValue.Properties)
            {
                if (property.Value is ScalarValue scalarValue && scalarValue.Value is string stringValue)
                {
                    // Redact scalar values within the structure
                    var redactedValue = SensitiveDataRedactor.Redact(stringValue);
                    redactedProperties.Add(new LogEventProperty(property.Name, new ScalarValue(redactedValue)));
                }
                else
                {
                    // Retain non-sensitive properties as is
                    redactedProperties.Add(property);
                }
            }

            return new StructureValue(redactedProperties);
        }


    }// end class 


}
