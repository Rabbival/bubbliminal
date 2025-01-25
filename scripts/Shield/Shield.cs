using Godot;
using System;

public partial class Shield : Area2D
{
	// // Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
	 public override void _Ready()
    {
        GD.Print("Shield node is ready.");
		//base._Ready();
		BodyExited += OnWhatever2;
		// BodyEntered += OnWhatever;
		
    }

	private void OnWhatever2(Node body)
	{
		GD.Print("Exited!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!dsfn,sdbfmsd,bnfs89sd");
	}

	private void OnWhatever(Node body)
    {
        GD.Print("A body has entered the area.");
        if (body is CollisionObject2D)
        {
            GD.Print("A body with CollisionObject2D has entered the area!");
        }
    }

    private void _on_Area2D_body_exited(Node body)
    {
        GD.Print("A body has exited the area.");
        if (body is CollisionObject2D)
        {
            GD.Print("A body with CollisionObject2D has exited the area!");
        }
    }
}
