using System.Collections;
using System.Collections.Generic;
using PlayerController.FSM;
using UnityEngine;


namespace PlayerController
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(PlayerFSMSystem))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private PlayerFSMSystem pm;
        private Rigidbody2D rb;

        private int isWalkId;
        private int isCrouchingId;
        private int isRunId;
        private int isDeathId;
        private int verticalVelocityId;
        private int inTheAirId;
        private int inTheClimbId;
        private int isClimbingId;

        // 临时存储
        private bool isWalk;
        private bool isClimbing;


        private void Start()
        {
            anim = GetComponent<Animator>();
            pm = GetComponent<PlayerFSMSystem>();
            rb = GetComponentInParent<Rigidbody2D>();

            isWalkId = Animator.StringToHash("isWalk");
            isClimbingId = Animator.StringToHash("isClimbing");
            isCrouchingId = Animator.StringToHash("isCrouching");
            isRunId = Animator.StringToHash("isRun");
            isDeathId = Animator.StringToHash("isDeath");
            inTheAirId = Animator.StringToHash("inTheAir");
            inTheClimbId = Animator.StringToHash("inTheClimb");
            verticalVelocityId = Animator.StringToHash("verticalVelocity");
        }

        // Update is called once per frame
        private void Update()
        {
            // 只有状态改变了才调用 SetBool 方法（因为每次调用 SetBool 都会通知 anyState）
            if (anim.GetBool(isCrouchingId) != pm.isCrouching) anim.SetBool(isCrouchingId, pm.isCrouching);
            if (anim.GetBool(isRunId) != pm.isRun) anim.SetBool(isRunId, pm.isRun);
            if (anim.GetBool(inTheAirId) != pm.inTheAir) anim.SetBool(inTheAirId, pm.inTheAir);
            if (anim.GetBool(inTheClimbId) != pm.graspWall) anim.SetBool(inTheClimbId, pm.graspWall);

            anim.SetFloat(verticalVelocityId, rb.velocity.y);

            isWalk = Mathf.Abs(pm.xVelocity) > 0.01;
            isClimbing = Mathf.Abs(pm.yVelocity) > 0.01;

            if (anim.GetBool(isWalkId) != isWalk) anim.SetBool(isWalkId, isWalk);
            if (anim.GetBool(isClimbingId) != isClimbing) anim.SetBool(isClimbingId, isClimbing);
        }
    }
}