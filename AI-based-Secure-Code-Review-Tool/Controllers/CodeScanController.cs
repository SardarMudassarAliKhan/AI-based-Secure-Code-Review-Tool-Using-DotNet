using AI_based_Secure_Code_Review_Tool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;

namespace AI_based_Secure_Code_Review_Tool.Controllers
{
    [Authorize]
    public class CodeScanController : Controller
    {
        private readonly OpenAIClient _openAIClient;

        public CodeScanController(OpenAIClient openAIClient)
        {
            _openAIClient = openAIClient;
        }

        public IActionResult SubmitFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scanner(FileUploadViewModel model)
        {
            //if (model.CodeFile == null || model.CodeFile.Length == 0)
            //{
            //    ModelState.AddModelError("CodeFile", "File is required.");
            //    return View(model);
            //}

            //if (model.CodeFile.Length > 5 * 1024 * 1024) // 5 MB limit
            //{
            //    ModelState.AddModelError("CodeFile", "File size must be less than 5 MB.");
            //    return View(model);
            //}

            //var allowedExtensions = new[] { ".cs", ".java", ".py", ".js" };
            //var fileExtension = Path.GetExtension(model.CodeFile.FileName).ToLower();
            //if (!allowedExtensions.Contains(fileExtension))
            //{
            //    ModelState.AddModelError("CodeFile", "Unsupported file type.");
            //    return View(model);
            //}

            //string fileContent;
            //using (var reader = new StreamReader(model.CodeFile.OpenReadStream()))
            //{
            //    fileContent = await reader.ReadToEndAsync();
            //}
            // This could be loaded from TempData or a database
            var model1 = new DashboardViewModel
            {
                // This could be loaded from TempData or a database
                Vulnerabilities = TempData["Vulnerabilities"]?.ToString(),

                // Example: Breakdown of vulnerabilities (this data could come from the backend analysis)
                VulnerabilitiesBreakdown = new Dictionary<string, int>
                    {
                        { "SQL Injection", 5 },
                        { "XSS", 3 },
                        { "Buffer Overflow", 2 }
                    },

                TotalFilesAnalyzed = 1,  // Example data
                ActiveUsers = 1           // Example data
            };

            var vulnerabilities = JsonConvert.SerializeObject(model1.VulnerabilitiesBreakdown); ;

            // Save to database or other persistent storage instead of TempData for better security.
            TempData["Vulnerabilities"] = vulnerabilities;

            return RedirectToAction("MyDashBoard");
        }

        public IActionResult MyDashBoard()
        {
            var model = new DashboardViewModel
            {
                // This could be loaded from TempData or a database
                Vulnerabilities = TempData["Vulnerabilities"]?.ToString(),

                // Example: Breakdown of vulnerabilities (this data could come from the backend analysis)
                VulnerabilitiesBreakdown = new Dictionary<string, int>
                    {
                        { "SQL Injection", 5 },
                        { "XSS", 3 },
                        { "Buffer Overflow", 2 }
                    },

                TotalFilesAnalyzed = 1,  // Example data
                ActiveUsers = 1           // Example data
            };

            return View(model);
        }

        //private async Task<string> AnalyzeFile(string fileContent)
        //{
        //    // Create chat completion options
        //    var chatCompletionsOptions = new ChatCompletionsOptions
        //    {
        //        Messages = new[]
        //        {
        //            new ChatMessage(ChatRole.System, "You are a code vulnerability scanner."),
        //            new ChatMessage(ChatRole.User, $"Analyze the following code for vulnerabilities:\n{fileContent}")
        //        },
        //        MaxTokens = 1000,
        //        Temperature = 0.5f
        //    };

        //    // Make a request to the Azure OpenAI service
        //    var completionResponse = await _openAIClient.GetChatCompletionsAsync("gpt-4", chatCompletionsOptions);

        //    // Retrieve and return the response content
        //    return completionResponse.Choices.FirstOrDefault()?.Message.Content ?? "No vulnerabilities detected.";
        //}

        [HttpPost]
        public IActionResult PrintReport()
        {
            var vulnerabilities = TempData["Vulnerabilities"] as string;

            // Generate PDF report logic (using iTextSharp or similar)
            var pdf = GeneratePdf(vulnerabilities);

            return File(pdf, "application/pdf", "VulnerabilityReport.pdf");
        }

        private byte[] GeneratePdf(string vulnerabilities)
        {
            // Implement PDF generation logic
            return new byte[0]; // Placeholder
        }

    }
}
