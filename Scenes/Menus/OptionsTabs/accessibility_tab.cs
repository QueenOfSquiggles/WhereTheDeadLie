using data;
using bridge;

public partial class accessibility_tab : PanelContainer
{
    [Export] private NodePath path_checkbox_no_flashing_lights;

    public override void _Ready()
    {
        var chk_no_flash = this.GetNodeCustom<CheckBox>(path_checkbox_no_flashing_lights);
        chk_no_flash.SetPressedNoSignal(Accessibility.NoFlashingLights);
    }

    private void OnNoFlashingLightsChanged(bool do_no_flashing_lights)
    {
        Accessibility.NoFlashingLights = do_no_flashing_lights;
        Accessibility.Serialize();
    }
}
