using Godot;
using System;

public partial class FallingArea : Node3D
{
    Node3D control;
    bool falling = false;
    public override void _Ready()
    {
        control = GetNode<Node3D>("Control");
    }
    public void _on_area_3d_body_entered(Node3D body)
    {
        if (body is Player && !falling)
        {
            StartFalling();
        }
    }
    async void StartFalling()
    {
        falling = true;

        await ToSignal(GetTree().CreateTimer(0.75f), "timeout");

        RemoveChild(control);

        await ToSignal(GetTree().CreateTimer(5f), "timeout");

        AddChild(control);

        falling = false;
    }
}