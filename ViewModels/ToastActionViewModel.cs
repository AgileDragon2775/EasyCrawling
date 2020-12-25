using EasyCrawling.Enums;
using EasyCrawling.Helpers;
using EasyCrawling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyCrawling.ViewModels
{
    public class ToastActionViewModel : NameViewModel
    {
        private readonly ToastAction _toastAction;
       
        public ToastActionViewModel()
        {
            _toastAction = new ToastAction();
            wordPointer = _toastAction;
        }

        public ToastActionViewModel(ToastAction inAction)
        {
            _toastAction = inAction;
            wordPointer = _toastAction;
        }

        public ToastAction ToastAction
        {
            get => _toastAction;
        }

        public WhenType When
        {
            get => _toastAction.When;
            set
            {
                if(_toastAction.When != value)
                {
                    _toastAction.When = value;
                    RaisePropertyChanged();
                }
            }
        }
        public BaseActionType ActionType
        {
            get => _toastAction.ActionType;
            set
            {
                if(_toastAction.ActionType != value)
                {
                    _toastAction.ActionType = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Folder
        {
            get => _toastAction.Folder;
            set
            {
                if(_toastAction.Folder != value)
                {
                    _toastAction.Folder = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string NameWhen
        {
            get => EnumHelper.StringValueOf(When);
        }

        public string NameAction
        {
            get => EnumHelper.StringValueOf(ActionType);
        }        
    }
}
