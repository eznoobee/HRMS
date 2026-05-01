namespace HRMS.Domain.Enums;

public enum NotificationType
{
    LeaveApplied = 1,
    LeaveManagerApproved = 2,
    LeaveManagerRejected = 3,
    LeaveHRApproved = 4,
    LeaveHRRejected = 5,
    OvertimeApplied = 6,
    OvertimeManagerApproved = 7,
    OvertimeManagerRejected = 8,
    OvertimeHRApproved = 9,
    OvertimeHRRejected = 10,
    NewMessage = 11,
    NewDirectMessage = 12,
    NewAnnouncement = 13,
    MentionedInMessage = 14
}
