namespace BasicExtensions.Attribute
{
    public class HtmlTableColumnAttribute : System.Attribute
    {
        public HtmlTableColumnAttribute()
        {

        }
        public HtmlTableColumnAttribute(string name)
        {
            PropertyName = name;
        }
        public string PropertyName { get; set; }
    }
}
