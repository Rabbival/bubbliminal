using Godot;
using System;

public partial class BubbleZone : Node2D
{
	private Random random = new Random();
	bool _countChildren;

    [Signal]
    public delegate void GameWonEventHandler();

	public override void _Ready()
	{
        _countChildren = true;
		foreach (Sprite2D child in GetChildren())
        {
			double randomNumber = random.NextDouble();
            if (child is Bubble bubble)
            {
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
        if (_countChildren){
            DeclareGameDoneByChildCount();
        }
    }

	private void DeclareGameDoneByChildCount(){
       if (GetChildCount() <= 1){
            _countChildren = false;
           EmitSignal(SignalName.GameWon);
       }
    }
}
