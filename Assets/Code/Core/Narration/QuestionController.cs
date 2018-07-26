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
        public Canvas questionPrefab;
        public QuestionBehaviour loadedQuestion;
        public int currentQuestionIndex;
        public QuestionData[] questions;
        // Use this for initialization
        private void Start () 
		{
            currentQuestionIndex = 0;
            currentQuestion = questions[currentQuestionIndex];
            GenerateQuestion(questions[currentQuestionIndex]);
        }
        public void GenerateQuestion(QuestionData questionConfig)
        {
            Canvas question = Instantiate(questionPrefab);
            loadedQuestion = question.GetComponent<QuestionBehaviour>();
        }

        public void NextQuestion()
        {
            loadedQuestion = null;
            
            if (currentQuestionIndex > questions.Length)
            {
                EndQuestions();
                return;
            }
            currentQuestion = questions[currentQuestionIndex];
            GenerateQuestion(questions[currentQuestionIndex]);
        }

        public void EndQuestions()
        {
            Debug.Log("End of Questions");
        }
    }
}
