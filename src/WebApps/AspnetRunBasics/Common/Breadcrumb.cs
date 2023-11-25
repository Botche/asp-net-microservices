namespace AspnetRunBasics.Common
{
    public class Breadcrumb
    {
        public Breadcrumb(string name)
        {
            this.Page = string.Empty;
            this.Name = name;
        }

        public Breadcrumb(string page, string name)
            : this(name)
        {
            this.Page = page;
        }

        public string Page { get; set; }

        public string Name { get; set; }

        public bool ShouldAppedPage => this.Page != string.Empty;

    }
}
