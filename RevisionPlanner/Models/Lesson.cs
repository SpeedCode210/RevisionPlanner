
using SQLite;

namespace RevisionPlanner.Models
{
    public class Lesson
    {
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastRevised { get; set; }

    }
}
