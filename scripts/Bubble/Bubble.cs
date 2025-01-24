using Godot;
using System;
using Optional;

[GlobalClass]
public partial class Bubble : Sprite2D
{
	[Export]
	BubbleType _bubbleTypeValue;
	BubbleType _bubbleType{
		get => _bubbleTypeValue;
		set => OnBubbleTypeSet(value);
	}

	[Export]
	float _tweenDuration = 0.5f;

	public bool _controlledBubble{get; protected set;}
	Option<Tween> _activePositionTween;

	[Signal]
	public delegate void PositionTweenDoneEventHandler(Bubble bubble);
	
	public override void _Ready()
	{
		base._Ready();
		_controlledBubble = false;
		_activePositionTween = Option.None<Tween>();
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
		_activePositionTween = Option.None<Tween>();
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

	public void TweenPosition(Vector2 targetPosition, float fullPositionDelta){
		_activePositionTween = Option.Some(TweenProperty(
			Position,
			targetPosition,
			fullPositionDelta,
			new Callable(this, MethodName.SetPosition),
			_activePositionTween,
			Option.Some(DeclarePositionTweenDone)
		));
	}

	private Tween TweenProperty(
		Vector2 currentValue, 
		Vector2 targetValue,
		float fullDelta, 
		Callable functionToFeed, 
		Option<Tween> maybeActiveTween,
		Option<Action> maybeCallOnceDone
	){
		float delta = 0.0f;
		Tween newbornTween = CreateTween();
		delta = currentValue.DistanceTo(targetValue);
		maybeActiveTween.MatchSome(activeTween => activeTween.Kill());
		newbornTween.TweenMethod(
			functionToFeed, 
			currentValue, 
			targetValue, 
			_tweenDuration * (delta / fullDelta)
		);
		maybeCallOnceDone.MatchSome(callOnceDone => AwaitDoneTweenWith(newbornTween, callOnceDone));
		return newbornTween;
	}

	async void AwaitDoneTweenWith(Tween awaitMe, Action callOnceDone){
		await ToSignal(awaitMe, Tween.SignalName.Finished);
		callOnceDone();
	}

	private void DeclarePositionTweenDone(){
		_activePositionTween = Option.None<Tween>();
		EmitSignal(SignalName.PositionTweenDone, this);
	}
}
