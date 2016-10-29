namespace Farmhand.API.Player
{
    public class MailInformation
    {
        public string Id { get; set; }
        public string Message { get; set; }

        public override string ToString() => Message;
    }
}
