public partial class JumpscareScene : Node3D
{
    [Export(PropertyHint.File, "*.tscn")] private string main_menu_file = "";

    private void ReturnToMenu() => SceneManager.LoadSceneFile(main_menu_file);
}
