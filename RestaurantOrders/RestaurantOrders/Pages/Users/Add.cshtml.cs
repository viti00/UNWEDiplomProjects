using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOrders.Pages.Users
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AddModel(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Role { get; set; }
        public void OnGetAsync()
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Users.Any(x => x.UserName == Email))
            {
                ModelState.AddModelError("Exist", "Вече има потребител с този имейл адрес");
            }
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Email,
                Email = Email,
            };

            var roleName = _context.Roles.FirstOrDefault(x => x.Id == Role).Name;

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, roleName);
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
