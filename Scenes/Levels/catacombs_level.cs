using bridge;
using data;
using Godot.Collections;
using objects;
using System.Collections.Generic;

public partial class catacombs_level : Node3D
{

	[Signal] public delegate void OpenPuzzlePanelEventHandler();

	[Export] private NodePath chest_root_path;
	[Export] private NodePath generators_root_path;

	[Export] private NodePath path_smeth_patrol_points_parent;
	[Export] private NodePath path_smeth;
	private TheSmeth the_smeth;
	private GameDataManager gdm = null;

	public override void _Ready()
	{
		gdm = GameDataManager.instance;
		SelectGenerators();
		SelectChests();
		the_smeth = this.GetNodeCustom<TheSmeth>(path_smeth);
		AddSmethPatrolPoints(this.GetNodeCustom<Node3D>(path_smeth_patrol_points_parent).GetChildren());
	}

	private void SelectGenerators()
	{
		var viable_objs = GetViableObjects(this.GetNodeCustom<Node3D>(generators_root_path).GetChildren());
		gdm.LevelGenerators = viable_objs.Count;
		SelectViableObjects(viable_objs, gdm.RequiredGenerators);
	}

	private void SelectChests()
	{
		var viable_objs = GetViableObjects(this.GetNodeCustom<Node3D>(chest_root_path).GetChildren());
		gdm.LevelChests = viable_objs.Count;
		SelectViableObjects(viable_objs, gdm.RequiredKeys);
	}

	private List<IHasViability> GetViableObjects(Array<Node> nodes)
	{
		List<IHasViability> target_objects = new();
		foreach (var c in nodes)
		{
            if (c is IHasViability v_obj && (c as Node3D).Visible) target_objects.Add(v_obj);
        }
		return target_objects;
	}

	private void SelectViableObjects(List<IHasViability> target_objects, int allowed_count)
	{
		var rand = new Random();
		for(int i = 0; i < allowed_count; i++)
		{
			int index = rand.Next() % target_objects.Count;
			target_objects[index].SetViability(true);
			target_objects.RemoveAt(index);
		}

	}

	private void AddSmethPatrolPoints(Array<Node> nodes)
	{
		List<Marker3D> markers = new();
		foreach (Node n in nodes)
		{
			if (n is Marker3D point && point.Visible)
			{
				markers.Add(point);
			}
		}
		the_smeth.LoadPatrolPoints(markers);
	}

    public void TriggerOpenPanel() => EmitSignal(nameof(OpenPuzzlePanel));

}
