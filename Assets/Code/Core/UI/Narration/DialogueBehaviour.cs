using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class DialogueBehaviour : MonoBehaviour 
	{
        public float playerTalkRange;
        public NPCController controller;
        public QuestionData currentQuestion;
        public QuestionPanel questionPanel;
        public QuestionList m_QuestionList;

        public int currentQuestionIndex;
        public void Awake()
        {
            controller = GetComponent<NPCController>();
        }
        public void OnEnable()
        {

        }
        private void OnMouseOver()
        {
            playerTalkRange = controller.DistanceToPlayer;
            if (Input.GetMouseButtonUp(0))
            {
                questionPanel.LoadQuestion(currentQuestion);
                questionPanel.enabled = true;
            }
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
