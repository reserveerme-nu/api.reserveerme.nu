using api.reserveerme.nu.ViewModels;

namespace Model.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User()
        {
            
        }

        public User(UserViewModel userViewModel)
        {
            this.Name = userViewModel.Name;
        }
    }
}