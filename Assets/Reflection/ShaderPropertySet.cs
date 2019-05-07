using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Liminal.Core.Rendering
{
    public abstract class ShaderPropertySet
    {
        private bool mPropertyIdsAssigned;

        /// <summary>
        /// Assigns shader property id values to all public integer fields on the set.
        /// </summary>
        public void AssignPropertyIds()
        {
            if (mPropertyIdsAssigned)
                return;

            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fInfo in fields)
            {
                if (fInfo.FieldType == typeof(int))
                    fInfo.SetValue(this, Shader.PropertyToID(fInfo.Name));
            }

            mPropertyIdsAssigned = true;
        }
    }
}
