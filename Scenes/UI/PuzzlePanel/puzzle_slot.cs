using bridge;
public partial class puzzle_slot : VBoxContainer
{

	[Export] private NodePath texture_rect_path;
	private TextureRect texture_rect;


	[Export] private Texture2D[] icons;
	private int index = 0;


	public int Index {
		get { return index; }
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		texture_rect = this.GetNodeCustom<TextureRect>(texture_rect_path);
		texture_rect.Texture = icons[index];
	}

	private void RefreshDisplay()
	{
		while (index < 0) index += icons.Length;
		index %= icons.Length; // wrap
		texture_rect.Texture = icons[index];
	}

	private void SelectPrevious() 
	{
		index--;
		RefreshDisplay();
	}
	private void SelectNext() 
	{
		index++;
		RefreshDisplay();
	}

}
