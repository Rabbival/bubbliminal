using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BubbleCollisionDetector : Area2D
{
	Bubble _parentBubble;

    public override void _Ready()
    {
        base._Ready();
		_parentBubble = GetParent<Bubble>();
		BodyEntered += (Node2D body) => OnBodyEntered(body);
		_parentBubble.ChainDestructionInitiated += OnChainDestructionInitiated;
    }

	private void OnBodyEntered(Node2D body) {
		Node2D _parentOfCollidingBody = body.GetParent<Node2D>();

		if(_parentOfCollidingBody == _parentBubble) return;

		DebugPrinter.Print("Body: "+ _parentOfCollidingBody.Name + " collided with: " +
			 _parentBubble.Name, LogCategory.BubbleCollision);

		if (!_parentBubble._controlledBubble) return;

		if (_parentOfCollidingBody is Bubble bubble)
		{
			DebugPrinter.Print("Detected collision with: " + bubble.Name + " for: " + _parentBubble.Name + "", LogCategory.BubbleCollision);
			bubble.Infect(_parentBubble);
		}
	}	

	private void OnChainDestructionInitiated(Bubble bubble, int lastDelayInMillis) {	
		if (bubble == _parentBubble){
			Array<Node2D> overlappingBodies = GetOverlappingBodies();
			foreach (Node2D body in overlappingBodies) {
				Bubble overlappingBubble = body.GetParent<Bubble>();
				if (overlappingBubble != _parentBubble &&
					overlappingBubble._bubbleType == BubbleType.Oil
				){
					DebugPrinter.Print("Initiating chain destruct for: " + overlappingBubble.Name, LogCategory.BubbleCollision);

					overlappingBubble.ChainDestruct(
						awaitTimeInMillis: lastDelayInMillis + 
						_parentBubble._awaitTimeIncrementationInMillis
					);
				}
			}
		}
	}
}
