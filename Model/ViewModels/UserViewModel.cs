using Model.Models;

namespace api.reserveerme.nu.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public UserViewModel()
        {
            
        }

        public UserViewModel(User user)
        {
            this.Name = user.Name;
        }
    }
}