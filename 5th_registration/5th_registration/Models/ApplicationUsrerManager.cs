using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace _5th_registration.Models
{
    public class ApplicationUsrerManager : UserManager<ApplicationUser>
    {
        public ApplicationUsrerManager(IUserStore<ApplicationUser> store) : base(store)
        {

        }

        public static ApplicationUsrerManager Create(IdentityFactoryOptions<ApplicationUsrerManager> options, IOwinContext context)
        {
            ApplicationDbContext db = context.Get<ApplicationDbContext>();
            ApplicationUsrerManager manager = new ApplicationUsrerManager(new UserStore<ApplicationUser>(db));
            return manager;
        }
    }
}