using Godot;
using System;
using System.Transactions;

public partial class MovingPlatform : Path2D
{
	[Export]
	[ExportGroup("Properties")]
	public Texture2D _SpriteTexture;

	[Export]
	[ExportGroup("Movement")]
	private bool _Loop = true;

	[Export]
	[ExportGroup("Movement")]
	private float _Speed = 2.0f;

	[Export]
	[ExportGroup("Movement")]
	private float _SpeedScale = 1.0f;


	private Sprite2D _Sprite;
	private PathFollow2D _PathFollow2D;
	private AnimationPlayer _AnimationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_PathFollow2D = GetNode<PathFollow2D>("PathFollow2D");
		_AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_Sprite = GetNode<Sprite2D>("AnimatableBody2D/Sprite2D");

		if (_SpriteTexture != null && _Sprite != null)
		{
			_Sprite.Texture = _SpriteTexture;
		}

		if (!_Loop)
		{
			_AnimationPlayer.Play("move");
			_AnimationPlayer.SpeedScale = _SpeedScale;
			SetProcess(false);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_PathFollow2D.Progress += _Speed;
	}
}
