namespace events;
using System;

public class EventBus
{
	/*
		Only C# events here. No signals, no interfacing with Godot at all.
		This is mainly just to make things a little more simple. Less overhead from Godot, just straight code patterns and such.
		Love it tbh. Makes me want to make a C# game engine or something. (Probably on top of Godot so I can skip making a decent render engine and all)
	*/

	public static EventBus Instance
	{
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private static EventBus _instance = null; 
	
	public event Action<bool> SetPlayerCanMove;
	public event Action OnPlayerDie;
	public event Action OnPlayerWin;
	public event Action OnGameStart;

	public event Action<string> RequestSubtitle;

	public void TriggerSetPlayerCanMove(bool can_move)
	{
		SetPlayerCanMove?.Invoke(can_move);
	}
	public void TriggerOnPlayerDie()
	{
		OnPlayerDie?.Invoke();
	}
	public void TriggerOnPlayerWin()
	{
		OnPlayerWin?.Invoke();
	}

	public void TriggerOnGameStart()
	{
		OnGameStart?.Invoke();
	}

	public void TriggerRequestSubtitle(string sub_text)
	{
		RequestSubtitle?.Invoke(sub_text);
	}
}
