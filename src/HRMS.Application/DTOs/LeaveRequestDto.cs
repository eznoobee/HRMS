namespace HRMS.Application.DTOs;

public record LeaveRequestDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeFullName,
    string LeaveTypeName,
    DateOnly StartDate,
    DateOnly EndDate,
    int TotalDays,
    string? Reason,
    string Status,
    string? ManagerReviewerName,
    string? ManagerNote,
    DateTime? ManagerReviewedAt,
    string? HRReviewerName,
    string? HRNote,
    DateTime? HRReviewedAt,
    DateTime CreatedAt);
