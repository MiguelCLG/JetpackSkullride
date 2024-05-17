using Godot;
using System;

public partial class LevelManagement : Node2D
{
    [Export] int totalScore = 0;
    [Export] int meters = 0;

    float time = 0;
    Label ScoreLabel { get; set; }
    Label MetersLabel { get; set; }

    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("%ScoreLabel");
        MetersLabel = GetNode<Label>("%MetersLabel");
        RegisterEvents();
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        time += (float)delta;
        if(time > 1)
        {
            meters += Mathf.RoundToInt(time);
            totalScore += Mathf.RoundToInt(time);
            time = 0;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreLabel.Text = $"Score: {totalScore}";
        MetersLabel.Text = $"Meters: {meters}m";
    }

    void RegisterEvents()
    {
        EventRegistry.RegisterEvent("OnEnemyHitWithPewPew");
        EventSubscriber.SubscribeToEvent("OnEnemyHitWithPewPew", OnEnemyHitWithPewPew);
    }

    private void OnEnemyHitWithPewPew(object sender, object obj)
    {
        GD.Print("Level: Enemy hit");
        totalScore += 5;
        UpdateUI();
    }
}
