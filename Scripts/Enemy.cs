using Godot;
using System;

public partial class Enemy : RigidBody2D
{
    float currentTime { get; set; } = 0;
    int jumpRate { get; set; } = 1;

    [Export] float jumpForce { get; set; } = 10f;
    [Export] float speed { get; set; } = 150f;
    public override void _PhysicsProcess(double delta)
    {
        currentTime += (float)delta;

        if (currentTime > jumpRate) // spawn within the spawn rate and no more than 5 hazards
        {
            ApplyImpulse(new Vector2(0, -jumpForce));
            currentTime = 0;
        }
        if (GlobalPosition.X < -300)
        {
            GetParent().RemoveChild(this);
        }
        GlobalPosition -= new Vector2((float)delta * speed, 0);
    }

    public void OnBodyEntered(Node body)
    {
        if(body is PewPew)
        {
            GetParent().CallDeferred("remove_child", this);
            Dispose();            
            GD.Print("Enemy hit");
            EventRegistry.GetEventPublisher("OnEnemyHitWithPewPew").RaiseEvent(this);
        }
        
    }
}
