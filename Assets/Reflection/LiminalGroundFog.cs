using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Liminal.Platform.Environment
{
    public class LiminalGroundFog : MonoBehaviour
    {
        private Color mCurrentColor;
        
        [SerializeField] private Color m_DefaultColor = Color.black;
        [SerializeField] private float m_Distance = 0.6f;

        #region Properties

        public Color DefaultColor
        {
            get { return m_DefaultColor; }
        }
        
        public Color Color
        {
            get { return mCurrentColor; }
            set { SetColor(value); }
        }

        public float Distance
        {
            get { return m_Distance; }
            set { SetDistance(value); }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            SetColor(m_DefaultColor);
            SetDistance(m_Distance);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            SetColor(m_DefaultColor);
            SetDistance(m_Distance);
        }

        #endregion

        private void SetColor(Color color)
        {
            mCurrentColor = color;
            Shader.SetGlobalColor("_LiminalGroundFogColor", mCurrentColor);
        }

        private void SetDistance(float value)
        {
            Shader.SetGlobalFloat("_LiminalGroundFogDistance", m_Distance);
        }
        
        public IEnumerator FadeColor(float duration, Color color, AnimationCurve curve = null)
        {
            var startTime = Time.time;
            var startColor = Shader.GetGlobalColor("_LiminalGroundFogColor");
            while (true)
            {
                var t = (duration > 0) ? Mathf.Clamp01((Time.time - startTime) / duration) : 1f;
                var f = (curve != null) ? curve.Evaluate(t) : t;
                SetColor(Color.LerpUnclamped(startColor, color, f));

                if (t >= 1f)
                    break;

                yield return null;
            }
        }
    }
}
