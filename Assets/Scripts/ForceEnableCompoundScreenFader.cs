using Liminal.Core.Fader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEnableCompoundScreenFader : MonoBehaviour
{
    public CompoundScreenFader CompoundScreenFader;

    // Start is called before the first frame update
    void Start()
    {
        CompoundScreenFader.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
