using System.Collections.Generic;

namespace EasyCrawling.Models
{   
    public class TreeData
    {
        readonly List<TreeData> _children = new List<TreeData>();
        public string Text { get; set; }
        public NodeTag Tag { get; set; }

        public IList<TreeData> Children
        {
            get { return _children; }
        }

        public TreeData()
        {
            Text = "";
            Tag = new NodeTag();
        }
    }
}
