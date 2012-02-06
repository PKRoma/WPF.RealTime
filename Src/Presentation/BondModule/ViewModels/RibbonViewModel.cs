using System.ComponentModel.Composition;
using System.Windows.Input;
using BondModule.Views;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.Commands;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;

namespace BondModule.ViewModels
{
    [Export(typeof(IStaticViewModel))]
    [View(typeof(RibbonView))]
    public class RibbonViewModel : BaseViewModel, IStaticViewModel
    {
        #region Private members only
        private bool _canGetData;
        #endregion

        #region IStaticView Members
        private string _staticViewName;
        public string StaticViewName
        {
            get { return _staticViewName; }
            set { _staticViewName = value; OnPropertyChanged("StaticViewName"); }
        }
        private string _openButtonContent;
        public string OpenButtonContent
        {
            get { return _openButtonContent; }
            set { _openButtonContent = value; OnPropertyChanged("OpenButtonContent"); }
        }
        #endregion

        public ICommand GetDataCommand { get; private set; }
        public ICommand OpenModuleCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }

        public RibbonViewModel() : base("BondModule Ribbon", true, false)
        {
            StaticViewName = "BondModule Ribbon";
            GetDataCommand = new SimpleCommand<object>(o => _canGetData, GetData);
            OpenModuleCommand = new SimpleCommand<bool>(OpenModule);
            PauseCommand = new SimpleCommand<object>(o => _canGetData, _ => Mediator.GetInstance.Broadcast(Topic.BondModuleHang));
            OpenButtonContent = "Open Bond Module";
        }

        private void OpenModule(bool state)
        {
            _canGetData = state;
            OpenButtonContent = state ? "Close Bond Module" : "Open Bond Module";
            Mediator.GetInstance.Broadcast(Topic.BondModuleOpen, state);
        }

        private static void GetData(object _)
        {
            Mediator.GetInstance.Broadcast(Topic.BondServiceGetData);
        }
    }
}
