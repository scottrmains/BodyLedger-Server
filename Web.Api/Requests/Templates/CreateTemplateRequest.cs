using MediatR;
using SharedKernel;

namespace Web.Api.Requests.Templates
{
    public abstract class CreateTemplateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public abstract IRequest<Result<Guid>> CreateCommand(Guid userId);
    }
}
