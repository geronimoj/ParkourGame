using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Camera_Test
    {
        [UnityTest]
        public IEnumerator Camera_Move_Without_Transform_Assigned()
        {
            GameObject camera = CreateCamera();

            Transform position = camera.transform;
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            Debug.Assert(camera.transform == position);
        }

        [UnityTest]
        public IEnumerator Camera_Move_With_Transform()
        {
            GameObject camera = CreateCamera();

            GameObject target = new GameObject();
            target.transform.position = new Vector3(10, 5, 0);

            camera.GetComponent<CameraFollower>().target = target;

            yield return null;

            Debug.Assert(camera.transform.position == target.transform.position);
        }

        [UnityTest]
        public IEnumerator Camera_Move_With_Position_Offset()
        {
            GameObject camera = CreateCamera();
            Vector3 initPos = camera.transform.position;
            CameraFollower cf = camera.GetComponent<CameraFollower>();
            cf.positionOffset = new Vector3(10, 5, -7);

            yield return null;

            Debug.Assert(camera.transform.position == initPos);

            GameObject target = new GameObject();
            target.transform.position = new Vector3(-5, 6, 0);
            cf.target = target;

            yield return null;

            Debug.Assert(camera.transform.position == target.transform.position + cf.positionOffset);
            //Repeat the check to ensure the position is consistent among frames
            yield return null;
            Debug.Assert(camera.transform.position == target.transform.position + cf.positionOffset);
        }

        [UnityTest]
        public IEnumerator Camera_Move_With_Rotation_Offset()
        {
            GameObject camera = CreateCamera();
            CameraFollower cf = camera.GetComponent<CameraFollower>();

            Debug.Assert(camera.transform.eulerAngles == Vector3.zero);

            cf.rotationOffset = new Vector3(90, 180, 56);
            yield return null;

            Debug.Assert(camera.transform.eulerAngles == Vector3.zero);

            GameObject target = new GameObject();
            target.transform.eulerAngles = new Vector3(20, 5, 100);

            cf.target = target;
            yield return null;
            Debug.Assert(Mathf.Abs(Quaternion.Angle(camera.transform.rotation, Quaternion.Euler(110,185,156))) < 1e-3f);
        }

        private GameObject CreateCamera()
        {
            GameObject g = new GameObject();
            g.AddComponent<CameraFollower>();

            return g;
        }
    }
}
