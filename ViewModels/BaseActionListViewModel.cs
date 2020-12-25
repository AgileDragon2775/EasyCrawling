using EasyCrawling.Enums;
using EasyCrawling.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyCrawling.ViewModels
{
    public class BaseActionListViewModel : Base.ViewModelBase
    {

        #region Fields

        private List<BaseAction> _actionList;
        private BaseActionType _type;
        private RelayCommand _addActionCommand;
        private RelayCommand _deleteActionCommand;
        private RelayCommand _settingActionCommand;

        #endregion

        #region event

        public event EventHandler SetAction;
        public event EventHandler ActionChanged;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public List<BaseAction> BaseList
        {
            get => Get(ref _actionList);
            set => Set(ref _actionList, value);
        }

        public ObservableCollection<BaseActionViewModel> ActionList
        {
            get => new ObservableCollection<BaseActionViewModel>(
                BaseList?
                .Select(m => new BaseActionViewModel(m))
                .ToList());
        }

        public BaseActionType Type
        {
            get => _type;
            set => Set(ref _type, value);
        }

        #endregion

        #region Command

        public RelayCommand AddActionCommand
        {
            get => _addActionCommand ??
                    (_addActionCommand = new RelayCommand(OnAddAction, CanAddAction));
            set => Set(ref _addActionCommand, value);
        }

        public RelayCommand DeleteActionCommand
        {
            get => _deleteActionCommand ??
                    (_deleteActionCommand = new RelayCommand(OnDeleteAction));
            set => Set(ref _deleteActionCommand, value);
        }

        public RelayCommand SettingActionCommand
        {
            get => _settingActionCommand ??
                    (_settingActionCommand = new RelayCommand(OnSettingAction, CanSettingAction));
            set => Set(ref _settingActionCommand, value);
        }


        #endregion

        #region method

        #endregion

        #region ExcuteCommand
        private void OnAddAction(object param)
        {
            BaseList?.Add(new MyToast 
            {
                ActionType = (BaseActionType)param
            });

            ActionChanged(param, null);
            RaisePropertyChanged("ActionList");
        }

        private void OnDeleteAction(object param)
        {
            (param as BaseActionViewModel).ActionType = BaseActionType.NONE;
            BaseList?.Remove((param as BaseActionViewModel).BaseAction);
            ActionChanged(param, null);
            RaisePropertyChanged("ActionList");
        }

        private void OnSettingAction(object param)
        {
            SetAction(param, null);
        }
        #endregion

        #region CanCommand

        private bool CanAddAction(object param)
        {
            return param != null && (BaseActionType)param == BaseActionType.NOTIFITY;
        }

        private bool CanSettingAction(object param)
        {
            return (SetAction != null);
        }

        #endregion
    }
}
