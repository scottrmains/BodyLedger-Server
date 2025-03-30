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
using SharedKernel.Enums;
using Web.Api.Requests.Templates;

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
                    new JsonDerivedType(typeof(CompleteWorkoutItemRequest), (int)TemplateType.Workout),
                    new JsonDerivedType(typeof(CompleteFitnessItemRequest), (int)TemplateType.Fitness),
                }
                };
            }

            if (type == typeof(CreateTemplateRequest))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "templateType",
                    IgnoreUnrecognizedTypeDiscriminators = false,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes =
                {
                    new JsonDerivedType(typeof(CreateWorkoutTemplateRequest), (int)TemplateType.Workout),
                    new JsonDerivedType(typeof(CreateFitnessTemplateRequest), (int)TemplateType.Fitness),
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
                        new JsonDerivedType(typeof(WorkoutAssignmentResponse), (int)TemplateType.Workout),
                        new JsonDerivedType(typeof(FitnessAssignmentResponse), (int)TemplateType.Fitness),
                    }
                };
            }


            return jsonTypeInfo;
        }
    }
}
