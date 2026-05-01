using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using Mapster;

namespace HRMS.Application.Common.Mappings;

public static class MappingConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<Company, CompanyDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.LogoUrl, src => src.LogoUrl)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Department, DepartmentDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CompanyId, src => src.CompanyId)
            .Map(dest => dest.ManagerId, src => src.ManagerId)
            .Map(dest => dest.ManagerFullName,
                src => src.Manager != null
                    ? $"{src.Manager.FirstName} {src.Manager.LastName}"
                    : null)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Employee, EmployeeDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.AvatarUrl, src => src.AvatarUrl)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
            .Map(dest => dest.JoinDate, src => src.JoinDate)
            .Map(dest => dest.JobTitle, src => src.JobTitle)
            .Map(dest => dest.Role, src => src.Role.ToString())
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.CompanyId, src => src.CompanyId)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .Map(dest => dest.DepartmentName,
                src => src.Department != null ? src.Department.Name : string.Empty)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<LeaveRequest, LeaveRequestDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeId, src => src.EmployeeId)
            .Map(dest => dest.EmployeeFullName,
                src => src.Employee != null
                    ? $"{src.Employee.FirstName} {src.Employee.LastName}"
                    : string.Empty)
            .Map(dest => dest.LeaveTypeName,
                src => src.LeaveType != null ? src.LeaveType.Name : string.Empty)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.TotalDays, src => src.TotalDays)
            .Map(dest => dest.Reason, src => src.Reason)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.ManagerReviewerName,
                src => src.ManagerReviewer != null
                    ? $"{src.ManagerReviewer.FirstName} {src.ManagerReviewer.LastName}"
                    : null)
            .Map(dest => dest.ManagerNote, src => src.ManagerNote)
            .Map(dest => dest.ManagerReviewedAt, src => src.ManagerReviewedAt)
            .Map(dest => dest.HRReviewerName,
                src => src.HRReviewer != null
                    ? $"{src.HRReviewer.FirstName} {src.HRReviewer.LastName}"
                    : null)
            .Map(dest => dest.HRNote, src => src.HRNote)
            .Map(dest => dest.HRReviewedAt, src => src.HRReviewedAt)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<LeaveBalance, LeaveBalanceDto>.NewConfig()
            .Map(dest => dest.LeaveTypeId, src => src.LeaveTypeId)
            .Map(dest => dest.LeaveTypeName,
                src => src.LeaveType != null ? src.LeaveType.Name : string.Empty)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.TotalDays, src => src.TotalDays)
            .Map(dest => dest.UsedDays, src => src.UsedDays)
            .Map(dest => dest.RemainingDays, src => src.RemainingDays);

        TypeAdapterConfig<OvertimeRequest, OvertimeRequestDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeId, src => src.EmployeeId)
            .Map(dest => dest.EmployeeFullName,
                src => src.Employee != null
                    ? $"{src.Employee.FirstName} {src.Employee.LastName}"
                    : string.Empty)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.TotalHours, src => src.TotalHours)
            .Map(dest => dest.Reason, src => src.Reason)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.ManagerReviewerName,
                src => src.ManagerReviewer != null
                    ? $"{src.ManagerReviewer.FirstName} {src.ManagerReviewer.LastName}"
                    : null)
            .Map(dest => dest.ManagerNote, src => src.ManagerNote)
            .Map(dest => dest.ManagerReviewedAt, src => src.ManagerReviewedAt)
            .Map(dest => dest.HRReviewerName,
                src => src.HRReviewer != null
                    ? $"{src.HRReviewer.FirstName} {src.HRReviewer.LastName}"
                    : null)
            .Map(dest => dest.HRNote, src => src.HRNote)
            .Map(dest => dest.HRReviewedAt, src => src.HRReviewedAt)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Announcement, AnnouncementDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.Scope, src => src.Scope.ToString())
            .Map(dest => dest.CompanyId, src => src.CompanyId)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .Map(dest => dest.DepartmentName,
                src => src.Department != null ? src.Department.Name : null)
            .Map(dest => dest.AuthorFullName,
                src => src.Author != null
                    ? $"{src.Author.FirstName} {src.Author.LastName}"
                    : string.Empty)
            .Map(dest => dest.IsPinned, src => src.IsPinned)
            .Map(dest => dest.ExpiresAt, src => src.ExpiresAt)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Channel, ChannelDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Type, src => src.Type.ToString())
            .Map(dest => dest.CompanyId, src => src.CompanyId)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .Map(dest => dest.CreatorFullName,
                src => src.Creator != null
                    ? $"{src.Creator.FirstName} {src.Creator.LastName}"
                    : string.Empty)
            .Map(dest => dest.MemberCount,
                src => src.Members != null ? src.Members.Count : 0)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Message, MessageDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ChannelId, src => src.ChannelId)
            .Map(dest => dest.SenderId, src => src.SenderId)
            .Map(dest => dest.SenderFullName,
                src => src.Sender != null
                    ? $"{src.Sender.FirstName} {src.Sender.LastName}"
                    : string.Empty)
            .Map(dest => dest.SenderAvatarUrl,
                src => src.Sender != null ? src.Sender.AvatarUrl : null)
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.SentAt, src => src.SentAt)
            .Map(dest => dest.EditedAt, src => src.EditedAt)
            .Map(dest => dest.ReplyToMessageId, src => src.ReplyToMessageId)
            .Map(dest => dest.ReplyToContent,
                src => src.ReplyToMessage != null ? src.ReplyToMessage.Content : null);

        TypeAdapterConfig<Notification, NotificationDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.Type, src => src.Type.ToString())
            .Map(dest => dest.IsRead, src => src.IsRead)
            .Map(dest => dest.RelatedEntityId, src => src.RelatedEntityId)
            .Map(dest => dest.ActionUrl, src => src.ActionUrl)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<DirectMessage, DirectMessageDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SenderId, src => src.SenderId)
            .Map(dest => dest.SenderFullName,
                src => src.Sender != null
                    ? $"{src.Sender.FirstName} {src.Sender.LastName}"
                    : string.Empty)
            .Map(dest => dest.SenderAvatarUrl,
                src => src.Sender != null ? src.Sender.AvatarUrl : null)
            .Map(dest => dest.ReceiverId, src => src.ReceiverId)
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.SentAt, src => src.SentAt)
            .Map(dest => dest.EditedAt, src => src.EditedAt)
            .Map(dest => dest.IsRead, src => src.IsRead);
    }
}
