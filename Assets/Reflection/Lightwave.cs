using Liminal.Core.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Liminal.Platform.Environment.Effects
{
    public class Lightwave : MonoBehaviour
    {
        private class ShaderProperties : ShaderPropertySet
        {
#pragma warning disable 0649
            public int _LiminalLightwaveStrength;
            public int _LiminalLightwaveDistance;
#pragma warning restore 0649
        }

        private static readonly ShaderProperties _shaderProperties = new ShaderProperties();

        private Animator mAnimator;
        private bool mExpanding;

        [SerializeField] private float m_Strength = 1f;
        [SerializeField] private float m_Distance = 80f;

        #region MonoBehaviour

        private void Awake()
        {
            _shaderProperties.AssignPropertyIds();
            UpdateGlobalShaderProperties();

            mAnimator = GetComponent<Animator>();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            
            _shaderProperties.AssignPropertyIds();
            m_Distance = Mathf.Max(m_Distance, 0);
            m_Strength = Mathf.Max(m_Strength, 0);
            UpdateGlobalShaderProperties();
        }

        private void Update()
        {
            if (mExpanding)
            {
                UpdateGlobalShaderProperties();
            }
        }

        #endregion

        public void Expand()
        {
            mExpanding = true;
            mAnimator.SetTrigger("Expand");
        }

        public IEnumerator WaitUntilExpandCompleted()
        {
            while (mExpanding)
                yield return null;
        }

        #region Animation Events

        public void ExpandComplete()
        {
            mExpanding = false;
        }

        #endregion

        private void UpdateGlobalShaderProperties()
        {
            Shader.SetGlobalFloat(_shaderProperties._LiminalLightwaveStrength, m_Strength);
            Shader.SetGlobalFloat(_shaderProperties._LiminalLightwaveDistance, m_Distance);
        }
    }
}
