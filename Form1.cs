﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

//define log4net
using log4net;

//define Emgu.CV
using Emgu;
using Emgu.CV;


//define Smartray APiX
using SmartRay;
using SmartRay.Api;
using System.Net;



//define log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace APIX_Winform_Demo
{
    public partial class Form1 : Form
    {
        private readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        Sensor Sensor0 = new Sensor();




        public Form1()
        {
            InitializeComponent();
            log.Info("The UI initialed!");
            initSR_APIx();

        }



        private bool initSR_APIx()
        {
            //get the apix version
            ApiManager.Initialize();
            log.Info("The API version is:" + ApiManager.Version);
            //ApiManager.EnableConsoleLogging();
            ApiManager.OnMessage += new ApiManager.OnMessageDelegate(OnSensorMessgae);

            return true;
        }

        private void OnSensorMessgae(MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {
            if (aMsg == null)
                return;
            if (aMsgType == MessageType.Connection)
            {
                log.Info("sensor is connection");
            }
            else if (aMsgType == MessageType.Info)
            {
                log.Info(aMsg + ";" + "Message data:" + aMsgData);
            }
            else if (aMsgType == MessageType.Error)
            {
                log.Error(aMsg);
            }
            else if (aMsgType == MessageType.Data)
            {
                log.Info("Data message:" + aMsg + ";" + "Message data:" + aMsgData);
            }
            //throw new NotImplementedException();
        }

        public Task InitialSensor(Sensor sensor)
        {
            Task InitialSensorTask = new Task(() =>
            {
                
                IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse("192.168.178.200"), 40);
                TimeSpan timeSpan = new TimeSpan(500);
                sensor.Connect(ipSensorEndPoint, timeSpan);
                sensor.LoadCalibrationDataFromSensor();

                sensor.OnConnected += new Sensor.OnConnectedDelegate(OnSensorConnectEvent);
                sensor.OnDisconnected += new Sensor.OnDisconnectedDelegate(OnSensorDisconnectEvent);
                sensor.OnLiveImage += new Sensor.OnLiveImageDelegate(OnSensorLiveImageEvent);
                sensor.OnPilImageNative += new Sensor.OnPilImageNativeDelegate(OnSensorPILNativeEvent);
                sensor.OnPilImage += new Sensor.OnPilImageDelegate(OnSensorPILEvent);
                sensor.OnZilImage += new Sensor.OnZilImageDelegate(OnSensorZILEvent);
                sensor.OnPointCloudImage += new Sensor.OnPointCloudImageDelegate(OnSensorPointCloudEvent);
            });
            InitialSensorTask.Start();
            return InitialSensorTask;

        }

        private void OnSensorPointCloudEvent(Sensor aSensor, ImageDataType aImageDataType, uint aNumPoints, uint aNumProfiles, Point3F[] aPointCloudImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            throw new NotImplementedException();
        }

        private void OnSensorZILEvent(Sensor aSensor, ImageDataType aImageDataType, int aHeight, int aWidth, float aVerticalRes, float aHorizontalRes, ushort[] aZMapImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, float aOriginYMillimeters)
        {
            throw new NotImplementedException();
        }

        private void OnSensorPILEvent(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, ushort[] aProfileImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            throw new NotImplementedException();
        }

        private void OnSensorPILNativeEvent(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, IntPtr aProfileImageData, IntPtr aIntensityImageData, IntPtr aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            throw new NotImplementedException();
        }

        private void OnSensorLiveImageEvent(Sensor aSensor, int aOriginX, int aWidth, int aHeight, IntPtr aImageDataPtr)
        {
            //convert it to OpenCV image
            Mat mat = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 1, aImageDataPtr, aWidth);
            //Cross thread to display image
            new Task(new Action(() =>
            {
                cv_imageBox1.Invoke(new Action(() =>
                {
                    cv_imageBox1.Image = mat;
                }));
            })).Start();

        }

        private void OnSensorDisconnectEvent(Sensor aSensor)
        {
            log.Info("Sensor disconnected");
            //throw new NotImplementedException();
        }

        private void OnSensorConnectEvent(Sensor aSensor)
        {
            log.Info("Sensor Connected");

            throw new NotImplementedException();
        }

        private async void btn_InitialSensor_Click(object sender, EventArgs e)
        {
            await InitialSensor(Sensor0);
            Sensor0.SetImageAcquisitionType(ImageAcquisitionType.LiveImage);
            Sensor0.StartAcquisition();
            //IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse("192.168.178.200"), 40);
            //TimeSpan timeSpan = new TimeSpan(500);
            //Sensor0.Connect(ipSensorEndPoint, timeSpan);
            //Sensor0.LoadCalibrationDataFromSensor();

        }
    }
}
