using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Data;

namespace ContosoUniversity.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly SchoolContext _context;

        public BaseController(SchoolContext context)
        {
            _context = context;
        }
    }
}
