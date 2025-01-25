using Godot;
using System;

public partial class BubbleCount : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private BubbleZone _bubbleZone;
	public override void _Ready()
	{
		GD.Print("Number of children in BubbleZone: " + _bubbleZone.GetChildCount());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
