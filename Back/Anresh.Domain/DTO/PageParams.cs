namespace Anresh.Domain.DTO
{
    public class PageParams
    {
        public int? Take { get; set; }
        public int Skip { get; set; }
        public string OrderBy { get; set; }
        public string AscDesc { get; set; }
    }
}