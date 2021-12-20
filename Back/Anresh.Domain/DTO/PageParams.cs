namespace Anresh.Domain.DTO
{
    public class PageParams
    {
        public int take;
        public int Skip { get; set; }
        public string OrderBy { get; set; }
        public string AscDesc { get; set; }

        public int Take
        {
            get => take;
            set => take = value == 0 ? 10 : value;
        }
    }
}