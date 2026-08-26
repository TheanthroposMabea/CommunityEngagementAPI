using System.Threading.Tasks;
using CommunityEngagementAPI.Models.DTOs;

namespace CommunityEngagementAPI.Services
{
    public interface IManagementReportService
    {
        Task<ProjectReportDto> GetProjectReportAsync(int? campusId = null);
        Task<OverallPerformanceReportDto> GetOverallPerformanceReportAsync(int? campusId = null);
    }
}
