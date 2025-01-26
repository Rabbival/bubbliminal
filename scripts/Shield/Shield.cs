using Godot;

public partial class Shield : Area2D
{
	public override void _Ready()
    {
        BodyExited += (Node2D body) => OnBodyExited(body);
    }

	private void OnBodyExited(Node2D body)
	{
        Bubble parent = body.GetParent<Bubble>();

        DebugPrinter.Print("my parent is"  + parent.Name, LogCategory.Shield);

        parent.DeclarePositionTweenDone();
	}
}
