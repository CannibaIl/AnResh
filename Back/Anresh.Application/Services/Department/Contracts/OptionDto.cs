namespace Anresh.Application.Services.Department.Contracts
{
    public class OptionDto
    {
        public OptionDto(int id, string label)
        {
            Id = id;
            Label = label;
        }
        public int Id { get; private set; }
        public string Label { get; private set; }
    }
}