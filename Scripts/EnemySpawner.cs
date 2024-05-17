using Godot;
using Godot.Collections;
using System;

public partial class EnemySpawner : Node2D
{

    [Export] PackedScene enemyScene;

    int SpawnEnemyCount { get; set; } = 0;
    int MaxSpawn { get; set; } = 5;

    int spawnRateMin = 3; // per second
    int spawnRateMax = 10; // per second
    int spawnRate;
    float currentTime = 0;
    Vector2 ScreenTransform { get; set; }

    public override void _Ready()
    {
        ScreenTransform = DisplayServer.WindowGetSize();
        spawnRate = new Random().Next(spawnRateMin, spawnRateMax);
    }
    public override void _Process(double delta)
    {
        currentTime += (float)delta;

        if (currentTime > spawnRate && SpawnEnemyCount < MaxSpawn) // spawn within the spawn rate and no more than 5 hazards
        {
            SpawnEnemy();
            currentTime = 0;
            spawnRate = new Random().Next(spawnRateMin, spawnRateMax);
        }
        SpawnEnemyCount = GetChildCount();
    }

    private void SpawnEnemy()
    {
        var spawnInstance = enemyScene.Instantiate<Node2D>();
        spawnInstance.GlobalPosition = new Vector2(ScreenTransform.X + 100, ScreenTransform.Y - 128);
        AddChild(spawnInstance);
    }
}
