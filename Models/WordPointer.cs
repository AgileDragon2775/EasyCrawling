using System;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class WordPointer
    {
        [NonSerialized]
        public Word Pointer;
        public string Name;

        public override string ToString()
        {
            return Example;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public string Example
        {
            get => (Pointer == null || Pointer.Tag == null) ? Name : Pointer.Encoded;
        }
    }
}
