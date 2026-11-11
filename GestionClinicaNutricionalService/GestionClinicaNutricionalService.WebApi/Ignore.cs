using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using NJsonSchema.Generation;

namespace GestionClinicaNutricionalService
{
    public class ExcludeHabitoAlimenticioPropertySchemaProcessor : ISchemaProcessor
    {
        public void Process(SchemaProcessorContext context)
        {
            foreach (var key in context.Schema.Properties.Keys)
            {
                //Console.WriteLine(key);
                //Console.WriteLine(context.ContextualType.Type);
            }

            if (context.ContextualType.Type == typeof(HabitoAlimenticio) || context.ContextualType.Type == typeof(PlanComida))
            {
                context.Schema.Properties.Remove("domainEvents");
                context.Schema.Properties.Remove("id");
            }
        }
    }
}