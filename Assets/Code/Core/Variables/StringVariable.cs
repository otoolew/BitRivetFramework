using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.Variables
{
    [CreateAssetMenu(fileName = "newString", menuName = "Variables/String")]
    public class StringVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public string Value;

        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetValue(StringVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(string val)
        {
            Value = val;
        }

        public void ApplyChange(StringVariable val)
        {
            Value = val.Value;
        }
    }
}
