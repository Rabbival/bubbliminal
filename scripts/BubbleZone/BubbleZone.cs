using Godot;
using System;

public partial class BubbleZone : Node2D
{
	private Random random = new Random();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Sprite2D child in GetChildren())
        {
			double randomNumber = random.NextDouble();
            if (child is Bubble bubble)
            {
				
				if (randomNumber > 0.8)
                {
                    bubble._bubbleType = BubbleType.Oil;
                }
                else
                {
                    bubble._bubbleType = BubbleType.Neutral;
                }
                bubble.SetColorByBubbleType(bubble._bubbleType); 
            }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
