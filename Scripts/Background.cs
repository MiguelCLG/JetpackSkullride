using Godot;
using System;

public partial class Background : Sprite2D
{

    Vector2 initialPosition;
    [Export] float speed = 10f;
    public override void _Ready()
    {
        initialPosition = GlobalPosition;
        SetCloudColor();
        SetCloudPosition();
    }
    public override void _Process(double delta)
    {
        if (GlobalPosition.X < -100)
        { 
            GlobalPosition = initialPosition;
            SetCloudColor();
            SetCloudPosition();
        }
        GlobalPosition -= new Vector2((float)delta * speed, 0);
    }

    private void SetCloudPosition()
    {
        GlobalPosition = new Vector2(GlobalPosition.X, new Random().Next(100, 400));
    }

    private void SetCloudColor()
    {
        var rand = (float)(new Random().NextDouble());
        var grayscaleRand = rand < 0.2 ? 0.5f : rand;
        Color color = new Color(grayscaleRand, grayscaleRand, grayscaleRand, 0.8f);
        Modulate = color;
    }
}
