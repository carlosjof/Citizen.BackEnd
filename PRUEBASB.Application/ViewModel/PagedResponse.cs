namespace PRUEBASB.Application.ViewModel
{
    public class PagedResponse <T>
    {
        public int Count { get; set; }
        public int TotalPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public List<T>? Data { get; set; }
    }
}
