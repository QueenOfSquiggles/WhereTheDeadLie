using bridge;
using data;
using Godot;
using System;
using System.Collections.Generic;

public partial class GameplayTab : PanelContainer
{
    [Export] private NodePath path_difficulty_preset;
    [Export] private NodePath path_required_generators;
    [Export] private NodePath path_required_keys;
    [Export] private NodePath path_aggression;

    private OptionButton difficulty_presets;
    private HSlider r_generators;
    private HSlider r_keys;
    private HSlider aggression;

    private List<KeyValuePair<string, GamePreset>> available_presets = new();
    public override void _Ready()
    {
        difficulty_presets = this.GetNodeCustom<OptionButton>(path_difficulty_preset);
        r_generators = this.GetNodeCustom<HSlider>(path_required_generators);
        r_keys = this.GetNodeCustom<HSlider>(path_required_keys);
        aggression = this.GetNodeCustom<HSlider>(path_aggression);
        LoadPresets();
        LoadFromPreviousData();
        AttachSignals();
    }

    private void LoadPresets()
    {
        available_presets.Add(KeyValuePair.Create("Default", GamePreset.DEFAULT));
        available_presets.Add(KeyValuePair.Create("Max Utility", GamePreset.MAX_UTILIZATION));
        available_presets.Add(KeyValuePair.Create("RNG Hell", GamePreset.RNG_HELL));

        for (int i = 0; i < available_presets.Count; i++)
        {
            var pair = available_presets[i];
            difficulty_presets.AddItem(pair.Key, i);
        }
        difficulty_presets.AddItem("Custom", available_presets.Count);
    }

    private void LoadFromPreviousData()
    {
        r_generators.SetValueNoSignal(GameDataManager.instance.RequiredGenerators);
        r_keys.SetValueNoSignal(GameDataManager.instance.RequiredKeys);
        aggression.SetValueNoSignal(GameDataManager.instance.GameAggression);
        VerifyDifficultySelection();
    }

    private void AttachSignals()
    {
        difficulty_presets.ItemSelected += OnDifficultySelected;
        r_generators.DragEnded += OnReqGeneratorsChanged;
        r_keys.DragEnded += OnReqKeysChanged;
        aggression.DragEnded += OnAggressionChanged;
    }

    private void OnDifficultySelected(int index)
    {
        if (index < 0 || index >= available_presets.Count) return;

        var preset = available_presets[index].Value;
        r_generators.SetValueNoSignal(preset.required_generators);
        r_keys.SetValueNoSignal(preset.required_keys);
        aggression.SetValueNoSignal(preset.aggression_level);
        GameDataManager.instance.RequiredGenerators = preset.required_generators;
        GameDataManager.instance.RequiredKeys = preset.required_keys;
        GameDataManager.instance.GameAggression = preset.aggression_level;
    }

    private void OnReqGeneratorsChanged(bool changed)
    {
        if (!changed) return;
        int value =  (int)r_generators.Value;
        GameDataManager.instance.RequiredGenerators = value;
        VerifyDifficultySelection();
    }

    private void OnReqKeysChanged(bool changed)
    {
        if (!changed) return;
        int value =  (int)r_keys.Value;
        GameDataManager.instance.RequiredKeys = value;
        VerifyDifficultySelection();
    }

    private void OnAggressionChanged(bool changed)
    {
        if (!changed) return;
        int value =  (int)aggression.Value;
        GameDataManager.instance.GameAggression = value;
        VerifyDifficultySelection();
    }

    private void VerifyDifficultySelection()
    {
        GD.Print($"Current Difficulty: gen={GameDataManager.instance.RequiredGenerators}, keys={GameDataManager.instance.RequiredKeys}, aggression={GameDataManager.instance.GameAggression}");

        var selected = new GamePreset(GameDataManager.instance.RequiredGenerators, GameDataManager.instance.GameAggression, GameDataManager.instance.RequiredKeys);
        for (int i = 0; i < available_presets.Count; i++)
        {
            var pair = available_presets[i];
            if (selected == pair.Value)
            {
                difficulty_presets.Select(i);
                return;
            }
        }
        difficulty_presets.Select(available_presets.Count); // index of "custom" marker
    }

}
