using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCandSC_CSharp.Pages
{
    public class LoginModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Name { get; set; } = "";

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        User user { get; set; } = new();

        public IActionResult OnGet()
        {
            return RedirectToPage("/Formation", new { userId = 1 });
            //return Page();
        }

        public IActionResult OnPostLogin()
        {
            user = user.GetUser(Name, Password);
            if (user.Id > 0)
                return RedirectToPage("/Formation", new {userId = user.Id});
                
            return Page();
        }

        public IActionResult OnPostSignUp()
        {
            user = user.SetUser(Name, Password);
            if (user.Id > 0)
                return RedirectToPage("/Formation", new { userId = user.Id });

            return Page();
        }

    }
}