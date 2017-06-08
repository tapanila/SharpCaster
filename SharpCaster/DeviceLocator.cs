using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SharpCaster.Annotations;
using SharpCaster.Models;
using Tmds.MDns;

namespace SharpCaster
{
    public class DeviceLocator : INotifyPropertyChanged
    {
        public ObservableCollection<Chromecast> DiscoveredDevices { get; set; }
        private ServiceBrowser _serviceBrowser;

        public DeviceLocator()
        {
            DiscoveredDevices = new ObservableCollection<Chromecast>();
            _serviceBrowser = new ServiceBrowser();
            _serviceBrowser.ServiceAdded += onServiceAdded;
        }

        private void onServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            var name = e.Announcement.Hostname;
            var ip = e.Announcement.Addresses[0];
            Uri myUri;
            Uri.TryCreate("https://" + ip, UriKind.Absolute, out myUri);
            var chromecast = new Chromecast
            {
                DeviceUri = myUri,
                FriendlyName = name
            };
            DiscoveredDevices.Add(chromecast);
        }

        public async Task<ObservableCollection<Chromecast>> LocateDevicesAsync()
        {
            return await LocateDevices();
        }

        [System.Obsolete("This overload of LocateDevicesAsync is deprecated, please use other overload instead.")]
        public async Task<ObservableCollection<Chromecast>> LocateDevicesAsync(string localIpAdress)
        {
            return await LocateDevices();
        }

        private async Task<ObservableCollection<Chromecast>> LocateDevices()
        {
            if (_serviceBrowser.IsBrowsing)
            {
                _serviceBrowser.StopBrowse();
            }
            _serviceBrowser.StartBrowse("_googlecast._tcp");
            var timeOut = 0;
            while (DiscoveredDevices.Count < 1 && timeOut < 5000)
            {
                await Task.Delay(200);
                timeOut += 200;
            }
            _serviceBrowser.StopBrowse();
            return DiscoveredDevices;
        }




        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}