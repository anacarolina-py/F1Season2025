namespace Domain.Competition.Models.Messages;

public abstract class BaseMessage
{
    public Guid MessageId { get; protected set; } = Guid.NewGuid();

    public string MessageType { get; protected set; }

    protected BaseMessage()
    {
        MessageType = GetType().Name;
    }
}
