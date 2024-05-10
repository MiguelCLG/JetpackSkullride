using Godot;
using System;

public partial class Skull : RigidBody2D
{
    GpuParticles2D jetpackParticles;
    Sprite2D skull;

    PackedScene pewpews = GD.Load<PackedScene>("res://Scenes/pewpew.tscn");

    public override void _Ready()
    {
        jetpackParticles = GetNode<GpuParticles2D>("%JetpackParticles");
        skull = GetNode<Sprite2D>("SkullSprite");
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("jump"))
        {
            LinearVelocity = Vector2.Zero;
            ApplyImpulse(new Vector2(0, -300));
            jetpackParticles.Emitting = true;
            skull.GlobalRotation = -20;
            SpawnPewPews();
        }
        else
        {
            jetpackParticles.Emitting = false;
            skull.GlobalRotation = 0;
        }
    }

    public void SpawnPewPews()
    {
        var pewpewInstance = pewpews.Instantiate<PewPew>();
        pewpewInstance.Position = new Vector2(new Random().Next(-10, 50), 30);
        pewpewInstance.Rotation = new Random().Next((int)Mathf.DegToRad(-45), (int)Mathf.DegToRad(45));
        AddChild(pewpewInstance);
    }
}
