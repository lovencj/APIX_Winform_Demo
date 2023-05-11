using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//define log4net
using log4net;

//define Emgu.CV
using Emgu;
using Emgu.CV;


//define Smartray APiX
using SmartRay;
using SmartRay.Api;
using System.Net;


//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace APIX_Winform_Demo
{
    class APIXSensor
    {
        private readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Sensor sensor;

        public APIXSensor()
        {
            log.Info("Initial sensor class");
            sensor = new Sensor();
            sensor.OnConnected += new Sensor.OnConnectedDelegate(OnConnectedEvent);
            sensor.OnDisconnected += new Sensor.OnDisconnectedDelegate(OnDisconnectedEvent);
            sensor.OnMessage += new Sensor.OnMessageDelegate(OnSensorMessageEvent);
            sensor.OnLiveImage += new Sensor.OnLiveImageDelegate(OnLiveImageEvent);
            sensor.OnPilImage += new Sensor.OnPilImageDelegate(OnPILImageEvent);
            sensor.OnPilImageNative += new Sensor.OnPilImageNativeDelegate(OnPILNativeImageEvent);
            sensor.OnPointCloudImage += new Sensor.OnPointCloudImageDelegate(OnPintCloudEvent);
            sensor.OnZilImage += new Sensor.OnZilImageDelegate(OnZILImageEvent);
            sensor.OnZilImageNative += new Sensor.OnZilImageNativeDelegate(OnZILNativeImageEvent);

            //intial the sensor parameters
            this._IPAddress = "192.168.178.200";//default IPAddress
            this._portNumber = 40;//default port number;
            this._isSensorConnected = false;
            this._AcquisitionType = ImageAcquisitionType.ProfileIntensityLaserLineThickness;
            this._NumberOfProfileToCapture = 1000;
            this._PackSize = 500;
            this._PacketTimeout = new TimeSpan(0, 0, 0, 0, 500);




        }

        private void OnZILNativeImageEvent(Sensor aSensor, ImageDataType aImageDataType, int aHeight, int aWidth, float aVerticalRes, float aHorizontalRes, IntPtr aZMapImageData, IntPtr aIntensityImageData, IntPtr aLaserLineThicknessImageData, float aOriginYMillimeters)
        {
            //throw new NotImplementedException();
        }

        private void OnZILImageEvent(Sensor aSensor, ImageDataType aImageDataType, int aHeight, int aWidth, float aVerticalRes, float aHorizontalRes, ushort[] aZMapImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, float aOriginYMillimeters)
        {
            //throw new NotImplementedException();
        }

        private void OnPintCloudEvent(Sensor aSensor, ImageDataType aImageDataType, uint aNumPoints, uint aNumProfiles, Point3F[] aPointCloudImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            throw new NotImplementedException();
        }

        private void OnPILImageEvent(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, ushort[] aProfileImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            //throw new NotImplementedException();
        }

        private void OnPILNativeImageEvent(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, IntPtr aProfileImageData, IntPtr aIntensityImageData, IntPtr aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            //throw new NotImplementedException();
        }

        private void OnLiveImageEvent(Sensor aSensor, int aOriginX, int aWidth, int aHeight, IntPtr aImageDataPtr)
        {
            throw new NotImplementedException();
        }

        private void OnSensorMessageEvent(Sensor aSensor, MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {
            //throw new NotImplementedException();
        }

        private void OnDisconnectedEvent(Sensor aSensor)
        {
            this._isSensorConnected = false;

        }

        private void OnConnectedEvent(Sensor aSensor)
        {
            this._isSensorConnected = true;
        }

        #region sensor parameters propetys
        private string _IPAddress;

        public string mIPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }

        private UInt16 _portNumber;

        public UInt16 PortNumber
        {
            get { return _portNumber; }
            set { _portNumber = value; }
        }


        private long _ConnectionTimeout;

        public long ConnectionTimeout
        {
            get { return _ConnectionTimeout; }
            set { _ConnectionTimeout = value; }
        }

        private string _ConfigFilePath;

        public string ConfigFilePath
        {
            get { return _ConfigFilePath; }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.LoadParameterSetFromFile(value);
                    sensor.SendParameterSetToSensor(value);
                }
                _ConfigFilePath = value;
            }
        }


        private bool _isSensorConnected;

        public bool isSensorConnected
        {
            get { return _isSensorConnected; }
            set { _isSensorConnected = value; }
        }


        private ImageAcquisitionType _AcquisitionType;

        public ImageAcquisitionType AcquisitionType
        {
            get
            {
                if (_isSensorConnected)
                {
                    _AcquisitionType = sensor.GetImageAcquisitionType();
                    log.Info("Get Image Acquisition Type:" + _AcquisitionType.ToString());
                }
                else
                    log.Error("sensor not connected!");
                return _AcquisitionType;
            }
            set
            {
                _AcquisitionType = value;
                if (_isSensorConnected)
                {
                    sensor.SetImageAcquisitionType(value);
                    log.Info("Set Image Acquisition Type:" + value.ToString());
                }
                else
                {
                    log.Warn("Sensor not connected! It just set the property of this value" + value.ToString());
                }
            }
        }

        private UInt32 _NumberOfProfileToCapture;

        public UInt32 NumberOfProfileToCapture
        {
            get
            {
                if (_isSensorConnected)
                {
                    _NumberOfProfileToCapture = sensor.GetNumberOfProfilesToCapture();
                }
                return _NumberOfProfileToCapture;
            }
            set
            {
                _NumberOfProfileToCapture = value;
                if (_isSensorConnected)
                {
                    sensor.SetNumberOfProfilesToCapture(value);
                }
            }
        }


        private UInt32 _PackSize;

        public UInt32 PackSize
        {
            get
            {
                if (_isSensorConnected)
                {
                    _PackSize = sensor.GetPacketSize();
                }
                return _PackSize;
            }
            set
            {
                _PackSize = value;
                if (_isSensorConnected)
                {
                    sensor.SetPacketSize(value);
                }
            }
        }


        private TimeSpan _PacketTimeout;

        public TimeSpan PacketTimeout
        {
            get
            {
                if (_isSensorConnected)
                {
                    _PacketTimeout = sensor.GetPacketTimeOut();
                }
                return _PacketTimeout;
            }
            set
            {
                _PacketTimeout = value;
                if (_isSensorConnected)
                {
                    sensor.SetPacketTimeOut(value);
                }
            }
        }

        private Size2I _Granularity;

        public Size2I Granularity
        {
            get
            {
                if (_isSensorConnected)
                {
                    _Granularity = sensor.Granularity;
                }
                return _Granularity;
            }

        }




        private ROI _aROI;

        public ROI SensorROI
        {
            get
            {
                if (_isSensorConnected)
                {
                    sensor.GetROI(out _aROI.StartX, out _aROI.Width, out _aROI.StartY, out _aROI.Height);
                }
                else
                {
                    _aROI = new ROI();
                    log.Warn("sensor not connected,set the sensor ROI as default value");
                }
                return _aROI;
            }
            set
            {
                _aROI = value;
                if (_isSensorConnected)
                {
                    if (value.Width % Granularity.Width != 0 || value.Height % Granularity.Height != 0)
                    {
                        log.Warn("Sensor ROI set error! The ROI height and with should match the granularity size");
                    }
                    else
                    {
                        try
                        {
                            sensor.SetROI(value.StartX, value.Width, value.StartY, value.Height);

                        }
                        catch (Exception ce)
                        {
                            log.Error("Set the ROI failed, the error message as below:\n" + ce.Message);
                            throw;
                        }
                    }
                }
            }
        }





        #endregion
        #region functions
        public Task<bool> Connect()
        {
            Task<bool> connecctsensortask = new Task<bool>(() =>
            {
                try
                {
                    IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse(_IPAddress), _portNumber);
                    TimeSpan timeSpan = new TimeSpan(500);
                    sensor.Connect(ipSensorEndPoint, timeSpan);
                    sensor.LoadCalibrationDataFromSensor();
                    log.Info("Sensor connected!");
                    //sensor.Granularity

                    return true;
                }
                catch (Exception ce)
                {
                    log.Error("sensor can't be connected, the error message as below:\n" + ce.Message);
                    return false;
                }

            });

            connecctsensortask.Start();
            return connecctsensortask;

        }

        public Task<bool> StartAcquisition()
        {
            Task<bool> startacqTask = new Task<bool>(() =>
            {
                bool startAcqusition = false;
                try
                {
                    if (_isSensorConnected)
                    {
                        sensor.StartAcquisition();
                        startAcqusition = true;
                    }
                    else
                    {
                        startAcqusition = false;
                        log.Error("sensor not connected yet or the sensor disconnected,\n please check the sensor status");
                    }
                }
                catch (Exception ce)
                {
                    startAcqusition = false;
                    log.Error("Execute sensor start acquisition failed,\nPlease check the sensor status, the error message as below:\n" + ce.Message);
                }
                return startAcqusition;
            });
            startacqTask.Start();
            return startacqTask;
        }

        #endregion

    }

    public struct ROI
    {
        public int StartX;

        public int Width;

        public int StartY;

        public int Height;

        public ROI(int _StartX = 0, int _width = 1920, int _StartY = 0, int _Height = 100)
        {
            StartX = _StartX;
            StartY = _StartY;
            Width = _width;
            Height = _Height;
        }
    }
}
