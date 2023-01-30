using events;
using Godot;
using System;

public partial class SubtitleEmitter : Node
{

    public void EmitSubtitle(string text)
    {
        EventBus.Instance.TriggerRequestSubtitle(text);
    }
}
