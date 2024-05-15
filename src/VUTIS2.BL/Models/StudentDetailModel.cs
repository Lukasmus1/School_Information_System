using System.Collections.ObjectModel;

namespace VUTIS2.BL.Models;

public record StudentDetailModel : ModelBase
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    public string? PhotoUrl { get; init; }

    public ObservableCollection<EnrollmentListModel> Enrollments { get; init; } = new ();

    public static StudentDetailModel Empty => new()
    {
        Id = Guid.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
    };
}
