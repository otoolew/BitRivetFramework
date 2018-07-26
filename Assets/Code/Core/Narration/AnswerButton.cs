using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core
{
    public class AnswerButton : MonoBehaviour
    {
        /// <summary>
        /// Reference to the required button component
        /// </summary>
        protected Button m_Button;

        /// <summary>
        /// The UI text element that displays the answer
        /// </summary>
        public Text m_AnswerDisplay;
        /// <summary>
        /// The data concerning the answer this button displays
        /// </summary>
        protected AnswerData m_Answer;
        /// <summary>
        /// The data concerning the answer this button displays
        /// </summary>
        public QuestionBehaviour m_Question;


        // Use this for initialization
        private void Start () 
		{
            m_Question = GetComponentInParent<QuestionBehaviour>();
            if (m_Button == null)
            {
                m_Button = GetComponent<Button>();
            }
            m_Button.onClick.AddListener(HandleClick);
        }


        internal void Setup(AnswerData answerData)
        {
            m_Answer = answerData;
            m_AnswerDisplay.text = answerData.answerText;
        }

        public void HandleClick()
        {
            m_Question.CheckAnswer(m_Answer);
            
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
