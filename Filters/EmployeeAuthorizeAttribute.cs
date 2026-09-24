using System;
using System.Threading.Tasks;
using Gift_of_the_Givers_Relief_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Filters
{
    // Attribute you can place on Razor PageModels to require an "Employee" role.
    // Implementation uses TypeFilter to allow constructor injection of the DbContext.
    public class EmployeeAuthorizeAttribute : TypeFilterAttribute
    {
        public EmployeeAuthorizeAttribute() : base(typeof(RequireEmployeeFilter)) { }

        private class RequireEmployeeFilter : IAsyncPageFilter
        {
            private readonly ApplicationDbContext _db;

            public RequireEmployeeFilter(ApplicationDbContext db)
            {
                _db = db;
            }

            // Run before the page handler. If the cookie is missing or the user is not an employee
            // redirect to the login page (returnUrl set so user can be returned after sign-in).
            public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
            {
                var http = context.HttpContext;
                var requestPath = http.Request.Path + http.Request.QueryString;

                // Read lightweight session cookie used in the prototype.
                if (!http.Request.Cookies.TryGetValue("GgUserId", out var idValue) || !int.TryParse(idValue, out var userId))
                {
                    context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = requestPath });
                    return;
                }

                // Lookup user and verify role == "Employee"
                var user = await _db.Users.FindAsync(userId);
                if (user is null || !string.Equals(user.Role?.Trim(), "Employee", StringComparison.OrdinalIgnoreCase))
                {
                    // Not authorized — send to login (could be AccessDenied page instead)
                    context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = requestPath });
                    return;
                }

                // Authorized — continue to the page handler
                await next();
            }

            // No-op selection hook required by interface.
            public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
        }
    }
}