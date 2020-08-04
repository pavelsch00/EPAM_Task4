namespace InternetCore
{
    public interface ICore
    {
        public void SendMessage(string message);

        public void ReceivingMessages();

        public void StartChat();

        public void Disconnect();
    }
}
