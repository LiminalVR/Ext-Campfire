using Effects;
using Liminal.SDK.Extensions;
using UnityEngine;

namespace Liminal.Platform.Environment.Effects
{
    public class ReflectionSetup : MonoBehaviour
    {
        private StereoReflectionRenderer mReflectionRenderer;

        [SerializeField] private Camera m_RendererCamera = null;
        [SerializeField] private ReflectionCamera m_ReflectionCamera = null;

        #region MonoBehaviour

        private void OnEnable()
        {
            if (m_RendererCamera == null)
                m_RendererCamera = Camera.main;
            
            if (m_RendererCamera == null)
                return;

            // Hook up renderer to reflection camera
            mReflectionRenderer = m_RendererCamera.gameObject.GetOrAddComponent<StereoReflectionRenderer>();
            mReflectionRenderer.ReflectionCamera = m_ReflectionCamera.GetComponent<Camera>();
        }

        private void OnDisable()
        {
            if (mReflectionRenderer != null)
            {
                Destroy(mReflectionRenderer);
                mReflectionRenderer = null;
            }
        }

        #endregion
    }
}
