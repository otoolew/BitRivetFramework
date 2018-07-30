using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core { 

    public class AnswerSelectButton : MonoBehaviour
    {
        /// <summary>
        /// Reference to the required button component
        /// </summary>
        protected Button m_Button;

        /// <summary>
        /// Reference to the required button component
        /// </summary>
        protected QuestionController questionController;

        /// <summary>
        /// The UI text element that displays the answer text
        /// </summary>
        public Text answerDisplay;

   

        /// <summary>
        /// The data concerning the level this button displays
        /// </summary>
        protected AnswerData m_AnswerData;

        /// <summary>
        /// When the user clicks the button, change the scene
        /// </summary>
        public void ButtonClicked()
        {
            questionController = FindObjectOfType<QuestionController>();
            questionController.NextQuestion(m_AnswerData.nextQuestion);
        }

        /// <summary>
        /// A method for assigning the data from item to the button
        /// </summary>
        /// <param name="answerData">
        /// The data with the information concerning the level
        /// </param>
        public void Initialize(AnswerData answerData)
        {
            LazyLoad();
            

            if (answerDisplay == null)
            {
                return;
            }
            m_AnswerData = answerData;
            answerDisplay.text = answerData.answerText;
               
        }

        /// <summary>
        /// Ensure <see cref="m_Button"/> is not null
        /// </summary>
        protected void LazyLoad()
        {
            if (m_Button == null)
            {
                m_Button = GetComponent<Button>();
            }
        }

        /// <summary>
        /// Remove all listeners on the button before destruction
        /// </summary>
        protected void OnDestroy()
        {
            if (m_Button != null)
            {
                m_Button.onClick.RemoveAllListeners();
            }
        }
    }
}
