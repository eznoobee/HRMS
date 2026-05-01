namespace HRMS.Application.DTOs;

public record LeaveBalanceDto(
    Guid LeaveTypeId,
    string LeaveTypeName,
    int Year,
    int TotalDays,
    int UsedDays,
    int RemainingDays);
