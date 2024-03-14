using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace OnlyForIdentity.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public string? UserId { get; set; }
        public Case? Case { get; set; }
        public string? AccusedName { get; set; }
        public Report()
        {
            Case = new Case { Status = Case.CaseStatus.Unsolved };
        }
    }
}
