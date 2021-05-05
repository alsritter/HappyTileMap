using System.Collections;
using System.Collections.Generic;
using EventFrame;
using EventFrame.CustomEvent;
using UnityEngine;

namespace PlayerController.FSM
{
    public class ClimbingState : PlayerBaseState,IEventObserver
    {
        public readonly InClimbState parentState;

        public override string name => "Climbing";

        private bool isOnGround = false;
        private bool isOnWallTap = false;

        public ClimbingState(InClimbState parentState)
        {
            this.parentState = parentState;
            // 注册自己
            EventManager.Register(this,EventID.OnGround, EventID.GraspWall);
        }


        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.yVelocity) < 0.01)
            {
                TransitionOtherState(parentState, parentState.hangWallState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            switch (isOnGround)
            {
                // 如果已经在地面了就无法再下降了
                case true when player.yVelocity < 0:
                    return;
                // 如果到顶端了无法向上移动
                case false when isOnWallTap && player.yVelocity > 0:
                    //player.rb.bodyType = RigidbodyType2D.Dynamic;
                    //player.rb.AddForce(new Vector2(0, player.jumpForce / player.jump2ForceDivisor / 2),ForceMode2D.Impulse);
                    player.yVelocity = 0;
                    break;
            }

            player.transform.position =
                new Vector2(player.transform.position.x,
                    player.transform.position.y + player.climbSpeed * player.yVelocity);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.OnWallTap:
                    isOnWallTap = ((PlayerStateEventData) resp).trigger;
                    break;
                case EventID.OnGround:
                    isOnGround = ((PlayerStateEventData) resp).trigger;
                    break;
            }
        }
    }
}