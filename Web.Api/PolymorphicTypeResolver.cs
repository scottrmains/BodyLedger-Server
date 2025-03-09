using MediatR;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using Web.Api.Requests.AssignmentItems;
using Application.Assignments.GetById;
using Application.Checklists.GetByUserId;
using System.Reflection;

namespace Web.Api
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            // Set up polymorphic deserialization for CompleteItemRequest
            if (type == typeof(CompleteItemRequest))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "itemType",
                    IgnoreUnrecognizedTypeDiscriminators = false,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes =
                    {
                    new JsonDerivedType(typeof(CompleteWorkoutItemRequest), "Workout"),
                    // Add other types as needed
                }
                };
            }

            if (type == typeof(Requests.Templates.CreateTemplateRequest))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$type",
                    IgnoreUnrecognizedTypeDiscriminators = false,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes =
                {
                    new JsonDerivedType(typeof(Requests.Templates.CreateWorkoutTemplateRequest), "Workout"),

                }
                };
            }



            if (jsonTypeInfo.Type == typeof(AssignmentResponse))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType,
                    DerivedTypes =
            {
                new JsonDerivedType(typeof(WorkoutAssignmentResponse), "Workout"),
                // Add other derived types here as needed
            }
                };
            }

            // Add similar blocks for other polymorphic request types as needed

            return jsonTypeInfo;
        }
    }
}
