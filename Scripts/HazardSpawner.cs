using Godot;
using Godot.Collections;
using System;


public enum HazardType
{
    Floating,
    Floor,
    Ceiling,
}
public partial class HazardSpawner : Node2D
{
    [Export] Array<PackedScene> hazards;

    int SpawnHazardsCount { get; set; } = 0;
    int MaxSpawn { get; set; } = 5;

    int spawnRate = 3; // per second
    float currentTime = 0;
    Vector2 ScreenTransform { get; set; }

    public override void _Ready()
    {
        ScreenTransform = DisplayServer.WindowGetSize();
        GD.Print(ScreenTransform);
    }
    public override void _Process(double delta)
    {
        currentTime += (float)delta;

        if (currentTime > spawnRate && SpawnHazardsCount < MaxSpawn) // spawn within the spawn rate and no more than 5 hazards
        {
            SpawnHazard(PickHazardToSpawn());
            currentTime = 0;
        }
        SpawnHazardsCount = GetChildCount();
    }

    private PackedScene PickHazardToSpawn()
    {
        var rng = new Random().Next(0, hazards.Count);
        return hazards[rng];
    }

    private void SpawnHazard(PackedScene hazard)
    {
        var hazardInstance = hazard.Instantiate<Node2D>();
        if(hazardInstance is Hazard h){
            if(h.type == HazardType.Floating)
            {
                hazardInstance.Scale = new Vector2(.5f, .5f);
                hazardInstance.GlobalPosition = new Vector2(ScreenTransform.X + new Random().Next(200, 500), 0 + new Random().Next(100, Mathf.RoundToInt(ScreenTransform.Y - 100)));
            }
            if (h.type == HazardType.Floor)
            {
                hazardInstance.Scale = new Vector2(.5f, .5f);
                hazardInstance.GlobalPosition = new Vector2(ScreenTransform.X + new Random().Next(200, 500), ScreenTransform.Y - 95);
            }
            if (h.type == HazardType.Ceiling)
            {
                hazardInstance.Scale = new Vector2(.5f, .5f);
                hazardInstance.GlobalPosition = new Vector2(ScreenTransform.X + new Random().Next(200, 500), 64);
            }

            AddChild(hazardInstance);
        }
    }
}
