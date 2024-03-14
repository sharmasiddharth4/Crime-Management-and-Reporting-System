using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlyForIdentity.Models
{
    public class ReportEditViewModel
    {
        public int ReportId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; } // Include the FullName property from User
        public Case? Case { get; set; }


        public Case.CaseStatus SelectedCaseStatus { get; set; } // New property for drop-down
        public List<SelectListItem> CaseStatusOptions { get; set; } // Options for drop-down
    }
}
