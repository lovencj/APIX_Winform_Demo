using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//define log4net
using log4net;

//define Emgu.CV
using Emgu;
using Emgu.CV;


//define Smartray APiX
using SmartRay;
using SmartRay.Api;
using System.Net;
using System.Threading;
using System.Drawing;


//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace APIX_Winform_Demo
{
    class APIXSensor : IDisposable
    {
        private readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Sensor sensor;

        private uint ProfileCounter = 0;

        private Mat profileimage = new Mat();
        private Mat intensityImage = new Mat();
        private Mat laserlinethicknessImage = new Mat();
        private HiPerfTimer timer = new HiPerfTimer();
        //test for pointcloud data management list 
        private List<Point3F[]> point3Fs = new List<Point3F[]>();

        //meta data managment
        private List<MetaDataCollection> metaDataCollectionList = new List<MetaDataCollection>();



        /// <summary>
        /// 2. define event delegate
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="imageHandlerArgument"></param>

        public delegate void SensorImageDelegateHandler(object sensor, SRImageHandlerArgument SRimageHandlerArgument);

        /// <summary>
        /// 3. define event
        /// </summary>
        public event SensorImageDelegateHandler SensorImageEvent;

        public APIXSensor()
        {
            log.Info("Initial sensor class");
            sensor = new Sensor();
            sensor.OnConnected += new Sensor.OnConnectedDelegate(OnConnectedEvent);
            sensor.OnDisconnected += new Sensor.OnDisconnectedDelegate(OnDisconnectedEvent);
            sensor.OnMessage += new Sensor.OnMessageDelegate(OnSensorMessageEvent);
            sensor.OnLiveImage += new Sensor.OnLiveImageDelegate(OnLiveImageEvent);

            sensor.OnPilImage += Sensor_OnPilImage;
            //sensor.OnPilImageNative += Sensor_OnPilImageNative;//Native callback almost the same with non-Native, Just the memory handle
            sensor.OnZilImage += Sensor_OnZilImage;
            //sensor.OnZilImageNative += Sensor_OnZilImageNative;
            sensor.OnPointCloudImage += Sensor_OnPointCloudImage;
            sensor.OnIOChanged += new Sensor.OnIOChangedDelegate(OnSensorIOStatusChangedEvent);



            //intial the sensor parameters
            this._IPAddress = "192.168.178.200";//default IPAddress
            this._portNumber = 40;//default port number;
            this._isSensorConnected = false;
            this._AcquisitionType = ImageAcquisitionType.ProfileIntensityLaserLineThickness;
            this._acquistionMode = AcquisitionMode.RepeatSnapshot;
            this._NumberOfProfileToCapture = 1000;
            this._PackSize = 500;
            this._PacketTimeout = new TimeSpan(0, 0, 0, 0, 500);
            this._exposuresAndgains = new List<ExposureGain>();
        }
        #region callback functions

        private void OnSensorIOStatusChangedEvent(Sensor aSensor, byte aInputFlags, byte aOutputFlags)
        {
            //throw new NotImplementedException();
            //log.Info(aSensor.ToString() + "," + aInputFlags + "," + aOutputFlags);
            //log.Info("output ports:" + aOutputFlags);//2023年6月9日 17:51:23 SDK 6.0.1.19不起作用，始终返回0
        }

        private void Sensor_OnPointCloudImage(Sensor aSensor, ImageDataType aImageDataType, uint aNumPoints, uint aNumProfiles, Point3F[] aPointCloudImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            ProfileCounter++;
            timer.Start();
            point3Fs.Add(aPointCloudImageData);
            //log.Info("PointCloud callback" + intensityImage.Height);

            unsafe
            {
                IntPtr dataPointer_Intensity = Marshal.UnsafeAddrOfPinnedArrayElement(aIntensityImageData, 0);
                IntPtr dataPointer_llt = Marshal.UnsafeAddrOfPinnedArrayElement(aLaserLineThicknessImageData, 0);

                Mat _IntensityMatimage = new Mat((int)aNumProfiles, (int)(aNumPoints / aNumProfiles), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Intensity, (int)(aNumPoints / aNumProfiles) * sizeof(ushort));
                intensityImage.PushBack(_IntensityMatimage);
                Mat _lltMatimage = new Mat((int)aNumProfiles, (int)(aNumPoints / aNumProfiles), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_llt, (int)(aNumPoints / aNumProfiles) * sizeof(ushort));
                laserlinethicknessImage.PushBack(_lltMatimage);

                _IntensityMatimage.Dispose();
                _lltMatimage.Dispose();
                //Marshal.FreeHGlobal(dataPointer);
            }

            if (intensityImage.Height == _NumberOfProfileToCapture)
            {

                SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
                sRImageHandlerArgument.intensity_image = intensityImage.Clone();
                sRImageHandlerArgument.laserlinethickness_image = laserlinethicknessImage.Clone();
                sRImageHandlerArgument.imagetype = aImageDataType;

                sRImageHandlerArgument.imageheight = (uint)intensityImage.Height;
                sRImageHandlerArgument.imagewidth = (uint)intensityImage.Width;
                sRImageHandlerArgument.pointcloud = point3Fs.ToArray();
                //trigger event
                this.SensorImageEvent(this, sRImageHandlerArgument);
                //release memorey objects
                intensityImage.Dispose();
                intensityImage = new Mat();
                laserlinethicknessImage.Dispose();
                laserlinethicknessImage = new Mat();
                point3Fs.Clear();
                ProfileCounter = 0;
                timer.Stop();
                //  log.Info("PointCloud taken:" + timer.Duration + "ms");

            }

        }

        private void Sensor_OnZilImageNative(Sensor aSensor, ImageDataType aImageDataType, int aHeight, int aWidth, float aVerticalRes, float aHorizontalRes, IntPtr aZMapImageData, IntPtr aIntensityImageData, IntPtr aLaserLineThicknessImageData, float aOriginYMillimeters)
        {
            ProfileCounter++;

            //log.Info("ZIL native callback");


            switch (aImageDataType)
            {
                case ImageDataType.ZMap:
                    Mat _profileMatimage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aZMapImageData, aWidth * sizeof(UInt16));
                    profileimage.PushBack(_profileMatimage);
                    _profileMatimage.Dispose();
                    Marshal.Release(aZMapImageData); aZMapImageData = IntPtr.Zero;

                    break;
                case ImageDataType.ZMapIntensityLaserLineThickness:
                    Mat _profileMatimage1 = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aZMapImageData, aWidth * sizeof(UInt16));
                    profileimage.PushBack(_profileMatimage1);
                    Mat _intensityMatimage1 = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aIntensityImageData, aWidth * sizeof(UInt16));
                    intensityImage.PushBack(_intensityMatimage1);
                    Mat _llTImage1 = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aLaserLineThicknessImageData, aWidth * sizeof(UInt16));
                    laserlinethicknessImage.PushBack(_llTImage1);
                    _profileMatimage1.Dispose();
                    _intensityMatimage1.Dispose();
                    _llTImage1.Dispose();
                    //release unused memory 
                    Marshal.Release(aZMapImageData); aZMapImageData = IntPtr.Zero;
                    Marshal.Release(aIntensityImageData); aIntensityImageData = IntPtr.Zero;
                    Marshal.Release(aLaserLineThicknessImageData); aLaserLineThicknessImageData = IntPtr.Zero;

                    break;
                default:
                    break;
            }


            log.Info("Native ZIL image height:" + profileimage.Height);


            if (profileimage.Height == _NumberOfProfileToCapture)
            {
                SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
                sRImageHandlerArgument.profile_image = profileimage.Clone();
                sRImageHandlerArgument.intensity_image = intensityImage.Clone();
                sRImageHandlerArgument.laserlinethickness_image = laserlinethicknessImage.Clone();
                sRImageHandlerArgument.imagetype = aImageDataType;

                sRImageHandlerArgument.imageheight = (uint)profileimage.Height;
                sRImageHandlerArgument.imagewidth = (uint)profileimage.Width;
                //trigger event
                this.SensorImageEvent(this, sRImageHandlerArgument);
                //release memorey objects
                ProfileCounter = 0;
                profileimage.Dispose();
                profileimage = new Mat();
                intensityImage.Dispose();
                intensityImage = new Mat();
                laserlinethicknessImage.Dispose();
                laserlinethicknessImage = new Mat();

            }
        }

        private void Sensor_OnZilImage(Sensor aSensor, ImageDataType aImageDataType, int aHeight, int aWidth, float aVerticalRes, float aHorizontalRes, ushort[] aZMapImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, float aOriginYMillimeters)
        {
            ProfileCounter++;

            switch (aImageDataType)
            {
                case ImageDataType.ZMap:
                    unsafe
                    {
                        IntPtr dataPointer_Zmap = Marshal.UnsafeAddrOfPinnedArrayElement(aZMapImageData, 0);
                        Mat _ZmapMatimage = new Mat((int)aHeight, (int)(aWidth), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Zmap, (int)(aWidth) * sizeof(ushort));
                        profileimage.PushBack(_ZmapMatimage);
                        _ZmapMatimage.Dispose();
                    }

                    break;
                case ImageDataType.ZMapIntensityLaserLineThickness:
                    unsafe
                    {
                        IntPtr dataPointer_Zmap = Marshal.UnsafeAddrOfPinnedArrayElement(aZMapImageData, 0);
                        Mat _ZmapMatimage = new Mat((int)aHeight, (int)(aWidth), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Zmap, (int)(aWidth) * sizeof(ushort));
                        profileimage.PushBack(_ZmapMatimage);
                        _ZmapMatimage.Dispose();
                        IntPtr dataPointer_Intensity = Marshal.UnsafeAddrOfPinnedArrayElement(aIntensityImageData, 0);
                        Mat _IntensityMatimage = new Mat((int)aHeight, (int)(aWidth), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Intensity, (int)(aWidth) * sizeof(ushort));
                        intensityImage.PushBack(_IntensityMatimage);
                        _IntensityMatimage.Dispose();
                        IntPtr dataPointer_llt = Marshal.UnsafeAddrOfPinnedArrayElement(aLaserLineThicknessImageData, 0);
                        Mat _LLtMatimage = new Mat((int)aHeight, (int)(aWidth), Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_llt, (int)(aWidth) * sizeof(ushort));
                        laserlinethicknessImage.PushBack(_LLtMatimage);
                        _LLtMatimage.Dispose();
                    }
                    break;
                default:
                    break;
            }

            if (profileimage.Height == _NumberOfProfileToCapture)
            {
                SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
                sRImageHandlerArgument.profile_image = profileimage.Clone();
                sRImageHandlerArgument.intensity_image = intensityImage.Clone();
                sRImageHandlerArgument.laserlinethickness_image = laserlinethicknessImage.Clone();
                sRImageHandlerArgument.imagetype = aImageDataType;

                sRImageHandlerArgument.imageheight = (uint)profileimage.Height;
                sRImageHandlerArgument.imagewidth = (uint)profileimage.Width;
                //trigger event
                this.SensorImageEvent(this, sRImageHandlerArgument);
                //release memorey objects
                ProfileCounter = 0;
                profileimage.Dispose();
                profileimage = new Mat();
                intensityImage.Dispose();
                intensityImage = new Mat();
                laserlinethicknessImage.Dispose();
                laserlinethicknessImage = new Mat();
            }

            //log.Info("ZIL  callback");

        }

        private void Sensor_OnPilImageNative(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, IntPtr aProfileImageData, IntPtr aIntensityImageData, IntPtr aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            ProfileCounter++;

            //log.Info("ZIL native callback");


            switch (aImageDataType)
            {
                case ImageDataType.Profile:
                    Mat _profileMatimage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aProfileImageData, aWidth * sizeof(UInt16));
                    profileimage.PushBack(_profileMatimage);
                    _profileMatimage.Dispose();
                    Marshal.Release(aProfileImageData); aProfileImageData = IntPtr.Zero;

                    break;
                case ImageDataType.ProfileIntensityLaserLineThickness:
                    Mat _profileMatimage1 = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aProfileImageData, aWidth * sizeof(UInt16));
                    profileimage.PushBack(_profileMatimage1);
                    Mat _intensityMatimage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aIntensityImageData, aWidth * sizeof(UInt16));
                    intensityImage.PushBack(_intensityMatimage);
                    Mat _llTImage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, aLaserLineThicknessImageData, aWidth * sizeof(UInt16));
                    laserlinethicknessImage.PushBack(_llTImage);
                    _profileMatimage1.Dispose();
                    _intensityMatimage.Dispose();
                    _llTImage.Dispose();
                    //release unused memory 
                    Marshal.Release(aProfileImageData); aProfileImageData = IntPtr.Zero;
                    Marshal.Release(aIntensityImageData); aIntensityImageData = IntPtr.Zero;
                    Marshal.Release(aLaserLineThicknessImageData); aLaserLineThicknessImageData = IntPtr.Zero;
                    break;
                default:
                    break;
            }

            //Handle meta data
            if (mMetaDataLevel != MetaDataLevel.Off)
            {
                metaDataCollectionList.Add(aMetaDataCollection);
            }

            log.Info("Native Profile image height:" + profileimage.Height);

            if (profileimage.Height == _NumberOfProfileToCapture)
            {
                SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
                sRImageHandlerArgument.profile_image = profileimage.Clone();
                sRImageHandlerArgument.intensity_image = intensityImage.Clone();
                sRImageHandlerArgument.laserlinethickness_image = laserlinethicknessImage.Clone();
                sRImageHandlerArgument.imagetype = aImageDataType;

                sRImageHandlerArgument.imageheight = (uint)profileimage.Height;
                sRImageHandlerArgument.imagewidth = (uint)profileimage.Width;
                //Set the meta data values

                sRImageHandlerArgument.MetaDataList = metaDataCollectionList;

                //trigger event
                this.SensorImageEvent(this, sRImageHandlerArgument);
                //release memorey objects
                ProfileCounter = 0;
                profileimage.Dispose();
                profileimage = new Mat();
                intensityImage.Dispose();
                intensityImage = new Mat();
                laserlinethicknessImage.Dispose();
                laserlinethicknessImage = new Mat();
                metaDataCollectionList.Clear();


            }
        }

        private void Sensor_OnPilImage(Sensor aSensor, ImageDataType aImageDataType, int aOriginX, int aHeight, int aWidth, ushort[] aProfileImageData, ushort[] aIntensityImageData, ushort[] aLaserLineThicknessImageData, MetaDataCollection aMetaDataCollection)
        {
            //log.Info("PIL callback");
            switch (aImageDataType)
            {
                case ImageDataType.Profile:
                    unsafe
                    {
                        IntPtr dataPointer_Profile = Marshal.UnsafeAddrOfPinnedArrayElement(aProfileImageData, 0);
                        Mat _profileMatimage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Profile, aWidth * sizeof(UInt16));
                        profileimage.PushBack(_profileMatimage);
                        _profileMatimage.Dispose();
                    }

                    break;
                case ImageDataType.ProfileIntensityLaserLineThickness:
                    unsafe
                    {
                        IntPtr dataPointer_Profile = Marshal.UnsafeAddrOfPinnedArrayElement(aProfileImageData, 0);
                        IntPtr dataPointer_Intensity = Marshal.UnsafeAddrOfPinnedArrayElement(aIntensityImageData, 0);
                        IntPtr dataPointer_llt = Marshal.UnsafeAddrOfPinnedArrayElement(aLaserLineThicknessImageData, 0);

                        Mat _profileMatimage1 = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Profile, aWidth * sizeof(UInt16));
                        profileimage.PushBack(_profileMatimage1);
                        Mat _intensityMatimage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_Intensity, aWidth * sizeof(UInt16));
                        intensityImage.PushBack(_intensityMatimage);
                        Mat _llTImage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv16U, 1, dataPointer_llt, aWidth * sizeof(UInt16));
                        laserlinethicknessImage.PushBack(_llTImage);
                        _profileMatimage1.Dispose();
                        _intensityMatimage.Dispose();
                        _llTImage.Dispose();
                    }
                    break;
                default:
                    break;
            }

            //Handle meta data
            if (mMetaDataLevel != MetaDataLevel.Off)
            {
                metaDataCollectionList.Add(aMetaDataCollection);
            }

            //log.Info("Profile image height:" + profileimage.Height);

            if (profileimage.Height == _NumberOfProfileToCapture)
            {
                SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
                sRImageHandlerArgument.profile_image = profileimage.Clone();
                sRImageHandlerArgument.intensity_image = intensityImage.Clone();
                sRImageHandlerArgument.laserlinethickness_image = laserlinethicknessImage.Clone();
                sRImageHandlerArgument.imagetype = aImageDataType;

                sRImageHandlerArgument.imageheight = (uint)profileimage.Height;
                sRImageHandlerArgument.imagewidth = (uint)profileimage.Width;
                //Set the meta data values
                sRImageHandlerArgument.MetaDataList = metaDataCollectionList;
                //trigger event
                this.SensorImageEvent(this, sRImageHandlerArgument);
                //release memorey objects
                ProfileCounter = 0;
                profileimage.Dispose();
                profileimage = new Mat();
                intensityImage.Dispose();
                intensityImage = new Mat();
                laserlinethicknessImage.Dispose();
                laserlinethicknessImage = new Mat();
                metaDataCollectionList.Clear();
            }
        }


        private void OnLiveImageEvent(Sensor aSensor, int aOriginX, int aWidth, int aHeight, IntPtr aImageDataPtr)
        {
            //throw new NotImplementedException();
            Mat _liveImage = new Mat(aHeight, aWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 1, aImageDataPtr, aWidth * sizeof(byte));

            SRImageHandlerArgument sRImageHandlerArgument = new SRImageHandlerArgument();
            sRImageHandlerArgument.imagetype = ImageDataType.LiveImage;
            sRImageHandlerArgument.imageheight = (uint)aHeight;
            sRImageHandlerArgument.imagewidth = (uint)aWidth;
            sRImageHandlerArgument.liveimage = _liveImage.Clone();
            _liveImage.Dispose();
            this.SensorImageEvent(this, sRImageHandlerArgument);

        }

        private void OnSensorMessageEvent(Sensor aSensor, MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {
            //throw new NotImplementedException();
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
                //log.Info("Data message:" + aMsg + ";" + "Message data:" + aMsgData);
            }
        }



        private void OnDisconnectedEvent(Sensor aSensor)
        {
            this._isSensorConnected = false;

        }

        private void OnConnectedEvent(Sensor aSensor)
        {
            this._isSensorConnected = true;
        }

        #endregion


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
                    //sensor.SendParameterSetToSensor(value);
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
                    //log.Info("Get Image Acquisition Type:" + _AcquisitionType.ToString());
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


        private uint _PacketCounter;

        public uint PacketCounter
        {
            get
            {
                if (_isSensorConnected)
                {
                    _PacketCounter = this._NumberOfProfileToCapture / _PackSize;
                }
                return _PacketCounter;
            }

            set
            {
                _PacketCounter = value;
                if (_isSensorConnected)
                {
                    _PacketCounter = this._NumberOfProfileToCapture / _PackSize;
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

        private DataTriggerMode _sensorDataTriggerMode;

        public DataTriggerMode SensorDataTriggerMode
        {
            get
            {
                if (_isSensorConnected)
                {
                    sensor.GetDataTriggerMode(out _sensorDataTriggerMode);
                }
                return _sensorDataTriggerMode;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetDataTriggerMode(value);
                }
                _sensorDataTriggerMode = value;
            }
        }

        private uint _sensorInternalTriggerFreq;

        public uint SensorInternalTriggerFreq
        {
            get
            {
                if (_isSensorConnected)
                {
                    _sensorInternalTriggerFreq = (uint)sensor.GetDataTriggerInternalFrequency();

                }
                return _sensorInternalTriggerFreq;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetDataTriggerInternalFrequency((int)value);
                }
                _sensorInternalTriggerFreq = value;
            }
        }

        private bool _StartTriggerEnable;

        public bool StartTriggerEnable
        {
            get
            {
                if (_isSensorConnected)
                {
                    sensor.GetStartTrigger(out StartTriggerSource source, out _StartTriggerEnable, out TriggerEdgeMode edgeMode);
                }
                return _StartTriggerEnable;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetStartTrigger(StartTriggerSource.Input0, value, TriggerEdgeMode.RisingEdge);
                }
                _StartTriggerEnable = value;
            }
        }


        private List<ExposureGain> _exposuresAndgains;

        public List<ExposureGain> ExposuresAndGains
        {
            get
            {
                _exposuresAndgains.Clear();
                if (_isSensorConnected)
                {
                    int exposureCounter = sensor.GetNumberOfExposureTimes();
                    sensor.GetGain(out bool isGainEnable, out int gainValue);
                    for (int i = 0; i < exposureCounter; i++)
                    {
                        sensor.GetExposureDuration(i, out double _expo);
                        _exposuresAndgains.Add(new ExposureGain(_expo, gainValue));
                    }
                }
                else
                {
                    log.Warn("sensor not connected,can't get the exposure time and gain values");

                }
                return _exposuresAndgains;
            }
            set
            {
                if (_isSensorConnected)
                {
                    int exposureCount = value.Count;
                    if (exposureCount > 1)
                    {
                        sensor.SetNumberOfExposureTimes(exposureCount);
                    }
                    for (int i = 0; i < exposureCount; i++)
                    {
                        if (sensor.MinExposureTimeInMicroSeconds < value[i].ExposureTime && sensor.MaxExposureTimeInMicroSeconds > value[i].ExposureTime)
                        {
                            sensor.SetExposureDuration(i, value[i].ExposureTime);
                            if (value[i].Gain > 0)
                            {
                                sensor.SetGain(true, value[i].Gain);
                            }
                        }
                        else
                        {
                            log.Error("Exposure Time out of range:\n" + "Limited Maximum Exposure time:" + sensor.MaxExposureTimeInMicroSeconds + "Limited Minimum Exposure Time:" + sensor.MinExposureTimeInMicroSeconds);
                        }
                    }
                }
                _exposuresAndgains = value;
            }
        }


        private AcquisitionMode _acquistionMode;

        public AcquisitionMode acquisitionMode
        {
            get
            {
                if (_isSensorConnected)
                {
                    _acquistionMode = sensor.GetAcquisitionMode();
                }
                return _acquistionMode;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetAcquisitionMode(value);
                }
                _acquistionMode = value;
            }
        }


        private BinningMode _horizentalBinningMode;

        public BinningMode HorizentalBinningMode
        {
            get
            {
                if (_isSensorConnected)
                {
                    _horizentalBinningMode = sensor.GetHorizontalBinning();
                }
                return _horizentalBinningMode;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetHorizontalBinning(value);
                }
                _horizentalBinningMode = value;
            }
        }

        private BinningMode _verticalBinningeMode;

        public BinningMode VerticalBinningMode
        {
            get
            {
                if (_isSensorConnected)
                {
                    _verticalBinningeMode = sensor.GetVerticalBinning();
                }
                return _verticalBinningeMode;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetVerticalBinning(value);
                }
                _verticalBinningeMode = value;
            }
        }


        private string _SensorFWVersion;

        public string SensorFWVersion
        {
            get
            {
                if (_isSensorConnected)
                {
                    _SensorFWVersion = sensor.FirmwareVersion.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\0", "").Trim();
                }
                return _SensorFWVersion;
            }
        }


        private float _sensorTempearture;

        public float SensorTemperature
        {
            get
            {
                if (_isSensorConnected)
                {
                    _sensorTempearture = sensor.GetTemperature();
                }
                return _sensorTempearture;
            }

        }


        private float _tiltAnglePitch;

        public float TiltAnglePitch
        {
            get
            {
                if (_isSensorConnected)
                {
                    _tiltAnglePitch = sensor.GetTiltCorrectionPitch();
                }
                return _tiltAnglePitch;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetTiltCorrectionPitch(value);
                }
                _tiltAnglePitch = value;
            }
        }


        private float _tiltAngleYaw;

        public float TiltAngleYaw
        {
            get
            {
                if (_isSensorConnected)
                {
                    _tiltAngleYaw = sensor.GetTiltCorrectionYaw();
                }
                return _tiltAngleYaw;
            }
            set
            {
                if (_isSensorConnected)
                {
                    sensor.SetTiltCorrectionYaw(value);
                }
                _tiltAngleYaw = value;
            }
        }



        private string _sensorModel;

        public string SensorModel
        {
            get
            {
                if (_isSensorConnected)
                {
                    _sensorModel = sensor.ModelName.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\0", "").Trim(); ;
                }
                return _sensorModel;
            }
        }


        private float _transportResolution;

        public float TransportResolution
        {
            get
            {
                if (_isSensorConnected)
                {
                    _transportResolution = sensor.GetTransportResolution();
                }
                return _transportResolution;
            }
            set
            {
                if (_isSensorConnected)
                    sensor.SetTransportResolution(value);
                _transportResolution = value;
            }
        }


        private bool xEnhancement;

        public bool XEhancement
        {
            get
            {
                if (_isSensorConnected)
                {
                    xEnhancement = sensor.GetCalibrationDataXEnhancement();
                }
                return xEnhancement;
            }
            set
            {
                if (_isSensorConnected)
                    sensor.SetCalibrationDataXEnhancement(value);
                xEnhancement = value;
            }
        }
        private MetaDataLevel mMetaDataLevel;

        public MetaDataLevel MetaDataLevel
        {
            get
            {
                if (_isSensorConnected)
                {
                    mMetaDataLevel = sensor.GetMetaDataLevel();
                }
                return mMetaDataLevel;
            }
            set
            {
                if (_isSensorConnected) sensor.SetMetaDataLevel(value);
                mMetaDataLevel = value;
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
                    timer.Start();
                    IPEndPoint ipSensorEndPoint = new IPEndPoint(IPAddress.Parse(_IPAddress), _portNumber);
                    TimeSpan timeSpan = new TimeSpan(15000);
                    sensor.Connect(ipSensorEndPoint, timeSpan);
                    timer.Stop();
                    log.Info("Connect sensor taken:" + timer.Duration + "ms");
                    timer.Start();
                    sensor.LoadCalibrationDataFromSensor();
                    //sensor.SetHorizontalBinning(BinningMode.Off);
                    log.Info("Load Calibration file taken:" + timer.Duration + "ms\nSensor connected!");
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
                        PacketCounter = this.NumberOfProfileToCapture / this.PackSize;
                        sensor.StartAcquisition();
                        //sensor.SetAcquisitionMode
                        startAcqusition = true;
                    }
                    else
                    {
                        startAcqusition = false;
                        log.Error("sensor not connected yet or the sensor disconnected,\nPlease check the sensor status");
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


        public Task<bool> StopAcquisition()
        {
            Task<bool> stopAcqTask = new Task<bool>(() =>
            {
                bool stopAcqusition = false;
                try
                {
                    if (_isSensorConnected)
                    {
                        sensor.StopAcquisition();
                        stopAcqusition = true;
                    }
                    else
                    {
                        stopAcqusition = false;
                        log.Error("sensor not connected yet or the sensor disconnected,\nPlease check the sensor status");
                    }
                }
                catch (Exception ce)
                {
                    stopAcqusition = false;
                    log.Error("Execute sensor stop acquisition failed,\nPlease check the sensor status, the error message as below:\n" + ce.Message);
                }
                return stopAcqusition;

            });
            stopAcqTask.Start();
            return stopAcqTask;

        }


        public Task<bool> WriteIO(DigitalOutput Output_port_number)
        {
            Task<bool> writeIotask = new Task<bool>(() =>
            {
                bool succeeded = false;
                try
                {
                    sensor.SetDigitalOutput(Output_port_number, true);
                    Thread.Sleep(10);
                    sensor.SetDigitalOutput(Output_port_number, false);

                    succeeded = true;
                }
                catch (Exception ce)
                {
                    log.Error("Set sensor output failed,Please check the sensor status, the error message as below:\n" + ce.Message);
                }
                return succeeded;
            });
            writeIotask.Start();
            return writeIotask;
        }

        public Task<bool> SaveSensorParameters(string fileName)
        {
            Task<bool> SaveParaTask = new Task<bool>(() =>
            {
                bool succeeded = false;
                if (_isSensorConnected)
                {
                    sensor.SaveParameterSet(fileName);
                    succeeded = true;
                }
                return succeeded;

            });
            SaveParaTask.Start();
            return SaveParaTask;
        }

        public void clearbuffer()
        {
            ProfileCounter = 0;

            profileimage.Dispose(); profileimage = new Mat();
            intensityImage.Dispose(); intensityImage = new Mat();
            laserlinethicknessImage.Dispose(); laserlinethicknessImage = new Mat();

            //test for pointcloud data management list 
            point3Fs.Clear();
            metaDataCollectionList.Clear();
        }

        public void Dispose()
        {
            if (this.sensor != null)
            {
                sensor.Dispose(); sensor = null;
            }
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


    /// <summary>
    /// 1. define event args
    /// </summary>
    public class SRImageHandlerArgument : EventArgs
    {
        public Mat liveimage;
        public Mat profile_image;
        public Mat intensity_image;
        public Mat laserlinethickness_image;
        public Point3F[][] pointcloud;
        public uint imageheight;
        public uint imagewidth;
        public ImageDataType imagetype;
        public List<MetaDataCollection> MetaDataList;
        public SRImageHandlerArgument()
        {
            liveimage = null;
            profile_image = null;
            intensity_image = null;
            laserlinethickness_image = null;
            pointcloud = null;
            imageheight = 0;
            imagewidth = 0;
            imagetype = ImageDataType.Invalid;
            MetaDataList = null;
        }

    }

    /// <summary>
    /// define the exposure time and gain values parameters
    /// </summary>

    public struct ExposureGain
    {
        private double _exposureTime;

        public double ExposureTime
        {
            get
            {
                return _exposureTime;
            }
            set
            {
                if (_exposureTime > 0)
                {
                    _exposureTime = value;
                }
                else
                {
                    _exposureTime = 10;
                }
            }
        }

        private int _gain;

        public int Gain
        {
            get
            {
                return _gain;
            }
            set
            {
                //check the gain value, this value can't set bigger than 5
                if (value >= 0 && value <= 5)
                {
                    _gain = value;
                }
                else
                {
                    _gain = 0;
                }
            }
        }

        /// <summary>
        /// Initial the exposure and gain value
        /// </summary>
        /// <param name="exposure">exposure time, value from 0 to 1000</param>
        /// <param name="gain"></param>
        public ExposureGain(double exposure = 10, int gain = 0)
        {
            this._exposureTime = exposure;
            this._gain = gain;
        }

    }

}
