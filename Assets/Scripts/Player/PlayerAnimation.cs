using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.PlayerController.FSM;
using UnityEngine;


namespace AlsRitter.PlayerController
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(PlayerFSMSystem))]
    public class PlayerAnimation : MonoBehaviour, IEventObserver
    {
        private Animator anim;
        private PlayerFSMSystem pm;
        private Rigidbody2D rb;

        private int isWalkId;
        private int isCrouchingId;
        private int isRunId;
        private int verticalVelocityId;
        private int inTheAirId;
        private int inTheClimbId;
        private int isClimbingId;

        private int isDeathTriggerId;
        private int inTheAirTriggerId;
        private int isCrouchingTriggerId;
        private int inTheClimbTriggerId;

        // 临时存储
        private bool isWalk;
        private bool isClimbing;

        private void Awake()
        {
            EventManager.Register(this, EventID.IsCrouching, EventID.Run, EventID.InTheAir, EventID.GraspWall);
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            pm = GetComponent<PlayerFSMSystem>();
            rb = GetComponentInParent<Rigidbody2D>();

            isWalkId = Animator.StringToHash("isWalk");
            isClimbingId = Animator.StringToHash("isClimbing");
            isCrouchingId = Animator.StringToHash("isCrouching");
            isRunId = Animator.StringToHash("isRun");
            inTheAirId = Animator.StringToHash("inTheAir");
            inTheClimbId = Animator.StringToHash("inTheClimb");
            verticalVelocityId = Animator.StringToHash("verticalVelocity");

            isDeathTriggerId = Animator.StringToHash("IsDeathTrigger");
            inTheAirTriggerId = Animator.StringToHash("InTheAirTrigger");
            isCrouchingTriggerId = Animator.StringToHash("IsCrouchingTrigger");
            inTheClimbTriggerId = Animator.StringToHash("InTheClimbTrigger");
        }

        // Update is called once per frame
        private void Update()
        {

            anim.SetFloat(verticalVelocityId, rb.velocity.y);

            isWalk = Mathf.Abs(pm.xVelocity) > 0.01;
            isClimbing = Mathf.Abs(pm.yVelocity) > 0.01;

            anim.SetBool(isWalkId, isWalk);
            anim.SetBool(isClimbingId, isClimbing);
        }

        public void HandleEvent(EventData resp)
        {
            
            switch (resp.eid)
            {
                case EventID.IsCrouching:
                    anim.SetBool(isCrouchingId, ((PlayerStateEventData) resp).trigger);
                    if (((PlayerStateEventData) resp).trigger)
                    {
                        anim.SetTrigger(isCrouchingTriggerId);
                    }
                    break;
                case EventID.Run:
                    anim.SetBool(isRunId, ((PlayerStateEventData) resp).trigger);
                    break;
                case EventID.InTheAir:
                    anim.SetBool(inTheAirId, ((PlayerStateEventData) resp).trigger);
                    if (((PlayerStateEventData) resp).trigger)
                    {
                        anim.SetTrigger(inTheAirTriggerId);
                    }
                    break;
                case EventID.GraspWall:
                    anim.SetBool(inTheClimbId, ((PlayerStateEventData) resp).trigger);
                    if (((PlayerStateEventData) resp).trigger)
                    {
                        anim.SetTrigger(inTheClimbTriggerId);
                    }
                    break;
            }
        }
    }
}