using Godot;
using System;
using Optional;
using System.Threading.Tasks;

[GlobalClass]
public partial class Bubble : Sprite2D
{
	[Export]
	BubbleType _bubbleTypeValue;
	public BubbleType _bubbleType{
		get => _bubbleTypeValue;
		set => OnBubbleTypeSet(value);
	}

	public int _awaitTimeIncrementationInMillis = 60;

	public bool _controlledBubble;
	Option<Tween> _activePositionTween;
	bool _deemedForDestruction;

	[Signal]
	public delegate void PositionTweenDoneEventHandler(Bubble bubble);

	[Signal]
	public delegate void ChainDestructionInitiatedEventHandler(Bubble bubble, int lastDelayInMillis);
	
	public override void _Ready()
	{
		base._Ready();
		_controlledBubble = false;
		_deemedForDestruction = false;
		_activePositionTween = Option.None<Tween>();
		SetColorByBubbleType(_bubbleTypeValue);
	}

	public ActionPerformed Infect(Bubble infector)
	{
		if (_deemedForDestruction || infector._deemedForDestruction) return new ActionPerformed(false);

		bool acted = false;
		infector.DeclarePositionTweenDone();
		DeclarePositionTweenDone();
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
					infector._bubbleType = BubbleType.Oil;
					acted = true;
				} else if (_bubbleType == BubbleType.Neutral){
					_bubbleType = BubbleType.Fire;
					infector._bubbleType = BubbleType.Fire;
					acted = true;
				}
				break;
			case BubbleType.Oil:
				if (_bubbleType == BubbleType.Fire)
				{
					_bubbleType = BubbleType.Oil;
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
		if (_deemedForDestruction) return;

		DebugPrinter.Print("Setting state to: " + bubbleType + " for: " + Name, LogCategory.Bubble);
		HandleSpecialStateTransitions(_bubbleTypeValue, bubbleType);
		SetColorByBubbleType(bubbleType);
		_bubbleTypeValue = bubbleType;
	}

	private void HandleSpecialStateTransitions(BubbleType oldState, BubbleType newState){
		if (oldState == BubbleType.Oil && newState == BubbleType.Fire){
			ChainDestruct();
		}else if (oldState == BubbleType.Fire && newState == BubbleType.Oil){
			ChainDestruct();
		}else if (oldState == BubbleType.Neutral && newState == BubbleType.Fire){
			ExplodeThenDestory();
		}else if (oldState == BubbleType.Fire && newState == BubbleType.Neutral){
			ExplodeThenDestory();
		}
	}

	public async Task ChainDestruct(int awaitTimeInMillis = 0){
		if (_deemedForDestruction) return;

		_deemedForDestruction = true;
		await Task.Delay(awaitTimeInMillis);
		EmitSignal(SignalName.ChainDestructionInitiated, this, awaitTimeInMillis);
		ExplodeThenDestory();
	}

	public async void ExplodeThenDestory(){
		TweenScale();
		FadeOut();
		await Task.Delay((int)(BubblesConfig.DespawnDuration * 1000));
		QueueFree();
	}
	
	public void SetColorByBubbleType(BubbleType bubbleType)
	{
		if (_deemedForDestruction) return;

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

	public void TweenPosition(Vector2 targetPosition, float fullPositionDelta){
		if (_deemedForDestruction) return;
		
		_activePositionTween = Option.Some(TweenProperty(
			Position,
			targetPosition,
			fullPositionDelta,
			new Callable(this, MethodName.MoveUntilOutOfScreen),
			Option.None<Tween>(),
			Option.Some(DeclarePositionTweenDone),
			BubblesConfig.ShotDurationMultiplier
		));
	}

	public void TweenScale(){
		TweenProperty(
			Scale,
			BubblesConfig.TargetScaleOnExplosion * Scale,
			1.0f,
			new Callable(this, MethodName.SetScale),
			Option.None<Tween>(),
			Option.None<Action>(),
			BubblesConfig.DespawnDuration
		);
	}

	public void FadeOut(){
		var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate:a", 0, BubblesConfig.DespawnDuration);
	}

	private Tween TweenProperty(
		Vector2 currentValue, 
		Vector2 targetValue,
		float fullDelta, 
		Callable functionToFeed, 
		Option<Tween> maybeActiveTween,
		Option<Action> maybeCallOnceDone,
		float tweenDuration
	){
		float delta = 0.0f;
		Tween newbornTween = CreateTween();
		delta = currentValue.DistanceTo(targetValue);
		maybeActiveTween.MatchSome(activeTween => activeTween.Kill());
		newbornTween.TweenMethod(
			functionToFeed, 
			currentValue, 
			targetValue, 
			tweenDuration * (delta / fullDelta)
		);
		maybeCallOnceDone.MatchSome(callOnceDone => AwaitDoneTweenWith(newbornTween, callOnceDone));
		return newbornTween;
	}

	async void AwaitDoneTweenWith(Tween awaitMe, Action callOnceDone){
		await ToSignal(awaitMe, Tween.SignalName.Finished);
		callOnceDone();
	}

	public void DeclarePositionTweenDone(){
		_activePositionTween.MatchSome(tween => {
			tween.Kill();
			_activePositionTween = Option.None<Tween>();
			EmitSignal(SignalName.PositionTweenDone, this);
			DebugPrinter.Print("Position tween done for: " + Name, LogCategory.Bubble);	
		});
	}

	private void MoveUntilOutOfScreen(Vector2 newCalculatedPosition){
		bool outOfScreenPositiveX = newCalculatedPosition.X > GetViewportRect().Size.X;
		bool outOfScreenNegativeX = newCalculatedPosition.X < 0;
		bool outOfScreenPositiveY = newCalculatedPosition.Y > GetViewportRect().Size.Y;
		bool outOfScreenNegativeY = newCalculatedPosition.Y < 0;
		if (outOfScreenPositiveX 
			|| outOfScreenNegativeX 
			|| outOfScreenPositiveY 
			|| outOfScreenNegativeY
		){
			DeclarePositionTweenDone();
			ExplodeThenDestory();
		}else{
			Position = newCalculatedPosition;
		}
	}
}
