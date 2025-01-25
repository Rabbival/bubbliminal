using Godot;
using Optional;
using System;

public partial class BubblesContainer : Node2D
{
	[Export]
	PackedScene _bubbleScene;
	[Export]
	MouseControl _mouseController;

	Option<Bubble> _controlledBubble;
	Vector2 _spawnPosition;
	int _controlledBubbleIndex;
	bool _shouldSpawnNewBubble;
	bool _lastShotBubbleIsMoving;

	public override void _Ready()
	{
		_lastShotBubbleIsMoving = false;
		_shouldSpawnNewBubble = true;
		_controlledBubbleIndex = 0;
		_spawnPosition = _mouseController.Position;
	}

    public override void _Process(double delta)
    {
        base._PhysicsProcess(delta);
		if (_shouldSpawnNewBubble)
		{
			SpawnNewControlledBubble();
			_shouldSpawnNewBubble = false;
		}
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
				Vector2 deltaVector = (mousePosition - bubble.GlobalPosition).Normalized() * BubblesConfig.FurthestDistance;
				Vector2 targetLocation = bubble.GlobalPosition + deltaVector;
				float delta = deltaVector.Length();
				bubble.TweenPosition(targetLocation, delta);
				_controlledBubble = Option.None<Bubble>();
				_lastShotBubbleIsMoving = true;
			}, 
			none: () => {
				if (!_lastShotBubbleIsMoving){
					_shouldSpawnNewBubble = true;
				}
			}
		);
	}

	private void ListenForControlledBubbleMovementDone(Bubble controlledBubble)
	{
		controlledBubble.PositionTweenDone += OnControlledBubbleMovementDone;
	}

	private void OnControlledBubbleMovementDone(Bubble controlledBubble)
	{
		_lastShotBubbleIsMoving = false;
		_shouldSpawnNewBubble = true;
	}

	private void SpawnNewControlledBubble(){
		_lastShotBubbleIsMoving = false;
		
        Bubble newbornBubble = _bubbleScene.Instantiate<Bubble>();
		newbornBubble.Name = "ControlledBubble" + _controlledBubbleIndex;
        newbornBubble._bubbleType = ChanceManager.GetNextBubbleType();
		newbornBubble.Position = _spawnPosition;
		_controlledBubbleIndex++;

		ListenForControlledBubbleMovementDone(newbornBubble);
        AddChild(newbornBubble);
		newbornBubble._controlledBubble = true;
        _controlledBubble = Option.Some(newbornBubble);

		DebugPrinter.Print("Spawned: " + newbornBubble.Name + " at: " + _spawnPosition, LogCategory.BubbleContainer);
    }
}
