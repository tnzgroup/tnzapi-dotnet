using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core.Builders
{
    public class ListRequestOptionBuilder<T> where T : class, IListRequestOptions, new()
    {
        public T options { get; set; }

        public ListRequestOptionBuilder()
        {
            options = new T();
        }

        public ListRequestOptionBuilder<T> SetRecordsPerPage(int recordsPerPage)
        {
            options.RecordsPerPage = recordsPerPage;

            return this;
        }

        public ListRequestOptionBuilder<T> SetPage(int page)
        {
            options.Page = page;

            return this;
        }

        public T Build()
        {
            return options;
        }
    }

}
