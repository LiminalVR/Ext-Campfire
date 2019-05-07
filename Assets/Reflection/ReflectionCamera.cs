using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(Camera))]
    public class ReflectionCamera : MonoBehaviour
    {
        [SerializeField] private Camera mReflectionCamera;
        [SerializeField] private float m_ClipPlaneOffset = 0;
        public Camera Eye;

        #region MonoBehaviour
        private void Awake()
        {
            if(mReflectionCamera == null)
                mReflectionCamera = GetComponent<Camera>();
        }

        private void OnPreRender()
        {
            Prepare();
            GL.invertCulling = true;
        }

        private void OnPostRender()
        {
            GL.invertCulling = false;
        }

        #endregion

        private void Prepare()
        {
            // Find out the reflection plane: position and normal in world space
            var pos = gameObject.transform.position;

            // Reflection plane normal in the direction of Y axis
            var normal = Vector3.up;

            var d = -Vector3.Dot(normal, pos) - m_ClipPlaneOffset;
            var refPlane = new Vector4(normal.x, normal.y, normal.z, d);

            // Compute reflection matrix
            var refMatrix = Matrix4x4.zero;
            ComputeReflectionMatrix(ref refMatrix, refPlane);

            // Setup camera matrices
            var camMain = Eye;
            mReflectionCamera.worldToCameraMatrix = camMain.worldToCameraMatrix * refMatrix;
            mReflectionCamera.projectionMatrix = camMain.projectionMatrix;
        }

        private static void ComputeReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1.0f - 2 * plane[0] * plane[0]);
            reflectionMat.m01 = (-2 * plane[0] * plane[1]);
            reflectionMat.m02 = (-2 * plane[0] * plane[2]);
            reflectionMat.m03 = (-2 * plane[3] * plane[0]);
            reflectionMat.m10 = (-2 * plane[1] * plane[0]);
            reflectionMat.m11 = (1.0f - 2 * plane[1] * plane[1]);
            reflectionMat.m12 = (-2 * plane[1] * plane[2]);
            reflectionMat.m13 = (-2 * plane[3] * plane[1]);
            reflectionMat.m20 = (-2 * plane[2] * plane[0]);
            reflectionMat.m21 = (-2 * plane[2] * plane[1]);
            reflectionMat.m22 = (1.0f - 2 * plane[2] * plane[2]);
            reflectionMat.m23 = (-2 * plane[3] * plane[2]);
            reflectionMat.m30 = 0.0f;
            reflectionMat.m31 = 0.0f;
            reflectionMat.m32 = 0.0f;
            reflectionMat.m33 = 1.0f;
        }
    }
}
