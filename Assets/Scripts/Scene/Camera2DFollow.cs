using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        private Transform target;
		public float minimumHeight=0;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        public bool bound;
        public Vector3 minPos;//pega a posição minima que a camera pode chegar
        public Vector3 maxPos;//pega a posição maxima que a camera pode chegar


        // Use this for initialization
        private void Start() {
			target = Global.target.transform;
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }

		public void Change(){
			target = Global.target.transform;
		}
        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
			if (newPos.y <= minimumHeight)
				newPos.y = transform.position.y;
            transform.position = newPos;

            if(bound){
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
                    Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
                    transform.position.z);
            }

            m_LastTargetPosition = target.position;
        }
    }
}