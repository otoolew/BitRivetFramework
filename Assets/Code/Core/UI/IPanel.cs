using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public interface IPanel
    {
        /// <summary>
        /// Deactivates this page
        /// </summary>
        void Hide();

        /// <summary>
        /// Activates this page
        /// </summary>
        void Show();
    }
}
