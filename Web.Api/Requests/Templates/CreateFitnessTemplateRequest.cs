using Application.Templates.Create;
using MediatR;
using SharedKernel;
using System.Collections.Generic;

namespace Web.Api.Requests.Templates
{
    public class CreateFitnessTemplateRequest : CreateTemplateRequest
    {
        public List<FitnessActivityRequest> Activities { get; set; }

        public override IRequest<Result<Guid>> CreateCommand(Guid userId)
        {

            return new CreateFitnessTemplateCommand(
                Name,
                Description,
                Activities,
                userId);
        }
    }


}