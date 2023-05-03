public partial class CreditsScene : Control
{

    [Export(PropertyHint.File, "*.tscn")] private string main_menu_path;

    private void OnMenuButtonPressed() => SceneManager.LoadSceneFile(main_menu_path);
}
