namespace FSPWebAPI
{
    public record UserMessage(
        string Name,
        string Message,
        string Room,
        DateTimeOffset SendAt);


    public class ChatRegisry
    {
        private readonly Dictionary<string, List<UserMessage>> _roomMessages = new()
        {
            {"7443059C-1B28-4990-666B-08DBFC9A99D0", new List<UserMessage>() },
            {"8350B5D4-DB25-4780-55F3-08DC00B7D44F", new List<UserMessage>() },
            {"194E577D-F5E0-4613-483C-08DC011D0550", new List<UserMessage>() },
        };

        public void CreateRoom(string room)
        {
            _roomMessages[room] = new();
        }

        public void AddMessage(string room, UserMessage message)
        {
            _roomMessages[room].Add(message);
        }

        public List<UserMessage> GetMessages(string room)
        {
            return _roomMessages[room];
        }

        public List<string> GetRooms()
        {
            return _roomMessages.Keys.ToList();
        }
    }
}
