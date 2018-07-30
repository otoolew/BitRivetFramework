using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class QuestionController : MonoBehaviour 
	{
        public QuestionData currentQuestion;
        public QuestionPanel questionPanel;
        public QuestionList m_QuestionList;
        public int currentQuestionIndex;
        // Use this for initialization
        private void Start () 
		{
            currentQuestionIndex = 0;
            currentQuestion = m_QuestionList[currentQuestionIndex];
        }

        public void NextQuestion()
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < m_QuestionList.Count)
            {
                currentQuestion = m_QuestionList[currentQuestionIndex];
                questionPanel.LoadQuestion(currentQuestionIndex);
            }
            else
            {
                EndQuestions();
            }


        }
        public void NextQuestion(QuestionData nextQuestion)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < m_QuestionList.Count)
            {
                currentQuestion = nextQuestion;
                questionPanel.LoadQuestion(nextQuestion);
            }
            else
            {
                EndQuestions();
            }

        }
        public void EndQuestions()
        {
            Debug.Log("End of Questions... Hide Dialogue or something...");
            questionPanel.Hide();
        }
    }
}
