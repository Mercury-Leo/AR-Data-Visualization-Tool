using System;

namespace Core.Placeholders {
    public class ActivityLogPlaceholder {
        string _type;
        string _userName;
        DateTime _time;
        string _command;

        public ActivityLogPlaceholder(string type, string userName, DateTime time, string command) {
            _type = type;
            _userName = userName;
            _time = time;
            _command = command;
        }
    }
}