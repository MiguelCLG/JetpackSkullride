using Godot;
using System;

public partial class RotationClockwise : Sprite2D
{
    [Export] float speed = 10.0f;

    public override void _PhysicsProcess(double delta)
    {
        if (RotationDegrees > 359)
            RotationDegrees = 0f;
        RotationDegrees += (float)(delta * speed);
    }
}
