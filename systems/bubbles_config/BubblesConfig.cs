using Godot;
using System;

public partial class BubblesConfig : Node
{
	[Export]
	float _shotDurationMultiplier = 0.14f;
	[Export]
	float _targetScaleOnExplosion = 2.4f;
	[Export]
	float _despawnDuration = 0.15f;

	public static float ShotDurationMultiplier;
	public static float TargetScaleOnExplosion;
	public static float DespawnDuration;
	public static float FurthestDistance;

    public override void _Ready()
    {
        ShotDurationMultiplier = _shotDurationMultiplier;
		TargetScaleOnExplosion = _targetScaleOnExplosion;
		DespawnDuration = _despawnDuration;
		FurthestDistance = GetViewport().GetVisibleRect().Size.Length();
    }
}
