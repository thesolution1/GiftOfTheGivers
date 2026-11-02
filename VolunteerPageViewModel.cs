using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels;

public class VolunteerPageViewModel
{
    public bool IsAuthenticated { get; set; }
    public string? DonorName { get; set; }
    public List<ProjectCardViewModel> Projects { get; set; } = [];
}

public class ProjectCardViewModel
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    // The existing ReliefProject.Update field is used as the summary.
    public string Summary { get; set; } = string.Empty;

    public bool HasPendingRequest { get; set; }
    public bool IsApproved { get; set; }
}

public class VolunteerSubmissionViewModel
{
    [Required, StringLength(1000)]
    public string Skills { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Availability { get; set; } = string.Empty;
}

public class UpcomingProjectSubmissionViewModel
{
    [Required, StringLength(1000)]
    public string Skills { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Availability { get; set; } = string.Empty;
}