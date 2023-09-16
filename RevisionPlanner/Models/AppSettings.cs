
namespace RevisionPlanner.Models
{
    public class AppSettings
    {
        public int[] TrainingSchedule;

        public AppSettings() {
            TrainingSchedule = new int[] { 1,1,3,7,15 };
        }
    }
}
