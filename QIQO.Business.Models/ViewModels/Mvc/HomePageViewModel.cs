using QIQO.Business.Client.Entities;

namespace QIQO.Business.ViewModels.Mvc
{
    public class HomePageViewModel
    {
        public Account CurrentAccount { get; set; } = new Account() { AccountCode = "JDII", AccountName = "Java Dreams II" };
        public User CurrentUser { get; set; } = new User() { Email = "this@that.com", UserName = "Jerry Sienfield" };
    }
}
