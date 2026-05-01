namespace HRMS.Application.DTOs;

public record OvertimeRequestDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeFullName,
    DateTime StartTime,
    DateTime EndTime,
    double TotalHours,
    string? Reason,
    string Status,
    string? ManagerReviewerName,
    string? ManagerNote,
    DateTime? ManagerReviewedAt,
    string? HRReviewerName,
    string? HRNote,
    DateTime? HRReviewedAt,
    DateTime CreatedAt);
