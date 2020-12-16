using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;

namespace BLE_Test
{
    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;

        IList<IService> Services;
        IService Service;

        IReadOnlyList<ICharacteristic> Characteristics;
        ICharacteristic Characteristic;

        bool isConnected;


        public MainPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();

            //lv.ItemsSource = deviceList;

            isConnected = false;

            adapter.DeviceConnected += bleDeviceConnected;
            adapter.DeviceDisconnected += bleDeviceDisconnected;
            adapter.DeviceDiscovered += bleDeviceDiscovered;
            adapter.DeviceConnectionLost += bleDeviceDisconnected;
        }


        private async void bleDeviceDiscovered(object sender, DeviceEventArgs a)
        {
            if ("UART Service" == a.Device.Name)
            {
                device = a.Device;

                try
                {
                    await adapter.StopScanningForDevicesAsync();
                    while (adapter.IsScanning) { };
                    await adapter.ConnectToDeviceAsync(device);

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
                }
            }
        }

        private async void bleDeviceConnected(object sender, DeviceEventArgs e)
        {

            Services = (IList<IService>)await device.GetServicesAsync();

            //Thread.Sleep(500);

            Service = await device.GetServiceAsync(Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
            //Service = await device.GetServiceAsync(Services[2].Id);

            Characteristics = await Service.GetCharacteristicsAsync();
            //Thread.Sleep(50);
            Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
            //Thread.Sleep(50);
            if (!Characteristic.CanWrite)
            {
                await DisplayAlert("Notice", "Char NOT writable !", "OK");
            }


            isConnected = true;
            addBtnConnectText("Disconnect");
            setStatusText("Device Connected");
            setActIndicator(false);
        }

        private void bleDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            isConnected = false;
            //btnScan.Text = "connect";
            addBtnConnectText("Connect");
            setStatusText("Device Disconnected");
            setActIndicator(false);
        }

        private void addBtnConnectText(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                btnConnect.Text = text;
            });
        }

        private void setStatusText(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                StatusLabel.Text = text;
            });
        }

        private void setActIndicator(bool state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ConnectActivity.IsRunning = state;
            });
        }


        /// <summary>
        /// Define the status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatus_Clicked(object sender, EventArgs e)
        {
            var state = ble.State;

            DisplayAlert("Notice", state.ToString(), "OK !");
            //if (state == BluetoothState.Off)
            //{
            //    txtErrorBle.BackgroundColor = Color.Red;
            //    txtErrorBle.TextColor = Color.White;
            //    txtErrorBle.Text = "Your Bluetooth is off ! Turn it on !";
            //}
        }

        /// <summary>
        /// Scan the list of Devices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {

            try
            {   
                if ((!adapter.IsScanning) && ( false == isConnected))
                {
                    setActIndicator(true);
                    deviceList.Clear();
                    //adapter.ScanTimeout = 10000;

                    setStatusText("Connecting......");

                    adapter.ScanTimeoutElapsed += (s, a) =>
                    {
                        setActIndicator(false);
                        setStatusText("Connection time out");

                    };




                    await adapter.StartScanningForDevicesAsync();


                    
                    

                }
                //else if (adapter.IsScanning)
                //{
                //    await adapter.StopScanningForDevicesAsync();
                //}
                else if (true == isConnected)
                {
                    setActIndicator(true);
                    await adapter.DisconnectDeviceAsync(device);

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }

        }


        //private  void btnClearList_Clicked(object sender, EventArgs e)
        //{
        //    deviceList.Clear();
        //}

        /// <summary>
        /// Connect to a specific device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void btnConnect_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (device != null)
        //        {
        //            await adapter.ConnectToDeviceAsync(device);

        //        }
        //        else
        //        {
        //            await DisplayAlert("Notice", "No Device selected !", "OK");
        //        }
        //    }
        //    catch (DeviceConnectionException ex)
        //    {
        //        //Could not connect to the device
        //        await DisplayAlert("Notice", ex.Message.ToString(), "OK");
        //    }
        //}

        //private async void btnKnowConnect_Clicked(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        await adapter.ConnectToKnownDeviceAsync(new Guid("guid"));

        //    }
        //    catch (DeviceConnectionException ex)
        //    {
        //        //Could not connect to the device
        //        await DisplayAlert("Notice", ex.Message.ToString(), "OK");
        //    }
        //}



        /// <summary>
        /// Get list of services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void btnGetServices_Clicked(object sender, EventArgs e)
        //{
        //    //Thread.Sleep(500);

        //    Services = (IList<IService>)await device.GetServicesAsync();

        //    //Thread.Sleep(500);

        //    Service = await device.GetServiceAsync(Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
        //    //Service = await device.GetServiceAsync(Services[2].Id);
        //}






        /// <summary>
        /// Get Characteristics
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void btnGetcharacters_Clicked(object sender, EventArgs e)
        //{
        //    Characteristics = await Service.GetCharacteristicsAsync();
        //    //Thread.Sleep(50);
        //    Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
        //    //Thread.Sleep(50);
        //    if(!Characteristic.CanWrite)
        //    {
        //        await DisplayAlert("Notice", "Char NOT writable !", "OK");
        //    }

        //}


        private void btnGreenClicked(object sender, EventArgs e)
        {
            SendBleMsg("green");
        }

        private void btnRedClicked(object sender, EventArgs e)
        {
            SendBleMsg("red");
        }

        private void btnOffClicked(object sender, EventArgs e)
        {
            SendBleMsg("off");
        }


        private async void SendBleMsg(string msg)
        {
            if (isConnected)
            {
                byte[] array = Encoding.UTF8.GetBytes(msg);

                try
                {
                    await Characteristic.WriteAsync(array);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Notice", ex.Message.ToString(), "OK");
                }
            }
            else
            {
                await DisplayAlert("Notice", "Device NOT connected !", "OK");
            }
        }




        //IDescriptor descriptor;
        //IList<IDescriptor> descriptors;

        //private async void btnDescriptors_Clicked(object sender, EventArgs e)
        //{
        //    descriptors = (IList<IDescriptor>)await Characteristic.GetDescriptorsAsync();
        //    descriptor = await Characteristic.GetDescriptorAsync(Guid.Parse("guid"));

        //}

        //private async void btnDescRW_Clicked(object sender, EventArgs e)
        //{
        //    var bytes = await descriptor.ReadAsync();
        //    await descriptor.WriteAsync(bytes);
        //}

        //private async void btnGetRW_Clicked(object sender, EventArgs e)
        //{
        //    var bytes = await Characteristic.ReadAsync();
        //    await Characteristic.WriteAsync(bytes);
        //}

        //private async void btnUpdate_Clicked(object sender, EventArgs e)
        //{
        //    Characteristic.ValueUpdated += (o, args) =>
        //    {
        //        var bytes = args.Characteristic.Value;
        //    };
        //    await Characteristic.StartUpdatesAsync();
        //}

        ///// <summary>
        ///// Select Items
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (lv.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    device = lv.SelectedItem as IDevice;
        //}

        //private void txtErrorBle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{

        //}
    }
}
