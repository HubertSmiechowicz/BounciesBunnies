using BounciesBunnies.scenes.enemies;
using BounciesBunnies.scenes.enemies.state_machine;
using Godot;
using System;

public partial class SpikeMan : Enemy
{
    [Export]
    [ExportGroup("Movement")]
    private StaticPlatform _Platform;
    
	public override void _Ready()
	{
		_AnimationTree = GetNode<AnimationTree>("AnimationTree");
		_Timer = GetNode<Timer>("Timer");
		_EnemyStateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

    internal override void _Move(double delta)
    {
        _GetDirection();
        base._Move(delta);
    }

    public void _OnBunnyEntered(Node2D body)
	{
		if (body is Bunny bunny)
		{
			var direction = 1f;

			if (bunny.Position.X > Position.X)
				direction = 1f;
			else
				direction = -1f;

			bunny._Hurt(direction);
		}

	}

    #region _GetDirection()

    private void _GetDirection()
    {
        if (_Platform != null)
        {
            float halfSize = (_Platform._Sprite.Texture.GetSize().X * _Platform._Sprite.Scale.X) / 2 + 50;

            if (!_IsTriggered)
            {
                if (Position.X <= _Platform.Position.X - halfSize)
                    _Direction = 1f;
                else if (Position.X >= _Platform.Position.X + halfSize)
                    _Direction = -1f;
            }
            else
            {
                if (_Bunny != null)
                {
                    if (Position.X < _Platform.Position.X - halfSize || Position.X > _Platform.Position.X + halfSize)
                    {
                        _IsTriggered = false;
                        _Speed = _Speed / 2;
                    }

                    if (Math.Abs(Position.X - _Bunny.Position.X) < _AttackRange && !_IsAttacking)
                    {
                        if (Position.X > _Bunny.Position.X)
                            _Direction = 1f;
                        else
                            _Direction = -1f;
                    }
                    else if (Position.X > _Bunny.Position.X)
                        _Direction = -1f;
                    else
                        _Direction = 1f;
                }
            }
        }
    }

    #endregion
}
