using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Effects
{
    public class StereoReflectionRenderer : MonoBehaviour
    {
        [SerializeField] private Camera m_ReflectionCamera = null;

        #region Properties

        public Camera ReflectionCamera
        {
            get { return m_ReflectionCamera; }
            set { m_ReflectionCamera = value; }
        }

        #endregion

        #region MonoBehaviour

        private void OnPreRender()
        {
            if (m_ReflectionCamera != null && m_ReflectionCamera.gameObject.activeInHierarchy)
                m_ReflectionCamera.Render();
        }

        #endregion
    }
}
