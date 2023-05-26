using events;

public partial class SubtitleEmitter : Node
{

    public void EmitSubtitle(string text)
    {
        EventBus.Instance.TriggerRequestSubtitle(text);
    }
}
