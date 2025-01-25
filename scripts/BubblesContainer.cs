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
	int _controlledBubbleIndex;

	public override void _Ready()
	{
		_controlledBubbleIndex = 0;
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
		newbornBubble.Name = "ControlledBubble" + _controlledBubbleIndex;
        newbornBubble._bubbleType = ChanceManager.GetNextBubbleType();
		newbornBubble.Position = _spawnPosition;
		_controlledBubbleIndex++;

		ListenForControlledBubbleMovementDone(newbornBubble);
        AddChild(newbornBubble);
		newbornBubble._controlledBubble = true;
        _controlledBubble = Option.Some(newbornBubble);

		DebugPrinter.Print("spawned a new bubble at: " + _spawnPosition, LogCategory.BubbleContainer);
    }
}
