﻿using Application.Templates.Create;
using Application.Workouts.Create;
using MediatR;
using SharedKernel;
using System.Xml.Linq;

namespace Web.Api.Requests.Templates
{
    public class CreateWorkoutTemplateRequest : CreateTemplateRequest
    {
        public List<WorkoutActivityRequest> Exercises { get; set; }

        public override IRequest<Result<Guid>> CreateCommand(Guid userId)
        {
            return new CreateWorkoutTemplateCommand(
                Name,
                Description,
                Exercises,
                userId);
        }
    }
}
