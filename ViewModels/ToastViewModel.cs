using EasyCrawling.Enums;
using EasyCrawling.Helpers;
using EasyCrawling.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace EasyCrawling.ViewModels
{
    public class ToastViewModel : Base.ViewModelBase
    {
        #region Fields

        private MyToast _nowToast;

        private VisualType _visualType;
        private WhenType _whenType;

        private RelayCommand _addVisualCommand;
        private RelayCommand _addEventCommand;
        private RelayCommand _deleteVisualCommand;
        private RelayCommand _deleteEventCommand;
        private RelayCommand _openFolderDialogCommand;
        private RelayCommand _testToastCommand;
        private RelayCommand _titleChangedCommand;
        #endregion

        #region Event

        public EventHandler TitleChanged;
        public EventHandler ToastChanged;
        public EventHandler TestToast;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public MyToast NowToast
        {
            get => Get(ref _nowToast);
            set
            {
                Set(ref _nowToast, value);
                RaisePropertyChanged("VisualList");
                RaisePropertyChanged("ActionList");
            }
        }

        public VisualType VisualType
        {
            get => Get(ref _visualType);
            set => Set(ref _visualType, value);
        }
        public WhenType WhenType
        {
            get => Get(ref _whenType);
            set => Set(ref _whenType, value);
        }

        public ObservableCollection<ToastVisualViewModel> VisualList
        {
            get => new ObservableCollection<ToastVisualViewModel>(
                 NowToast.VisualList
                 .Select(m => new ToastVisualViewModel(m))
                 .ToList());
        }

        public ObservableCollection<ToastActionViewModel> ActionList
        {
            get => new ObservableCollection<ToastActionViewModel>(
                NowToast.ActionList
                .Select(m => new ToastActionViewModel(m))
                .ToList());
        }

        #endregion

        #region Command
        public RelayCommand AddVisualCommand
        {
            get => _addVisualCommand ??
                    (_addVisualCommand = new RelayCommand(OnAddVisual, IsInitialized));
            set => Set(ref _addVisualCommand, value);
        }
        public RelayCommand DeleteVisualCommand
        {
            get => _deleteVisualCommand ??
                    (_deleteVisualCommand = new RelayCommand(OnDeleteVisual));
            set => Set(ref _deleteVisualCommand, value);
        }
        public RelayCommand AddEventCommand
        {
            get => _addEventCommand ??
                    (_addEventCommand = new RelayCommand(OnAddEvent, IsInitialized));
            set => Set(ref _addEventCommand, value);
        }
        public RelayCommand DeleteEventCommand
        {
            get => _deleteEventCommand ??
                    (_deleteEventCommand = new RelayCommand(OnDeleteEvent));
            set => Set(ref _deleteEventCommand, value);
        }

        public RelayCommand OpenFolderDialogCommand
        {
            get => _openFolderDialogCommand ??
                    (_openFolderDialogCommand = new RelayCommand(OnOpenFolderDialog, IsNeedFolder));
            set => Set(ref _openFolderDialogCommand, value);
        }

        public RelayCommand TestToastCommand
        {
            get => _testToastCommand ??
                    (_testToastCommand = new RelayCommand(OnTestToast, IsInitialized));
            set => Set(ref _testToastCommand, value);
        }


        public RelayCommand TitleChangedCommand
        {
            get => _titleChangedCommand ??
                    (_titleChangedCommand = new RelayCommand(OnTitleChanged, CanTitleChanged));
            set => Set(ref _titleChangedCommand, value);
        }
        #endregion

        #region method

        public void RefreshWords()
        {
            foreach (ToastVisualViewModel visual in VisualList)
            {
                TitleChanged(visual, null);
            }
            foreach (ToastActionViewModel action in ActionList)
            {
                TitleChanged(action, null);
            }
        }

        public void RefreshView()
        {
            RaisePropertyChanged("VisualList");
            RaisePropertyChanged("ActionList");
        }

        #endregion

        #region ExcuteCommand

        private void OnAddVisual(object param)
        {
            NowToast.VisualList.Add(new ToastVisual
            {
                Visual = (VisualType)param               
            });
            ToastChanged(param, null);
            RaisePropertyChanged("VisualList");
        }

        private void OnDeleteVisual(object param)
        {
            NowToast.VisualList.Remove((param as ToastVisualViewModel).ToastVisual);
            ToastChanged(param, null);
            RaisePropertyChanged("VisualList");
        }

        private void OnAddEvent(object param)
        {
            NowToast.ActionList.Add(new ToastAction
            {
                When = (WhenType)param,
                ActionType = BaseActionType.NONE
            });
            ToastChanged(param, null);
            RaisePropertyChanged("ActionList");
        }

        private void OnDeleteEvent(object param)
        {
            NowToast.ActionList.Remove((param as ToastActionViewModel).ToastAction);
            ToastChanged(param, null);
            RaisePropertyChanged("ActionList");
        }

        private void OnOpenFolderDialog(object param)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                (param as ToastActionViewModel).Folder = dialog.FileName;
            }
        }

        private void OnTestToast(object param)
        {
            TestToast(param, null);
        }

        private void OnTitleChanged(object param)
        {
            TitleChanged(param, null);
        }

        #endregion

        #region CanCommand

        private bool CanTitleChanged(object param)
        {
            return TitleChanged != null;
        }

        private bool IsNeedFolder(object param)
        {
            if (param == null || param as ToastActionViewModel == null)
                return false;

            switch((param as ToastActionViewModel).ActionType)
            {
                case BaseActionType.DOWNLOAD_FILE:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsInitialized(object param)
        {
            return NowToast.ActionType == BaseActionType.NOTIFITY;
        }

        #endregion
    }
}
