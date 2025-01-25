using Godot;
using System;

public partial class InstructionsCanvas : CanvasLayer
{
	private Node2D game; 
	public override void _Ready()
	{
		game = GetParent().GetNode<Node2D>("Game");
        
    	game.Visible = false;
		
	}

	public override void _Input(InputEvent @event)
	{
		//GD.Print("Game node is still null.");
		if (@event is InputEventKey keyEvent && keyEvent.Pressed || @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GD.Print("Input received: ");
            GD.Print("Game node is still null.");
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
