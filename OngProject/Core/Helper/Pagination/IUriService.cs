namespace OngProject.Core.Helper.Pagination
{
    public interface IUriService
    {
        public string GetPage(string route, int? page);
    }
}