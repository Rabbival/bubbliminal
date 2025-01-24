using Godot;
using System;

public partial class Bubble : Sprite2D
{
	[Export]
	BubbleType bubbleType;
	
	public override void _Ready()
	{
		SetColorByBubbleType(bubbleType);
	}

	public void SetColorByBubbleType(BubbleType bubbleType)
	{
		switch (bubbleType)
		{
			case BubbleType.Neutral:
				Modulate = Colors.White;
				break;
			case BubbleType.Fire:
				Modulate = Colors.Red;
				break;
			case BubbleType.Oil:
				Modulate = Colors.Yellow;
				break;
		}
	}
}
