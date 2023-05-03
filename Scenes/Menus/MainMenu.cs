using bridge;

public partial class MainMenu : Control
{
    [Export] private bool test_pirate = false;
    [Export] private PackedScene play_scene;
    [Export] private PackedScene options_scene;
    [Export] private PackedScene credits_scene;
    [Export] private NodePath path_pirate_popup;

    private AcceptDialog pirate_popup;

    public override void _Ready()
    {
        GD.Print("Loaded main menu scene");
        pirate_popup = this.GetNodeCustom<AcceptDialog>(path_pirate_popup);
        if (OS.HasFeature("pirate") || test_pirate)
        {
            pirate_popup.PopupCenteredRatio(0.8f);
            OS.ShellOpen("https://ko-fi.com/queenofsquiggles");
            OS.ShellOpen("https://queenofsquiggles.itch.io/where-the-dead-lie");
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    private void OnBtnPlay() => SceneManager.LoadScenePacked(play_scene);

    private void OnBtnOptions() => SceneManager.LoadScenePacked(options_scene);

    private void OnBtnCredits() => SceneManager.LoadScenePacked(credits_scene);

    private void OnBtnQuit() => GetTree().Quit();


}
