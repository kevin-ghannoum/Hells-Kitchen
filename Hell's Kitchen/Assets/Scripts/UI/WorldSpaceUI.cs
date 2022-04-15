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
        public Transform worldSpaceObject;

        [SerializeField]
        public RectTransform screenSpaceObject;

        protected new Camera camera;

        public virtual void Reset()
        {
            canvas = GetComponent<Canvas>();
        }

        public virtual void Start()
        {
            camera = Camera.main;
            canvas.worldCamera = camera;
            screenSpaceObject.position = camera.WorldToScreenPoint(worldSpaceObject.position);
        }
        
        public virtual void Update()
        {
            screenSpaceObject.position = camera.WorldToScreenPoint(worldSpaceObject.position);
        }
    }
}
