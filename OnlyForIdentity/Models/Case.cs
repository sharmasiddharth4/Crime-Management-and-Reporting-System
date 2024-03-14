namespace OnlyForIdentity.Models
{
    public class Case
    {
        public int CaseId { get; set; }
        public enum CaseStatus
        {
            Solved,
            Unsolved,
            InProgress
        }
        internal CaseStatus Status { get; set; }
    }
}
