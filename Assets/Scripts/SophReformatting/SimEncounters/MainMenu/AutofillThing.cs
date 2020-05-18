using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class AutofillThing : MonoBehaviour
    {

        protected string[] TagOptions { get; set; }
        protected virtual void Awake()
        {
            TagOptions = GetTags();
            foreach (var tag in TagOptions)
                AddTag(tag);
        }


        protected virtual string[] GetTags()
        {
            var filePath = GetTagsFilePath();
            return File.ReadAllLines(filePath);
        }
        protected string GetTagsFilePath()
        {
            return null;
        }

        protected void AddTag(string tag)
        {

        }
    }
}
