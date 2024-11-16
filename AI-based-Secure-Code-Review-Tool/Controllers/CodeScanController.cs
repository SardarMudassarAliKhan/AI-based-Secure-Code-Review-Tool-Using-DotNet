using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Secure_Code_Review_Tool.Controllers
{
    public class CodeScanController : Controller
    {
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Scanner()
        {
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult MyDashBoard()
        {
            return View();
        }
    }
}
