using Godot;
using Optional;
using System;

public partial class BubblesContainer : Node2D
{
	[Export]
	PackedScene _bubbleScene;
	[Export]
	float _bubbleShotDurationMultiplier = 0.14f;
	[Export]
	MouseControl _mouseController;

	Option<Bubble> _controlledBubble;
	Vector2 _spawnPosition;

	public override void _Ready()
	{
		_spawnPosition = _mouseController.Position;
		SpawnNewControlledBubble();
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
		SpawnNewControlledBubble();
	}

	private void SpawnNewControlledBubble(){
        Bubble newbornBubble = _bubbleScene.Instantiate<Bubble>();
		newbornBubble._controlledBubble = true;
        newbornBubble._bubbleType = ChanceManager.GetNextBubbleType();
		ListenForControlledBubbleMovementDone(newbornBubble);
        AddChild(newbornBubble);
		newbornBubble.Position = _spawnPosition;
        _controlledBubble = Option.Some(newbornBubble);

		DebugPrinter.Print("spawned a new bubble at: " + _spawnPosition, LogCategory.Bubble);
    }
}
