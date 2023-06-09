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
        private HiPerfTimer HiPerfTimer = new HiPerfTimer();

        APIXSensor Sensor1 = new APIXSensor();

        public Form1()
        {
            InitializeComponent();
            log.Info("The UI initialed!");
            HiPerfTimer.Start();
            initSR_APIx();
            HiPerfTimer.Stop();
            log.Info("Initial APIx taken:" + HiPerfTimer.Duration+"ms");


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
            log.Info("Acquisition completed, trigger the sensor again and display");
            Sensor1.WriteIO(DigitalOutput.Channel1);

            //Cross thread to display image
            new Task(new Action(() =>
            {
                cv_imageBox1.Invoke(new Action(() =>
                {
                    //cv_imageBox1.Image.Dispose();
                    cv_imageBox1.Image = SRimageHandlerArgument.profile_image;
                }));
            })).Start();
            GC.Collect();
        }

        private void OnSensorMessgae(MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {

            //throw new NotImplementedException();
        }

        private async void btn_InitialSensor_Click(object sender, EventArgs e)
        {
            HiPerfTimer.Start();
            var result= await Sensor1.Connect();
            HiPerfTimer.Stop();
            log.Info("Connect sensor taken:" + HiPerfTimer.Duration + "ms");
            HiPerfTimer.Start();
            Sensor1.AcquisitionType = ImageAcquisitionType.ZMapIntensityLaserLineThickness;
            Sensor1.NumberOfProfileToCapture = 1000;
            Sensor1.PackSize = 100;
            Sensor1.SensorDataTriggerMode = DataTriggerMode.FreeRunning;
            Sensor1.SensorInternalTriggerFreq = 3000;
            Sensor1.StartTriggerEnable= Enabled;
            Sensor1.acquisitionMode = AcquisitionMode.RepeatSnapshot;
            Sensor1.HorizentalBinningMode = BinningMode.X2;
            Sensor1.VerticalBinningMode = BinningMode.X2;
            List<ExposureGain> exposureGains = new List<ExposureGain>();
            exposureGains.Add(new ExposureGain(10d, 3));
            exposureGains.Add(new ExposureGain(50d, 2));
            Sensor1.ExposuresAndGains = exposureGains;
            HiPerfTimer.Stop();
            log.Info("set sensor parameters taken:" + HiPerfTimer.Duration + "ms");

            //disable the button
            btn_InitialSensor.Enabled = false;
            //Sensor1.ExposuresAndGains.Add(new ExposureGain(30d, 2));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Sensor1.SaveParameterSet("MyParameters.json");
        }

        private async void btn_StartAcquisition_Click(object sender, EventArgs e)
        {
            isStarted = !isStarted;
            if (isStarted)
            {
                log.Info("Number of profile to capture:" + Sensor1.NumberOfProfileToCapture);
                log.Info("Packsize:" + Sensor1.PackSize);
                log.Info("Image Type:" + Sensor1.AcquisitionType);
                log.Info("exposure and gain:" + Sensor1.ExposuresAndGains.Count);
                Sensor1.SensorROI = new ROI(0, 4096, 498, 370);
                var s=await Sensor1.StartAcquisition();

                log.Info(Sensor1.HorizentalBinningMode);
                log.Info(Sensor1.VerticalBinningMode);
                //var s1 = await Sensor1.WriteIO(DigitalOutput.Channel2);
                
            }
            else if (!isStarted)
            {
                log.Info("Sensor stop acquisition");
                var s1=await Sensor1.StopAcquisition();
            }
            //set the button color
            btn_StartAcquisition.BackColor= isStarted? Color.Green : Color.Red;
            btn_StartAcquisition.Text = isStarted ? "Sensor running" : "Sensor Stop";
        }
    }
}
