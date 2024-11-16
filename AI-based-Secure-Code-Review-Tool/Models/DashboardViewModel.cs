namespace AI_based_Secure_Code_Review_Tool.Models
{
    public class DashboardViewModel
    {
        // Holds the raw text or data related to vulnerabilities
        public string Vulnerabilities { get; set; }

        // Optionally, a breakdown of vulnerabilities for the chart (e.g., by type)
        public Dictionary<string, int> VulnerabilitiesBreakdown { get; set; }

        // Any other dashboard-related data
        public int TotalFilesAnalyzed { get; set; }
        public int ActiveUsers { get; set; }
    }

}
