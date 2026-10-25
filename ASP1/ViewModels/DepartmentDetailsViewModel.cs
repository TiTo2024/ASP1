using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP1.ViewModels
{
    public class DepartmentDetailsViewModel
    {
        public string DepartmentName { get; set; }
        public string DepartmentState { get; set; } // "Main" لو أكتر من 50، أو "Branch" لو أقل
        public SelectList StudentsOver25List { get; set; }
    }
}