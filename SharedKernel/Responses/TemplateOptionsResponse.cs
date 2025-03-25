

namespace SharedKernel.Responses;

    public sealed class TemplateOptionsResponse
    {
        public List<TemplateOption> Templates { get; init; } = new();

    }


    public sealed class TemplateOption
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int Type { get; init; }
    }