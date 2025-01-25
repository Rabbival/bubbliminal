using Godot;
using System;

public partial class InstructionsCanvas : CanvasLayer
{
	[Export]
	ColorRect blackScreen;
	[Export]
	BubblesContainer bubblesContainer;

	public override void _Ready()
	{
    	bubblesContainer.Visible = false;
	}

	public void HandleInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed || @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
			blackScreen.Visible = false;
			Visible = false;
        	bubblesContainer.Visible = true;
        }
	}
	private void OnAnyButtonPressed()
    {
		
		GD.Print("Game node is still null.");

        this.Visible = false;
        
        
        bubblesContainer.Visible = true;
    }
}
