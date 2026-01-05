namespace TrickyTrayAPI.Models
{
    public class SystemState
    {
        public enum SaleStatus
        {
            Draft = 0,    // לפני תחילת המכירה
            Active = 1,   // המכירה פתוחה
            Finished = 2  // אחרי ההגרלה
        }

        public int Id { get; set; }

        public SaleStatus Status { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


    }
}
