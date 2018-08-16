using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class DialogueResponseData
    {
        public string reponseText;
        public bool isCorrect;
        public QuestionData nextQuestion;
    }
}
