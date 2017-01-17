namespace DocumentsExchange.Hub.Models
{
    public class HubUser
    {
        public string Id { get; set; }

        public UserState State { get; set; }

        public bool IsMobile { get; set; }
    }

    public enum UserState
    {
        Unknown = 0,
        Online = 1,
        Away = 2,
        Busy = 3,
        Invisible = 4,
        Offline = 5
    }
}
