using MediatR;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using Web.Api.Requests.AssignmentItems;
using Application.Assignments.GetById;
using Application.Checklists.GetByUserId;
using System.Reflection;
using SharedKernel.Responses;
using Domain.Templates;
using SharedKernel;

namespace Web.Api
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
            if (type == typeof(CompleteItemRequest))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "templateType",
                    IgnoreUnrecognizedTypeDiscriminators = false,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes =
                    {
                    new JsonDerivedType(typeof(CompleteWorkoutItemRequest), TemplateType.Workout.ToString()),
                    new JsonDerivedType(typeof(CompleteFitnessItemRequest), TemplateType.Fitness.ToString()),
                }
                };
            }

            if (type == typeof(Requests.Templates.CreateTemplateRequest))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "templateType",
                    IgnoreUnrecognizedTypeDiscriminators = false,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes =
                {
                    new JsonDerivedType(typeof(Requests.Templates.CreateWorkoutTemplateRequest), TemplateType.Workout.ToString()),
                    new JsonDerivedType(typeof(Requests.Templates.CreateFitnessTemplateRequest), TemplateType.Fitness.ToString()),
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
                        new JsonDerivedType(typeof(WorkoutAssignmentResponse), TemplateType.Workout.ToString()),
                        new JsonDerivedType(typeof(FitnessAssignmentResponse), TemplateType.Fitness.ToString()),
                    }
                };
            }

            // Add similar blocks for other polymorphic request types as needed

            return jsonTypeInfo;
        }
    }
}
