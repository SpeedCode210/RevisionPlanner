
using SQLite;

namespace RevisionPlanner.Models
{
    public class Category
    {
        public static readonly Category Default = new () { 
            Description = "",
            Name = "No Category",
            Id = -1,
            Color = "#3f944b"
        };

        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
