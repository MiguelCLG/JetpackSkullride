using Godot;
using System;

public partial class Hazard : Node2D
{
    [Export] public HazardType type;
    [Export] float speed = 10f;
    Vector2 initialPosition;

    public override void _Ready()
    {
        initialPosition = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        if (GlobalPosition.X < -300)
        {
            GetParent().RemoveChild(this);
        }
        GlobalPosition -= new Vector2((float)delta * speed, 0);
    }

    public void OnCollisionEnter(Node2D Body)
    {
        if(Body is Skull skull)
        {
            var impulseDirection = skull.Position - Position;
            if (skull is RigidBody2D rb)
                rb.SetDeferred("lock_rotation", false);
            skull.ApplyImpulse(impulseDirection * 5f);
            skull.CollidedWithHazard();
        }
    }
}
