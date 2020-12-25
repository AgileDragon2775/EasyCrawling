using System.Collections.Generic;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class Word
    {        
        public uint Numbering { get; set; }
        public string Name { get; set; }
        public NodeTag Tag { get; set; }
        public bool IsIndex { get; set; }
        [System.NonSerialized]
        public string Original;
        [System.NonSerialized]
        public string Encoded;
        readonly List<WordOption> _options = new List<WordOption>();
        readonly List<ExcpetWord> _excepts = new List<ExcpetWord>();

        public Word() 
        {
            Original = "";
            Encoded = "";

        }
        public Word(Word word) : this()
        {
            Numbering = word.Numbering;
            Name = word.Name;
            Tag = new NodeTag(word.Tag);
            foreach (var option in word.Options)
            {
                Options.Add(new WordOption(option));
            }
            foreach (var except in word.Excepts)
            {
                Excepts.Add(new ExcpetWord(except));
            }
        }

        public IList<WordOption> Options
        {
            get => _options;
        }

        public IList<ExcpetWord> Excepts
        {
            get => _excepts;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            var item = obj as Word;
            if (item == null)
            {
                return false;
            }

            if (ReferenceEquals(this, item))
            {
                return true;
            }

            if (Name != item.Name ||
                Numbering != item.Numbering ||
                !Tag.Equals(item.Tag))
            {
                return false;
            }

            for (int i = 0; i < Options.Count; i++)
            {
                if (!Options[i].Equals(item.Options[i]))
                {
                    return false;
                }
            }

            for (int i = 0; i < Excepts.Count; i++)
            {
                if (!Excepts[i].Equals(item.Excepts[i]))
                {
                    return false;
                }
            }

            return true;
        }       

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }   
}
