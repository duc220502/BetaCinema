using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.AI
{
    public class AiPolicyService : IAiPolicyService
    {
        private static readonly List<AiPolicyItemDto> Policies =
        [
            new(){Title =  "Hoàn vé",Content =  "Vé đã thanh toán không hoàn tiền. Khách hàng vui lòng kiểm tra kỹ trước khi xác nhận."},
            new(){ Title =  "Đổi vé", Content = "Khách hàng có thể đổi sang suất khác trước giờ chiếu 30 phút nếu hệ thống và điều kiện vé cho phép." },
            new(){Title = "Thành viên",Content =  "Thành viên được tích điểm theo hóa đơn và nhận ưu đãi theo hạng thành viên." },
            new(){Title = "Thanh toán", Content = "BetaCinema hỗ trợ thanh toán trực tuyến qua cổng thanh toán được hiển thị tại trang checkout."},
        ];

        public Task<IReadOnlyList<AiPolicyItemDto>> SearchRelevantPoliciesAsync(string query, CancellationToken ct)
        {
            var q = query.ToLowerInvariant();

            var result = Policies
                .Where(x =>
                    x.Title.ToLowerInvariant().Contains(q) ||
                    x.Content.ToLowerInvariant().Contains(q) ||
                    (q.Contains("hoàn vé") && x.Title == "Hoàn vé") ||
                    (q.Contains("đổi vé") && x.Title == "Đổi vé") ||
                    (q.Contains("thành viên") && x.Title == "Thành viên") ||
                    (q.Contains("thanh toán") && x.Title == "Thanh toán") ||
                    (q.Contains("ưu đãi") && x.Title == "Thành viên"))
                .Take(3)
                .ToList();

            if (result.Count == 0)
                result = Policies.Take(2).ToList();

            return Task.FromResult<IReadOnlyList<AiPolicyItemDto>>(result);
        }


    }
}
