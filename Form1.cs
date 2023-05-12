using System;
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
        private bool isStarted=false;


        //Sensor Sensor0 = new Sensor();


        APIXSensor Sensor1 = new APIXSensor();



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
            Sensor1.SensorImageEvent += Sensor1_SensorImageEvent;

            return true;
        }

        private void Sensor1_SensorImageEvent(object sensor, SRImageHandlerArgument SRimageHandlerArgument)
        {
            //Cross thread to display image
            new Task(new Action(() =>
            {
                cv_imageBox1.Invoke(new Action(() =>
                {
                    cv_imageBox1.Image = SRimageHandlerArgument.zilimage;
                }));
            })).Start();
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

        //public Task<bool> InitialSensor(Sensor sensor)
        //{
        //    Task<bool> InitialSensorTask = new Task<bool>(() =>
        //    {
        //        bool result = false;
        //        try
        //        {
        //            IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse("192.168.178.200"), 40);
        //            TimeSpan timeSpan = new TimeSpan(1500);
        //            sensor.Connect(ipSensorEndPoint, timeSpan);
        //            sensor.LoadCalibrationDataFromSensor();

        //            sensor.OnConnected += new Sensor.OnConnectedDelegate(OnSensorConnectEvent);
        //            sensor.OnDisconnected += new Sensor.OnDisconnectedDelegate(OnSensorDisconnectEvent);
        //            sensor.OnMessage += new Sensor.OnMessageDelegate(OnSensorMessageEvent);
        //            sensor.OnLiveImage += new Sensor.OnLiveImageDelegate(OnSensorLiveImageEvent);
        //            sensor.OnPilImage += new Sensor.OnPilImageDelegate(OnSensorPILImageEvent);
        //            result = true;
        //        }
        //        catch (Exception ce)
        //        {
        //            log.Error("Initial sensor faild:\n"+ce);
        //        }
        //        return result;

        //    });
        //    InitialSensorTask.Start();
        //    return InitialSensorTask;

        //}

        //private void OnSensorPILImageEvent(Sensor aSensor, SmartRay.Api.ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, ushort[] aProfileImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        //{
        //    throw new NotImplementedException();
        //}

        //private void Sensor_OnPilImage(Sensor aSensor, SmartRay.Api.ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, ushort[] aProfileImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        //{
        //    throw new NotImplementedException();
        //}

        //private void OnSensorMessageEvent(Sensor aSensor, MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        //{
        //    throw new NotImplementedException();
        //}


        //private void OnSensorLiveImageEvent(Sensor aSensor, int aOriginX, int aWidth, int aHeight, IntPtr aImageDataPtr)
        //{
        //    //convert it to OpenCV image
        //    Mat mat = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 1, aImageDataPtr, aWidth);
        //    //Cross thread to display image
        //    new Task(new Action(() =>
        //    {
        //        cv_imageBox1.Invoke(new Action(() =>
        //        {
        //            cv_imageBox1.Image = mat;
        //        }));
        //    })).Start();

        //}

        //private void OnSensorDisconnectEvent(Sensor aSensor)
        //{
        //    log.Info("Sensor disconnected");
        //    //throw new NotImplementedException();
        //}

        //private void OnSensorConnectEvent(Sensor aSensor)
        //{
        //    log.Info("Sensor Connected");

        //    throw new NotImplementedException();
        //}

        private async void btn_InitialSensor_Click(object sender, EventArgs e)
        {
            //var result=await InitialSensor(Sensor0);
            //if (result)
            //{
            //    Sensor0.SetImageAcquisitionType(ImageAcquisitionType.LiveImage);
            //    Sensor0.StartAcquisition();

            //}
            var result= await Sensor1.Connect();
            Sensor1.AcquisitionType = ImageAcquisitionType.ZMapIntensityLaserLineThickness;
            Sensor1.NumberOfProfileToCapture = 1000;
            Sensor1.PackSize = 1000;


            //IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse("192.168.178.200"), 40);
            //TimeSpan timeSpan = new TimeSpan(500);
            //Sensor0.Connect(ipSensorEndPoint, timeSpan);
            //Sensor0.LoadCalibrationDataFromSensor();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Sensor0.SaveParameterSet("MyParameters.json");
        }

        private void btn_StartAcquisition_Click(object sender, EventArgs e)
        {
            isStarted = !isStarted;
            if (isStarted)
            {
                log.Info("Number of profile to capture:" + Sensor1.NumberOfProfileToCapture);
                log.Info("Packsize:" + Sensor1.PackSize);
                log.Info("Image Type:" + Sensor1.AcquisitionType);
                Sensor1.SensorROI = new ROI(0, 4096, 660, 58);
                Sensor1.StartAcquisition();
            }
            else if (!isStarted)
            {
                log.Info("Sensor stop acquisition");
                Sensor1.StopAcquisition();
            }

        }
    }
}
