using System;

namespace Anresh.Domain.Shared
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Table { get; set; }
        public TableAttribute(string table)
        {
            Table = table;
        }
    }
}
