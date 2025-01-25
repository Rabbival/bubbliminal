using Godot;
using System;

public partial class BubbleZone : Node2D
{
	private Random random = new Random();
	bool _countChildrenNextFrame;

    [Signal]
    public delegate void GameWonEventHandler();

	public override void _Ready()
	{
        _countChildrenNextFrame = false;
		foreach (Sprite2D child in GetChildren())
        {
			double randomNumber = random.NextDouble();
            if (child is Bubble bubble)
            {
				bubble.WasDestoryed += (Bubble bubble) => _countChildrenNextFrame = true;
				if (randomNumber > 0.9)
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

    public override void _Process(double _delta)
    {
        if (_countChildrenNextFrame){
            _countChildrenNextFrame = false;
            DeclareGameDoneByChildCount();
        }
    }

    public void SubscribeToBubble(Bubble bubble){
        bubble.WasDestoryed += (Bubble bubble) => DeclareGameDoneByChildCount();
    }

	private void DeclareGameDoneByChildCount(){
       if (GetChildCount() <= 1){
           EmitSignal(SignalName.GameWon);
       }
    }
}
