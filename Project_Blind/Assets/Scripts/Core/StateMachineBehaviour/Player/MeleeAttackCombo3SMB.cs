﻿using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class MeleeAttackCombo3SMB : SceneLinkedSMB<PlayerCharacter>
    {
        UI_FieldScene ui = null;
        private bool _powerAttack = false;
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.StopMoveY();
        }

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_monoBehaviour.CheckForPowerAttack() && _monoBehaviour.CurrentWaveGauge > 10)
            {
                animator.speed = 0.1f;
                _monoBehaviour.EndPowerAttack();
                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StartCharge();
                }
            }
            else
            {
                if (_monoBehaviour.isJump)
                {
                    _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * _monoBehaviour.GetFacing());
                }

                _monoBehaviour.enableAttack();
            }
        }


        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!_monoBehaviour.isJump)
            {
                _monoBehaviour.AirborneVerticalMovement(1f);
                _monoBehaviour.UpdateJump();
                _monoBehaviour.CheckForGrounded();
                _monoBehaviour.GroundedHorizontalMovement(true);
                _monoBehaviour.UpdateFacing();
            }
            else _monoBehaviour.GroundedHorizontalMovement(false);

            if (_monoBehaviour.CheckForAttack())
            {
                _monoBehaviour._lastClickTime = Time.time;
                _monoBehaviour._clickcount++;
                _monoBehaviour._clickcount = Mathf.Clamp(_monoBehaviour._clickcount, 0, 4);
            }
            
            if (_monoBehaviour.CheckForAttackTime())
                _monoBehaviour._clickcount = 0;
            if(_monoBehaviour._clickcount>=4)
                _monoBehaviour.MeleeAttackCombo3();
            if(_monoBehaviour._clickcount == 0)
                _monoBehaviour.MeleeAttackComoEnd();
            
            if ((_monoBehaviour.CheckForUpKey() && _monoBehaviour.CurrentWaveGauge > 10 && !_powerAttack)
                || (_monoBehaviour.isPowerAttackEnd &&!_powerAttack))
            {
                animator.speed = 1.0f;
                _monoBehaviour._attack.DamageReset(_monoBehaviour.Data.powerAttackdamage);
                _monoBehaviour.enableAttack();
                _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * _monoBehaviour.GetFacing());
                _monoBehaviour.CurrentWaveGauge -= 10;
                _monoBehaviour.isPowerAttackEnd = false;
                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StopCharge();
                }
                _powerAttack = true;
            }
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_monoBehaviour._clickcount == 3)
                _monoBehaviour.MeleeAttackComoEnd();
            _monoBehaviour._attack.DefultDamage();
            _monoBehaviour.DisableAttack();
            _powerAttack = false;
            SoundManager.Instance.StopEffect();
        }
    }
}