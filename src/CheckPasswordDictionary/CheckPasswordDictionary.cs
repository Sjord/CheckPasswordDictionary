using CheckPasswordDictionary.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CheckPasswordDictionary
{
    public class CheckPasswordDictionary : IPasswordValidator<ApplicationUser>
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public CheckPasswordDictionary(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            var dictionary = LoadDictionary();
            if (dictionary.Contains(password))
            {
                return IdentityResult.Failed(new IdentityError { Code = "TOOCOMMON", Description = "Password is present in a list with commonly used passwords" });
            }
            return IdentityResult.Success;
        }

        private HashSet<string> LoadDictionary()
        {
            var filename = Path.Combine(hostingEnvironment.ContentRootPath, "Data/dictionary.txt");
            return new HashSet<string>(File.ReadLines(filename));
        }
    }
}
