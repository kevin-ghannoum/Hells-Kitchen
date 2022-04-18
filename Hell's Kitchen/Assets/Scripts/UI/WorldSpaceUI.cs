using System;
using Common.Enums;
using UnityEngine;

namespace UI
{
    public class WorldSpaceUI : MonoBehaviour
    {
        [SerializeField]
        public Canvas canvas;
        
        [SerializeField]
        public RectTransform screenSpaceObject;
        
        public virtual void LateUpdate()
        {
            var camera = Camera.main;
            if (camera)
            {
                screenSpaceObject.position = camera.WorldToScreenPoint(transform.position);
            }
        }
    }
}
