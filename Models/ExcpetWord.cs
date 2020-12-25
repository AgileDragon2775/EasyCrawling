namespace EasyCrawling.Models
{
    [System.Serializable]
    public class ExcpetWord
    {
        public string Except { get; set; }

        public ExcpetWord()
        {
            Except = "";
        }
        public ExcpetWord(ExcpetWord except)
        {
            Except = except.Except;
        }
    }
}
