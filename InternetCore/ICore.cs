namespace InternetCore
{
    public interface ICore
    {
        public void SendMessage(string message);

        public void StartChat();

        public void Disconnect();
    }
}
