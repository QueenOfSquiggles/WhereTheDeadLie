public partial class PauseMenu : Control
{

    [Export(PropertyHint.File, "*.tscn")] private string main_menu_file;

    public override void _Ready()
    {
        GetTree().Paused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("ui_cancel"))
        {
            ReturnToPlay();
            GetViewport().SetInputAsHandled();
        }
    }

    private void ReturnToPlay()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GetTree().Paused = false;
        QueueFree();
    }

    private void ExitToMainMenu()
    {
        GetTree().Paused = false;
        SceneManager.LoadSceneFile(main_menu_file);
    }
}
