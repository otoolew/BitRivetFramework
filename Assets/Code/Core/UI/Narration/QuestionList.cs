using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    /// <summary>
    /// Scriptable object for Level configuration
    /// </summary>
    [CreateAssetMenu(fileName = "newQuestionList", menuName = "Dialogue/Create Question List", order = 1)]
    public class QuestionList : ScriptableObject, IList<QuestionData>
    {
        public QuestionData[] questions;
        /// <summary>
        /// Cached dictionary of levels by their IDs
        /// </summary>
        IDictionary<string, QuestionData> m_QuestionDictionary;
        public QuestionData this[int index]
        {
            get { return questions[index]; }
            set { throw new NotSupportedException("Question List is read only"); }
        }

        /// <summary>
        /// Gets the number of levels
        /// </summary>
        public int Count
        {
            get { return questions.Length; }
        }

        /// <summary>
        /// Question list is always read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Add(QuestionData item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(QuestionData item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(QuestionData[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<QuestionData> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(QuestionData item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, QuestionData item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(QuestionData item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
