namespace EasyCrawling.Models
{
    [System.Serializable]
    public class NodeTag
    {
        public string xPath { get; set; }          
        public int attrCnt { get; set; }        
        public int childCnt { get; set; }        
        public string cmpAttr { get; set; }
        public string currentXPath { get; set; }
        public NodeTag(NodeTag tag)
        {
            xPath = tag.xPath;
            currentXPath = tag.currentXPath;        
            attrCnt = tag.attrCnt;
            childCnt = tag.childCnt;
            cmpAttr = tag.cmpAttr;          
        }

        public NodeTag(
            string xPath = "", 
            int attrCnt=0,
            int childCnt=0,
            string cmpAttr = "",
            string currentXPath = "")
        {
            this.xPath = xPath;         
            this.attrCnt = attrCnt;
            this.childCnt = childCnt;
            this.cmpAttr = cmpAttr;        
            this.currentXPath = currentXPath;
        }

        public override bool Equals(object obj)
        {
            return (obj is NodeTag) &&
                   ((NodeTag)obj).xPath == this.xPath &&
                   ((NodeTag)obj).attrCnt == this.attrCnt &&
                   ((NodeTag)obj).childCnt == this.childCnt &&
                   ((NodeTag)obj).cmpAttr == this.cmpAttr;                  
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
  
        public static bool operator ==(NodeTag a, NodeTag b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            else if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            else
                return
                    a.Equals(b);
        }

        public static bool operator !=(NodeTag a, NodeTag b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return false;
            else if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return true;
            else
                return
                    !a.Equals(b);
        }

        public override string ToString()
        {
            return xPath;
        }
    }   
}
