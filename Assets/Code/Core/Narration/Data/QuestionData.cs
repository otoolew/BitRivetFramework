using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [CreateAssetMenu(menuName = "Dialogue/Question")]
    public class QuestionData : ScriptableObject
    {
        public string questionText;
        public List<AnswerData> answerList;
    }
}
