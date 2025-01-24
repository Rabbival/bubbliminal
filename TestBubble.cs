using Godot;
using System;

public partial class TestBubble : Bubble
{
	public override void _Ready()
	{
		base._Ready();
		_controlledBubble = true; //for testing
	}

	public override void _Process(double delta)
	{
		Position += new Vector2(0, 2);
	}
}
