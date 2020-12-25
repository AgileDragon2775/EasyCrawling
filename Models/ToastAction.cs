using EasyCrawling.Enums;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class ToastAction : BaseAction
    {
        public WhenType When { get; set; }
    }
}
