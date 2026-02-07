using System;
using System.Collections.Generic;

namespace CSHarpCHarles2.Models
{
    public class ProjectTask
    {
        public string Title { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
    }

    public class ProjectDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;

        public int ProgressPercent { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public decimal BudgetSpent { get; set; }
        public decimal BudgetTotal { get; set; }

        public string ProjectManagerName { get; set; } = string.Empty;
        public string ProjectManagerRole { get; set; } = string.Empty;
        public string ProjectManagerInitials { get; set; } = string.Empty;

        public IReadOnlyList<ProjectTask> AssignedTasks { get; set; } = Array.Empty<ProjectTask>();
    }
}
