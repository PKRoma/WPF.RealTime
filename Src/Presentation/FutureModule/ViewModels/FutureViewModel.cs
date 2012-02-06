using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FutureModule.Views;
using WPF.RealTime.Data;
using WPF.RealTime.Data.Binding;
using WPF.RealTime.Data.Entities;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.AttachedCommand;
using WPF.RealTime.Infrastructure.Collections;
using WPF.RealTime.Infrastructure.Commands;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;

namespace FutureModule.ViewModels
{
    [Export(typeof(IDynamicViewModel))]
    [View(typeof(View))]
    public class FutureViewModel : BaseViewModel, IDynamicViewModel
    {
        #region IDynamicView Members
        private string _dynamicViewName;
        public string DynamicViewName
        {
            get { return _dynamicViewName; }
            set { _dynamicViewName = value; OnPropertyChanged("DynamicViewName"); }
        }
        #endregion

        public ICommand CreateColumnsCommand { get; private set; }
        public NotifyCollection<Entity> Entities { get; private set; }
        public FutureViewModel() : base("Future Module", true, true)
        {
            DynamicViewName = "Future Module";
            //Mediator.GetInstance.RegisterInterest<Entity>(Topic.MockBondServiceDataReceived, DataReceived);
            //var grid = GetRef<DataGrid>("MainGrid");
            Entities = new NotifyCollection<Entity>(EntityBuilder.LoadMetadata(AssetType.Common, AssetType.Future));
            CreateColumnsCommand = new SimpleCommand<Object, EventToCommandArgs>((parameter) => true, CreateColumns);
        }


        [RegisterInterest(Topic.FutureModuleOpen, TaskType.Sporadic)]
        private void OpenClose(bool state)
        {
            ProcessOnDispatcherThread(() =>
            {
                if (state)
                    ((Window)ViewReference).Show();
                else
                    ((Window)ViewReference).Hide();
            });
        }

        [RegisterInterest(Topic.FutureServiceDataReceived, TaskType.Periodic)]
        private void DataReceived(IEnumerable<Entity> entities)
        {
            Action w = () => Entities.AddOrUpdate(entities, false);
            DispatcherFacade.AddToDispatcherQueue(w);
        }

        private static void CreateColumns(EventToCommandArgs args)
        {
            var ea = (DataGridAutoGeneratingColumnEventArgs)args.EventArgs;

            ea.Column = new WpfGridColumn
            {
                Header = ea.PropertyName,
                Width = ((ea.PropertyType == typeof(String)) || (ea.PropertyType == typeof(DateTime))) ?
                            DataGridLength.SizeToCells : DataGridLength.SizeToHeader
            };
        }
    }
}
