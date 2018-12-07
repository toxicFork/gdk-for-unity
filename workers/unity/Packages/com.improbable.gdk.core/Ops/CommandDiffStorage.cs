using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class CommandDiffStorage<T>
    {
        private readonly List<T> messages = new List<T>();

        public void Clean()
        {
            messages.Clear();
        }

        public void AddMessage(T message)
        {
            messages.Add(message);
        }

        public void GetMessages(ICollection<T> messageList)
        {
            foreach (var message in messages)
            {
                messageList.Add(message);
            }
        }
    }
}
