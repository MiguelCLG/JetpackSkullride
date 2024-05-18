using Godot;
using System;

public partial class Skull : RigidBody2D
{
    GpuParticles2D jetpackParticles;
    Sprite2D skull;

    PackedScene pewpews = GD.Load<PackedScene>("res://Scenes/pewpew.tscn");

    bool IsDead { get; set; } = false;
    public override void _Ready()
    {
        jetpackParticles = GetNode<GpuParticles2D>("%JetpackParticles");
        skull = GetNode<Sprite2D>("SkullSprite");
    }
    public override void _PhysicsProcess(double delta)
    {
        if(IsDead)
        {
            if (Input.IsActionPressed("ui_accept"))
                GetTree().CallDeferred("reload_current_scene");
            return; 
        }
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
            ApplyImpulse(new Vector2(0, 1));
        }
    }

    public void CollidedWithHazard()
    {
        IsDead = true;
        var pauseNodes = GetTree().GetNodesInGroup("CanBePaused");

        foreach (var node in pauseNodes)
        {
            node.SetProcess(false);
            node.SetPhysicsProcess(false);
        }
        Action action = () => EventRegistry.GetEventPublisher("OnPlayerDie").RaiseEvent(this);
        Utils.TimerUtils.CreateTimer(action, this, 3f);
    }

    
    public void SpawnPewPews()
    {
        var pewpewInstance = pewpews.Instantiate<PewPew>();
        pewpewInstance.Position = new Vector2(new Random().Next(-10, 50), 30);
        pewpewInstance.Rotation = new Random().Next((int)Mathf.DegToRad(-45), (int)Mathf.DegToRad(45));
        AddChild(pewpewInstance);
    }

    public void OnCollisionEnter(Node Body)
    {
        if (Body.IsInGroup("Hazard") && Body is Node2D node)
        {
            var impulseDirection = node.Position - Position;
            SetDeferred("lock_rotation", false);
            ApplyImpulse(impulseDirection * 5f);
            CollidedWithHazard();
        }
    }
}
