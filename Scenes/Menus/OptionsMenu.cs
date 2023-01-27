using data;
using Godot;
using System;

public partial class OptionsMenu : Control
{
    [Export(PropertyHint.File, "*.tscn")] private string main_menu_path;

    private void OnMenuButtonPressed()
    {
        Accessibility.Serialize();
        GameDataManager.instance.Serialize();
        GetTree().ChangeSceneToFile(main_menu_path);
    }

}
