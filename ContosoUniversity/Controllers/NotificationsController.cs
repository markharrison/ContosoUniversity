using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class NotificationsController : BaseController
    {
        public NotificationsController(SchoolContext context) : base(context)
        {
        }

        // GET: Notifications/Index - Notification dashboard (MSMQ support removed)
        public IActionResult Index()
        {
            return View();
        }
    }
}

