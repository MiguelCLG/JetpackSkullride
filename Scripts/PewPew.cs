using Godot;
using System;

public partial class PewPew : RigidBody2D
{
    [Export] float projectileSpeed = 10f;
    public override void _PhysicsProcess(double delta)
    {
        var vector = new Vector2(Vector2.FromAngle(Rotation).X, 100);
        ApplyForce(vector * projectileSpeed);
    }

    void OnBodyEntered(Node body)
    {
        if (body.HasMethod("OnBodyEntered"))
            body.Call("OnBodyEntered", this);
        GetParent().CallDeferred("remove_child", this);
        Dispose();
    }
}
