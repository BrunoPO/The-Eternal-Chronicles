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
		private Vector3 PosiIni;

        // Use this for initialization
        private void Start() {
			PosiIni = transform.position;
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
			Vector3 localScale = GameObject.Find("Verde").transform.localScale;
			localScale.x = target.GetComponent<CharController>().life / target.GetComponent<CharController>().lifeIni;
			if (localScale.x < 0)
				localScale.x = 0;
			
			GameObject.Find("Verde").transform.localScale = localScale;

			if (target.transform.position.x > 35 && target.transform.position.x < 46)
				transform.gameObject.GetComponent<Camera2DFollow> ().minimumHeight = 2.3f;
			else if (target.transform.position.x > 48 && target.transform.position.x < 64)
				transform.gameObject.GetComponent<Camera2DFollow> ().minimumHeight = 3.7f;
			else
				transform.gameObject.GetComponent<Camera2DFollow> ().minimumHeight = 12.3f;


			if (Global.killSelf) {
				transform.position = PosiIni;
			}
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