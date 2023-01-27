namespace data;

using System;
using System.Text.Json;
using Godot;

public class AccessSettings // Apparently we have to use a class and properties to serialize json
{
    public bool NoFlashingLights {get; set;}
    public bool ExtraValueBecauseThisFuckerBreaksIfItsJustOneFuckingValueForSomeReason {set; get;}
    public AccessSettings(){}
}

public partial class Accessibility : Node
{
    const string FILE_PATH = "user://access_settings.json";
    private AccessSettings settings = new();
    private static Accessibility instance = null;
    private Accessibility() {}

    public override void _Ready()
    {
        if(instance != null)
        {
            QueueFree();
            return;
        }
        instance = this;
        Deserialize();
    }

    public override void _ExitTree()
    {
        Serialize();
    }

    public static bool NoFlashingLights
    {
        get => instance.settings.NoFlashingLights;
        set => instance.settings.NoFlashingLights = value;
    }

    public static void Serialize()
    {
        var ops = new JsonSerializerOptions { WriteIndented = true };
        string content = JsonSerializer.Serialize(instance.settings, ops);
        var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Write);
        file.StoreString(content);
    }

    public static void Deserialize()
    {
        var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read);
        if (file == null) return;
        try {
            var loaded_data = JsonSerializer.Deserialize<AccessSettings>(file.GetAsText());
            instance.settings = loaded_data;
		}catch (Exception e) {
			GD.PushError($"Error loading AccessibilitySettings from JSON : {e.Message}");
		}
    }

}
