using Godot;
using System;

public partial class LevelManagement : Node2D
{
    int totalScore = 0;
    int meters = 0;
    int highScore = 0;
    int highScoreMeters = 0;

    float time = 0;
    Label ScoreLabel { get; set; }
    Label MetersLabel { get; set; }
    Label HighScoreLabel { get; set; }
    Label HighScoreMetersLabel { get; set; }

    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("%ScoreLabel");
        MetersLabel = GetNode<Label>("%MetersLabel");
        HighScoreLabel = GetNode<Label>("%HighScoreLabel");
        HighScoreMetersLabel = GetNode<Label>("%HighScoreMetersLabel");
        RegisterEvents();
        LoadGame();
    }

    public override void _Process(double delta)
    {
        time += (float)delta;
        if(time > 1)
        {
            meters += Mathf.RoundToInt(time);
            totalScore += Mathf.RoundToInt(time);
            time = 0;
            if(meters >= highScoreMeters)
            {
                highScoreMeters = meters;
            }

            if(totalScore >= highScore)
            {
                highScore = totalScore;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreLabel.Text = $"Score: {totalScore}";
        MetersLabel.Text = $"Meters: {meters}m";
        HighScoreLabel.Text = $"HighScore: {highScore}";
        HighScoreMetersLabel.Text = $"Most Meters Reached: {highScoreMeters}m";
    }
    private void RestartGame()
    {
        UnregisterEvents();
        GetTree().CallDeferred("reload_current_scene");
    }

    void RegisterEvents()
    {
        EventRegistry.RegisterEvent("OnEnemyHitWithPewPew");
        EventSubscriber.SubscribeToEvent("OnEnemyHitWithPewPew", OnEnemyHitWithPewPew);
        EventRegistry.RegisterEvent("OnPlayerDie");
        EventSubscriber.SubscribeToEvent("OnPlayerDie", OnPlayerDie);
    }

    void UnregisterEvents()
    {
        EventSubscriber.UnsubscribeFromEvent("OnEnemyHitWithPewPew", OnEnemyHitWithPewPew);
        EventSubscriber.UnsubscribeFromEvent("OnPlayerDie", OnPlayerDie);
    }

    private void OnPlayerDie(object sender, object obj)
    {
        SaveGame();
        RestartGame();
    }

    void LoadGame()
    {
        if (!FileAccess.FileExists("user://savegame.save"))
        {
            GD.PrintErr("No save file.");
            return; // Error! We don't have a save to load.
        }

        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
        while (saveGame.GetPosition() < saveGame.GetLength())
        {
            var jsonString = saveGame.GetLine();

            var json = new Json();
            var parseResult = json.Parse(jsonString);

            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

            foreach (var (key, value) in nodeData)
            {
                if(key == "HighScore"){
                    HighScoreLabel.Text = $"High Score: {value}";
                    highScore = (int)value;
                }
                else if (key == "HighScoreMeters")
                {
                    HighScoreMetersLabel.Text = $"Most Meters Reached: {value}";
                    highScoreMeters = (int)value;
                }

            }
        }
    }

    private void SaveGame()
    {
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
        var saveNodes = GetTree().GetNodesInGroup("Persist");

        foreach (var node in saveNodes)
        {
            var nodeData = node.Call("Save");
            var jsonString = Json.Stringify(nodeData);

            saveGame.StoreLine(jsonString);
        }
    }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { "HighScore", highScore },
            { "HighScoreMeters", highScoreMeters }
        };
    }

    private void OnEnemyHitWithPewPew(object sender, object obj)
    {
        totalScore += 5;
        UpdateUI();
    }
}
