using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class GenericPanel : MonoBehaviour, IPanel
    {
        /// <summary>
        /// Canvas to disable. If this object is set, then the canvas is disabled instead of the game object 
        /// </summary>
        public Canvas canvas;

        /// <summary>
        /// Deactivates this page
        /// </summary>
        public virtual void Hide()
        {
            if (canvas != null)
            {
                canvas.enabled = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Activates this page
        /// </summary>
        public virtual void Show()
        {
            if (canvas != null)
            {
                canvas.enabled = true;
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
