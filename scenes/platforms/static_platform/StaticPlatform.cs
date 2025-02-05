using Godot;
using Godot.NativeInterop;
using System;

public partial class StaticPlatform : StaticBody2D
{
	private Sprite2D _Sprite;

	[Export]
	[ExportGroup("Properties")]
	public Texture2D _SpriteTexture;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Sprite = GetNode<Sprite2D>("Sprite2D");

		if (_SpriteTexture != null && _Sprite != null)
		{
			_Sprite.Texture = _SpriteTexture;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
