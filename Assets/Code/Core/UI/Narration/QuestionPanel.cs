using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class QuestionPanel : GenericPanel
    {
        QuestionController questionController;
        //public AnswerSelectButton answerButtonPrefab;
        public UIPool answerButtonObjectPool;
        public LayoutGroup layout;
        protected QuestionList m_QuestionList;

        public List<GameObject> answerButtonGameObjects = new List<GameObject>();
        public Text questionText;
        /// <summary>
        /// Instantiate the buttons
        /// </summary>
        private void Start()
        {
            questionController = FindObjectOfType<QuestionController>();
            m_QuestionList = questionController.m_QuestionList;
            if (layout == null || m_QuestionList == null)
            {
                return;
            }
            LoadQuestion(questionController.currentQuestionIndex);
        }
        public void LoadQuestion(int questionIndex)
        {
            RemoveAnswerButtons();
            questionText.text = m_QuestionList[questionIndex].questionText;
            for (int i = 0; i < m_QuestionList[questionIndex].answerList.Count; i++)
            {
                GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
                answerButtonGameObjects.Add(answerButtonGameObject);
                AnswerSelectButton answerButton = answerButtonGameObject.GetComponent<AnswerSelectButton>();
                answerButton.Initialize(m_QuestionList[questionIndex].answerList[i]);
                answerButton.transform.SetParent(layout.transform);
                answerButton.transform.localScale = Vector3.one;
            }
        }
        public void LoadQuestion(QuestionData questionData)
        {
            RemoveAnswerButtons();
            questionText.text = questionData.questionText;
            for (int i = 0; i < questionData.answerList.Count; i++)
            {
                GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
                answerButtonGameObjects.Add(answerButtonGameObject);
                AnswerSelectButton answerButton = answerButtonGameObject.GetComponent<AnswerSelectButton>();
                answerButton.Initialize(questionData.answerList[i]);
                answerButton.transform.SetParent(layout.transform);
                answerButton.transform.localScale = Vector3.one;
            }
        }
        private void RemoveAnswerButtons()
        {
            while (answerButtonGameObjects.Count > 0)
            {
                answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
                answerButtonGameObjects.RemoveAt(0);
            }
        }
        /// <summary>
        /// Play camera animations
        /// </summary>
        public override void Show()
        {
            base.Show();
        }

        /// <summary>
        /// Return camera to normal position
        /// </summary>
        public override void Hide()
        {
            base.Hide();
        }
    }
}
