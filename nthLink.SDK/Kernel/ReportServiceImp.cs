using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Kernel
{
    internal class ReportServiceImp : IReportService
    {
        public string Feedback(FeedbackParameter feedbackParameter)
        {
            return "do feedback";
        }
    }
}
