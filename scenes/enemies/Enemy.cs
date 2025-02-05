using BounciesBunnies.scenes.bunny.state_machine;
using BounciesBunnies.scenes.enemies.state_machine;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.TextServer;

namespace BounciesBunnies.scenes.enemies
{
    public partial class Enemy : CharacterBody2D
    {
        #region Properties

        [Export]
        [ExportGroup("Statistics")]
        private int _Lifes = 3;

        [Export]
        [ExportGroup("Movement")]
        private float _Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

        [Export]
        [ExportGroup("Movement")]
        private float _Distance = 100f;

        [Export]
        [ExportGroup("Movement")]
        private float _Speed = 100f;

        [Export]
        [ExportGroup("Movement")]
        internal bool _IsTriggered = false;

        [Export]
        [ExportGroup("Enemy Animations")]
        private float _HurtTime = 1f;

        [Export]
        [ExportGroup("Enemy Animations")]
        public bool _IsHurt = false;

        [Export]
        [ExportGroup("Enemy Animations")]
        private float _HurtImpulse = 50f;

        [Export]
        [ExportGroup("Enemy Animations")]
        public bool _IsAttacking = false;

        [Export]
        [ExportGroup("Enemy Animations")]
        private float _AttackingTime = 1f;

        [Export]
        [ExportGroup("Attack")]
        private float _AttackRange = 150f;

        internal AnimationTree _AnimationTree;
        internal Timer _Timer;
        internal EnemyStateMachine _EnemyStateMachine;


        internal Vector2 _StartPosition;
        internal float _Direction;
        internal Bunny _Bunny;

        #endregion

        #region _Ready()

        public override void _Ready()
        {
            _AnimationTree.Active = true;
            _AnimationTree.Set("parameters/conditions/move", true);

            _StartPosition = Position;
            _Direction = -1f;
        }

        #endregion

        #region _PhysicsProcess()

        public override void _PhysicsProcess(double delta)
        {
            if (_Lifes == 0)
                QueueFree();

            _Move(delta);

            MoveAndSlide();

        }

        #endregion

        #region _Move()

        private void _Move(double delta)
        {
            Vector2 velocity = Velocity;
            if (!_IsTriggered)
            {
                if (Position.X < _StartPosition.X - _Distance)
                    _Direction = 1f;
                else if (Position.X > _StartPosition.X + _Distance)
                    _Direction = -1f;
            }
            else
            {
                if (_Bunny != null)
                {
                    if (Position.X > _Bunny.Position.X)
                        _Direction = -1f;
                    else
                        _Direction = 1f;
                }
            }

            if (!_IsHurt)
            {

                if (_Bunny != null && Math.Abs(Position.X - _Bunny.Position.X) <= _AttackRange && !_IsAttacking)
                    _IsAttacking = true;

                velocity.X = _Direction * _Speed;

                velocity.Y += _Gravity * (float)delta;

            }
            else
                velocity.Y += _Gravity * (float)delta;

            _AnimationTree.Set("parameters/Move/blend_position", _Direction);

            Velocity = velocity;
        }

        #endregion

        #region _Hurt()

        public virtual void _Hurt(float direction)
        {
            if (_Lifes > 0)
            {
                _Lifes -= 1;
                _IsAttacking = false;
                _IsHurt = true;
                _EnemyStateMachine._PreviousState = _EnemyStateMachine._CurrentState;
                Velocity = new Vector2(direction * _HurtImpulse, -_HurtImpulse);
                _Timer.Start(_HurtTime);
            }
        }

        #endregion

        #region _EnemyTriggered()

        public virtual void _EnemyTriggered(Bunny bunny)
        {
            _IsTriggered = true;
            _Bunny = bunny;
            _Speed = 2 * _Speed;    

            if (bunny.Position.X < Position.X)
                _Direction = -1f;
            else
                _Direction = 1f;
        }

        #endregion

        #region _OnTimerTimeout

        public void _OnTimerTimeout()
        {
            if (_IsHurt)
            {
                _IsHurt = false;
                if (_EnemyStateMachine._PreviousState is Attack attack)
                    _EnemyStateMachine._SwitchStates(attack._NextAttackState);
                else
                    _EnemyStateMachine._SwitchStates(_EnemyStateMachine._PreviousState);
                _EnemyStateMachine._PreviousState = null;
            }
            else if (_IsAttacking)
            {
                _IsAttacking = false;
                if (_EnemyStateMachine._CurrentState is Attack attack)
                    attack._NextState = attack._NextAttackState;
            }
        }

        #endregion

        #region _Attack()

        public void _Attack(float direction)
        {
            _AnimationTree.Set("parameters/Attack/blend_position", direction);
            _Timer.Start(_AttackingTime);
        }

        #endregion
    }
}
