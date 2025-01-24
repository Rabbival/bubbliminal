using Godot;
using System;

[GlobalClass]
public partial class Bubble : Sprite2D
{
	[Export]
	BubbleType _bubbleTypeValue;
	BubbleType _bubbleType{
		get => _bubbleTypeValue;
		set => OnBubbleTypeSet(value);
	}

	public bool _controlledBubble{get; protected set;}
	
	public override void _Ready()
	{
		base._Ready();
		_controlledBubble = false;
		SetColorByBubbleType(_bubbleTypeValue);
	}

	public ActionPerformed Infect(Bubble infector)
	{
		bool acted = false;
		switch (infector._bubbleType){
			case BubbleType.Neutral:
				if (_bubbleType == BubbleType.Oil){
					infector._bubbleType = BubbleType.Oil;
					acted = true;
				}
				break;
			case BubbleType.Fire:
				if (_bubbleType == BubbleType.Oil)
				{
					_bubbleType = BubbleType.Fire;
					acted = true;
				}
				break;
			case BubbleType.Oil:
				if (_bubbleType == BubbleType.Fire)
				{
					infector._bubbleType = BubbleType.Fire;
					acted = true;
				}else if (_bubbleType == BubbleType.Neutral){
					_bubbleType = BubbleType.Oil;
					acted = true;
				}
				break;
		}
		return new ActionPerformed(acted);
	}

	private void OnBubbleTypeSet(BubbleType bubbleType){
		SetColorByBubbleType(bubbleType);
		_bubbleTypeValue = bubbleType;
		DebugPrinter.Print("Set state to: " + bubbleType + "for: " + Name, LogCategory.Bubble);
	}
	
	private void SetColorByBubbleType(BubbleType bubbleType)
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
