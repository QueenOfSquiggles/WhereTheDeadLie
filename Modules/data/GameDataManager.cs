namespace data;

using Godot;
using System;

public partial class GameDataManager : Node
{
	[Signal] public delegate void OnPhaseChangedEventHandler(int n_phase);
	[Signal] public delegate void OnAggressionChangedEventHandler(int n_aggression);
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
	[Export] public int GameAggression {
		get {
			return game_data.aggression_level;
		}
		set {
			EmitSignal(nameof(OnAggressionChanged), value);
			game_data.aggression_level = value;
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

	


    public override void _Ready()
    {
		if (instance != null) QueueFree();
		else instance = this;
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

}
