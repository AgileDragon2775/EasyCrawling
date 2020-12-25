using System.Collections.Generic;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class MyToast : BaseAction
    {
        readonly List<ToastVisual> _visualList = new List<ToastVisual>();
        readonly List<ToastAction> _actionList = new List<ToastAction>();

        public IList<ToastVisual> VisualList
        {
            get => _visualList;
        }
        public IList<ToastAction> ActionList
        {
            get => _actionList;
        }

    }
}
