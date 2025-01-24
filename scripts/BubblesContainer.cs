using Godot;
using Optional;
using System;

public partial class BubblesContainer : Node2D
{
	[Export]
	Bubble _firstControlledBubble;
	[Export]
	float _bubbleShotDurationMultiplier = 0.14f;
	[Export]
	PackedScene _bubbleScene;

	Option<Bubble> _controlledBubble;
	Vector2 _spawnPosition;

	public override void _Ready()
	{
		_firstControlledBubble._controlledBubble = true;
		_controlledBubble = Option.Some(_firstControlledBubble);
		ListenForControlledBubbleMovementDone(_firstControlledBubble);
		_spawnPosition = _firstControlledBubble.GlobalPosition;
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseMotion)
		{
			if (mouseMotion.Pressed && mouseMotion.ButtonIndex == MouseButton.Left)
			{
				ShootControlledBubble();
			}
		}
    }

	private void ShootControlledBubble()
	{
		_controlledBubble.Match(
			some: bubble => {
				Vector2 mousePosition = GetViewport().GetMousePosition();
				float delta = (mousePosition - bubble.GlobalPosition).Length();
				bubble.TweenPosition(mousePosition, delta, _bubbleShotDurationMultiplier);
			}, 
			none: () => GD.PushWarning("No controlled bubble")
		);
	}

	private void ListenForControlledBubbleMovementDone(Bubble controlledBubble)
	{
		controlledBubble.PositionTweenDone += OnControlledBubbleMovementDone;
	}

	private void OnControlledBubbleMovementDone(Bubble controlledBubble)
	{
		controlledBubble._controlledBubble = false;
		Bubble newbornBubble = SpawnNewControlledBubble();
		_controlledBubble = Option.Some(newbornBubble);
		ListenForControlledBubbleMovementDone(newbornBubble);
	}

	private Bubble SpawnNewControlledBubble(){
        Bubble newbornBubble = _bubbleScene.Instantiate<Bubble>();
		newbornBubble.Position = _spawnPosition;
		newbornBubble._controlledBubble = true;
        newbornBubble._bubbleType = ChanceManager.GetNextBubbleType();
        AddChild(newbornBubble);
        return newbornBubble;
    }
}
