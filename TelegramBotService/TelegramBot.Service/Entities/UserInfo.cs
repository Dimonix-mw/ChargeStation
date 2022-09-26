namespace TelegramBot.Service.Entities
{
    public class UserInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public StateChat StateChat { get; set; }
        public string Token { get; set; }
        public UserInfo()
        {
            StateChat = StateChat.Init;
        }
    }
}
