using Godot;
using System;

public partial class InstructionsCanvas : CanvasLayer
{
	[Export]
	ColorRect blackScreen;

	private Node2D game; 
	public override void _Ready()
	{
		game = GetParent().GetNode<Node2D>("Game");
        
    	game.Visible = false;
		
	}

	public void HandleInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed || @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
			blackScreen.Visible = false;
			Visible = false;
        	game.Visible = true;
        }
	}
	private void OnAnyButtonPressed()
    {
		
		GD.Print("Game node is still null.");

        this.Visible = false;
        
        
        game.Visible = true;
    }
}
