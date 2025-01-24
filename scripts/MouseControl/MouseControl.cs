using Godot;
using System;

public partial class MouseControl : Node2D
{
	private float currentAngle = 0f;
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetViewport().GetMousePosition();


        Vector2 direction = mousePosition - GlobalPosition;


        float angle = direction.Angle() + Mathf.Pi / 2;

		angle = Mathf.Clamp(angle, -Mathf.Pi / 2, Mathf.Pi / 2);

		if (mousePosition.Y < GlobalPosition.Y)
        {
            currentAngle = angle;
        }

		
		Rotation = currentAngle;
	}
}
