using Godot;
using System;

public partial class MouseControl : Node2D
{
	private Vector2 originalScale;
    private Vector2 originalPosition;
	private float timer = 0f;
	private float scaleTime = 0.2f;
	private float currentAngle = 0f;
	private bool isScaling = false;
    private float scaleDuration = 0.1f;
    private float scaleTimer = 0f;


		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalScale = Scale;
        originalPosition = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isScaling)
        {
            timer += (float)delta;

    
            if (timer >= scaleTime)
            {
                Scale = originalScale; 
                Position = originalPosition; 
                isScaling = false;
            }
        }
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

	public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
				Scale = new Vector2(Scale.X, Scale.Y * 0.8f);
				Position = new Vector2(Position.X, Position.Y + 10);

				isScaling = true;
                timer = 0f;
            }
        }
    }
}
