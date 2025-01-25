using Godot;
using Optional;

public partial class BubblesContainer : Node2D
{
	[Export]
	private BubbleZone _bubbleZone;
	[Export]
	PackedScene _bubbleScene;
	[Export]
	MouseControl _mouseController;
	[Export]
	AudioStreamPlayer2D _shootingSound { get; set; }
	[Export]
	InstructionsCanvas _instructionsCanvas;
	[Export]
	Label _scoreLabel;

	[Signal]
	public delegate void GameWonEventHandler(int score);

	Option<Bubble> _controlledBubble;
	Vector2 _spawnPosition;
	int _controlledBubbleIndex;
	bool _shouldSpawnNewBubble;
	bool _lastShotBubbleIsMoving;
	int _shotsTakenCounter;

	public override void _Ready()
	{
		_lastShotBubbleIsMoving = false;
		_shouldSpawnNewBubble = true;
		_controlledBubbleIndex = 0;
		_spawnPosition = _mouseController.Position;
		_shotsTakenCounter = 0;
		_bubbleZone.GameWon += OnGameWon;
		UpdateLabel(_shotsTakenCounter);
	}

    public override void _Process(double delta)
    {
		if (_shouldSpawnNewBubble)
		{
			SpawnNewControlledBubble();
			_shouldSpawnNewBubble = false;
		}
    }

    public override void _Input(InputEvent @event)
    {
		if (Visible){
			if (@event is InputEventMouseButton mouseMotion)
			{
				if (mouseMotion.Pressed && mouseMotion.ButtonIndex == MouseButton.Left)
				{
					ShootControlledBubble();
				}
			}
		}else{
			if (_instructionsCanvas.Visible){
				_instructionsCanvas.HandleInput(@event);
			}
		}
    }

	private void ShootControlledBubble()
	{
		_controlledBubble.Match(
			some: bubble => {
				float rotation = _mouseController.GlobalRotation - Mathf.Pi / 2;
				Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
				Vector2 deltaVector = direction * BubblesConfig.FurthestDistance;
				Vector2 targetLocation = bubble.Position + deltaVector;
				float delta = deltaVector.Length();
				bubble.TweenPosition(targetLocation, delta);
				_controlledBubble = Option.None<Bubble>();
				_lastShotBubbleIsMoving = true;
				if (_shootingSound != null) _shootingSound.Play();
				_shotsTakenCounter++;
				UpdateLabel(_shotsTakenCounter);
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
        _bubbleZone.AddChild(newbornBubble);
		_bubbleZone.SubscribeToBubble(newbornBubble);
		newbornBubble._controlledBubble = true;
        _controlledBubble = Option.Some(newbornBubble);
		

		DebugPrinter.Print("Spawned: " + newbornBubble.Name + " at: " + _spawnPosition, LogCategory.BubbleContainer);
    }

	private void OnGameWon(){
		Visible = false;
		GD.Print("Game won! Turns taken: "+ _shotsTakenCounter);
		EmitSignal(SignalName.GameWon, _shotsTakenCounter);
	}

	private void UpdateLabel(int shotsTakenCounter){
		_scoreLabel.Text = "Turns taken: " + shotsTakenCounter;
	}
}
