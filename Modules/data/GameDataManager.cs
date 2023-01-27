namespace data;

using events;
using Godot;
using System;
using System.Text.Json;

public partial class GameDataManager : Node
{
	[Signal] public delegate void OnPhaseChangedEventHandler(int n_phase);
	[Signal] public delegate void OnActiveGeneratorsChangedEventHandler(int n_generators);
	[Signal] public delegate void OnFoundKeysChangedEventHandler(int n_keys);
	[Signal] public delegate void OnPuzzleSolvedEventHandler();


	public static GameDataManager instance = null;
	private GameData game_data = new();


	[Export] public int GamePhase {
		// C# properties are mega based
		get {
			return game_data.phase;
		}
		set {
			EmitSignal(nameof(OnPhaseChanged), value);
			game_data.phase = value;
		}
	}
	
	[Export] public int ActiveGenerators {
		get {
			return game_data.active_generators;
		}
		set {
			EmitSignal(nameof(OnActiveGeneratorsChanged), value);
			game_data.active_generators = value;
			CheckGamePhase();
		}
	}

	[Export] public bool PuzzleSolved {
		get {
			return game_data.puzzle_solved;
		}
		set {
			if (value) EmitSignal(nameof(OnPuzzleSolved), value);
			game_data.puzzle_solved = value;
			CheckGamePhase();
		}
	}

	[Export] public int FoundKeys {
		get{
			return game_data.found_keys;
		}
		set {
			EmitSignal(nameof(OnFoundKeysChanged), value);
			game_data.found_keys = value;
			CheckGamePhase();
		}
	}
	public int GameAggression
	{
		get => game_data.aggression_level;
		set => game_data.aggression_level = value;
	}
	public int RequiredGenerators
	{
		get => game_data.required_generators;
		set => game_data.required_generators = value;
	}
	public int RequiredKeys
	{
		get => game_data.required_keys;
		set => game_data.required_keys = value;
	}


	public int LevelGenerators {
		get {
			return game_data.level_generators;
		}
		set {
			game_data.level_generators = value;
			if (game_data.required_generators > value) game_data.required_generators = value;
		}
	}
	public int LevelChests {
		get {
			return game_data.level_chests;
		}
		set {
			game_data.level_chests = value;
			if (game_data.required_keys > value) game_data.required_keys = value;
		}
	}
	const string FILE_PATH = "user://settings.json";

    public override void _Ready()
    {
		if (instance != null) QueueFree();
		else instance = this;
		EventBus.Instance.OnPlayerDie += OnEventPlayerDies;
		Deserialize();
		Reset();
    }

	private void CheckGamePhase() 
	{
		bool flag = false;
		switch (game_data.phase)
		{
			case GameData.PHASE_GENERATORS:
				flag = CheckPhaseGenerators();
				break;
			case GameData.PHASE_PUZZLE:
				flag = CheckPhasePuzzle();
				break;
			case GameData.PHASE_KEYS:
				flag = CheckPhaseKeys();
				break;
		}

		if (flag) 
		{
			GamePhase += 1; // will trigger signal
			GD.Print($"GameDataManager has progressed to phase #{GamePhase}");
		}
	}

	private bool CheckPhaseGenerators()
	{
		return game_data.active_generators >= game_data.required_generators;
	}

	private bool CheckPhasePuzzle()
	{
		return game_data.puzzle_solved;
	}

	private bool CheckPhaseKeys()
	{
		return game_data.found_keys >= game_data.required_keys;
	}

	public void Reset()
	{
		game_data.active_generators = 0;
		game_data.puzzle_solved = false;
		game_data.found_keys = 0;
		game_data.phase = GameData.PHASE_GENERATORS;
		Serialize();
	}

	// Event Callbacks

	private void OnEventPlayerDies()
	{
		GD.Print("Player has died!");
	}

	// IO

	public void Serialize()
	{
		var ops = new JsonSerializerOptions { WriteIndented = true };
		var content = JsonSerializer.Serialize(game_data, ops);
		var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Write);
		file.StoreString(content);
	}

	public void Deserialize()
	{
        var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read);
		if (file == null) return;
		try {
			var loaded_data = JsonSerializer.Deserialize<GameData>(file.GetAsText());
			game_data = loaded_data;
		}catch (Exception e) {
			GD.PushError($"Error loading GameData from JSON : {e.Message}");
		}
	}

}
