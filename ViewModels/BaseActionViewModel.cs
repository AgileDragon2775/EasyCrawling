using EasyCrawling.Enums;
using EasyCrawling.Helpers;
using EasyCrawling.Models;

namespace EasyCrawling.ViewModels
{
    public class BaseActionViewModel : Base.ViewModelBase
    {
        BaseAction baseAction;

        public BaseAction BaseAction
        {
            get => baseAction;
        }

        public BaseActionType ActionType
        {
            set
            {
                if (baseAction.ActionType != value)
                {
                    baseAction.ActionType = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string NameAction
        {
            get => EnumHelper.StringValueOf(baseAction.ActionType);
        }

        public BaseActionViewModel() { }

        public BaseActionViewModel(BaseAction inAction)
        {
            baseAction = inAction;
        }
    }
}
