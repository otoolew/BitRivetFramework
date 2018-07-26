using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class QuestionBehaviour : MonoBehaviour 
	{
        public QuestionController controller;
        public QuestionData currentQuestion;
        public AnswerButton answerButtonPrefab;
        public LayoutGroup layout;
        public Text questionText;

        private void Awake()
        {
            controller = FindObjectOfType<QuestionController>();
            currentQuestion = controller.currentQuestion;
            questionText.text = currentQuestion.questionText;
            AddButtons();
        }

        protected AnswerButton CreateButton(AnswerData item)
        {
            AnswerButton button = Instantiate(answerButtonPrefab);
            button.Setup(item);
            return button;
        }


        private void AddButtons()
        {
            for (int i = 0; i < currentQuestion.answerList.Count; i++)
            {
                AnswerData item = currentQuestion.answerList[i];
                AnswerButton newButton = CreateButton(item);
                newButton.transform.SetParent(layout.transform);
            }
        }

        public void CheckAnswer(AnswerData m_Answer)
        {
            if (m_Answer.isCorrect)
            {
                Debug.Log("Correct");
            }
            else
            {
                Debug.Log("Incorrect");
            }
            controller.NextQuestion();
            Destroy(gameObject);

        }

    }
}
