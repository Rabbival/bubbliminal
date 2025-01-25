using Godot;
using System;

public partial class BubblesConfig : Node
{
	[Export]
	float _shotDurationMultiplier = 0.6f;
	[Export]
	float _targetScaleOnExplosion = 2.4f;
	[Export]
	float _despawnDuration = 0.15f;
	[Export]
	Texture2D _oilBubbleTexture;
	[Export]
	Texture2D _fireBubbleTexture;
	[Export]
	Texture2D _neutralBubbleTexture;

	public static float ShotDurationMultiplier;
	public static float TargetScaleOnExplosion;
	public static float DespawnDuration;
	public static float FurthestDistance;
	public static Texture2D OilTexture;
	public static Texture2D FireTexture;
	public static Texture2D NeutralTexture;


    public override void _Ready()
    {
        ShotDurationMultiplier = _shotDurationMultiplier;
		TargetScaleOnExplosion = _targetScaleOnExplosion;
		DespawnDuration = _despawnDuration;
		OilTexture = _oilBubbleTexture;
		FireTexture = _fireBubbleTexture;
		NeutralTexture = _neutralBubbleTexture;
		FurthestDistance = GetViewport().GetVisibleRect().Size.Length();
    }
}
