using Godot;
using System;

public partial class Instructions : Label
{
	[Export]
	private float moveSpeed = 50f; 

	[Export]
    private float timeToMove = 3f; 

    private float timer = 0f;


	public override void _Process(double delta)
	{
		timer += (float)delta;

       
        if (timer < timeToMove)
        {
            Position = new Vector2(Position.X, Position.Y - (float)(moveSpeed * delta));
        }
	}
}
