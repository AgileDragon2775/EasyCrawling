using EasyCrawling.Enums;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class BaseAction : WordPointer
    {       
        public BaseActionType ActionType { get; set; }
        public string Folder { get; set; }

        public BaseAction() { }
        public BaseAction(BaseAction action)
        {
            ActionType = action.ActionType;
            Folder = action.Folder;
        }
    }
}
