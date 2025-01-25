using Godot;
using System;

public partial class ArrowCollider : Area2D
{
    public override void _Ready()
    {
        BodyEntered += (Node2D body) => OnBodyEntered(body);
    }

    private void OnBodyEntered(Node2D body){
        Node2D _parentOfCollidingBody = body.GetParent<Node2D>();

		if (_parentOfCollidingBody is Bubble bubble)
		{
			if (!bubble._controlledBubble){
                bubble.ExplodeThenDestory();
            }
		}
    }
}
