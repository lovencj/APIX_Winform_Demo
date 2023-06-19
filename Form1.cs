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
using System.Threading;



//define log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace APIX_Winform_Demo
{
    public partial class Form1 : Form
    {
        private readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isStarted = false;
        private HiPerfTimer HiPerfTimer = new HiPerfTimer();

        APIXSensor Sensor1 = new APIXSensor();

        public Form1()
        {
            InitializeComponent();
            tbx_NumberOfProfile.Enabled = false;
            tBx_PacketSize.Enabled = false;
            tBx_PacketTimeout.Enabled = false;




            log.Info("The UI initialed!");
            HiPerfTimer.Start();
            initSR_APIx();
            HiPerfTimer.Stop();
            log.Info("Initial APIx taken:" + HiPerfTimer.Duration + "ms");


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
            Sensor1.WriteIO(DigitalOutput.Channel2);

            //Cross thread to display image
            new Task(new Action(() =>
            {
                cv_imageBox1.Invoke(new Action(() =>
                {
                    //cv_imageBox1.Image.Dispose();
                    cv_imageBox1.Image = SRimageHandlerArgument.profile_image;
                }));

                tbx_SensorTempetature.Invoke((Action)(() =>
                {
                    tbx_SensorTempetature.Text = Sensor1.SensorTemperature.ToString("0.00") + "℃";
                }));
            })).Start();

            //Thread.Sleep(300);


            GC.Collect();
        }

        private void OnSensorMessgae(MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {

            //throw new NotImplementedException();
        }

        private async void btn_InitialSensor_Click(object sender, EventArgs e)
        {
            HiPerfTimer.Start();
            var result = await Sensor1.Connect();
            log.Info(Sensor1.SensorFWVersion);
            HiPerfTimer.Stop();
            log.Info("Connect sensor taken:" + HiPerfTimer.Duration + "ms");
            HiPerfTimer.Start();
            Sensor1.AcquisitionType = ImageAcquisitionType.ZMapIntensityLaserLineThickness;
            Sensor1.NumberOfProfileToCapture = 1000;
            Sensor1.PackSize = 100;
            Sensor1.SensorDataTriggerMode = DataTriggerMode.FreeRunning;
            Sensor1.SensorInternalTriggerFreq = 1500;
            Sensor1.StartTriggerEnable = Enabled;
            Sensor1.acquisitionMode = AcquisitionMode.RepeatSnapshot;
            Sensor1.TiltAnglePitch = 0;
            Sensor1.TiltAngleYaw = -19;
            //Sensor1.HorizentalBinningMode = BinningMode.X2;
            //Sensor1.VerticalBinningMode = BinningMode.X2;
            List<ExposureGain> exposureGains = new List<ExposureGain>();
            exposureGains.Add(new ExposureGain(4d, 3));
            exposureGains.Add(new ExposureGain(60d, 3));
            Sensor1.ExposuresAndGains = exposureGains;
            HiPerfTimer.Stop();
            log.Info("set sensor parameters taken:" + HiPerfTimer.Duration + "ms");

            //disable the button
            btn_InitialSensor.Enabled = false;
            tbx_NumberOfProfile.Enabled = true;
            tBx_PacketSize.Enabled = true;
            tBx_PacketTimeout.Enabled = true;

            //display the parameters
            tbx_NumberOfProfile.Text = Sensor1.NumberOfProfileToCapture.ToString();
            tBx_PacketSize.Text = Sensor1.PackSize.ToString();
            tBx_PacketTimeout.Text = Sensor1.PacketTimeout.ToString();

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
                log.Info("Sensor acquisition mode:" + Sensor1.acquisitionMode);
                log.Info("Sensor pitch angle:"+Sensor1.TiltAnglePitch);
                log.Info("Sensor Yaw angle:" + Sensor1.TiltAngleYaw);
                Sensor1.SensorROI = new ROI(0, 1920, 284, 480);
                var s = await Sensor1.StartAcquisition();

                //log.Info(Sensor1.HorizentalBinningMode);
                //log.Info(Sensor1.VerticalBinningMode);
                // var s1 = await Sensor1.WriteIO(DigitalOutput.Channel2);

            }
            else if (!isStarted)
            {
                log.Info("Sensor stop acquisition");
                var s1 = await Sensor1.StopAcquisition();
            }

            tbx_NumberOfProfile.Enabled = !isStarted;
            tBx_PacketSize.Enabled = !isStarted;
            tBx_PacketTimeout.Enabled = !isStarted;



            //set the button color
            btn_StartAcquisition.BackColor = isStarted ? Color.Green : Color.Red;
            btn_StartAcquisition.Text = isStarted ? "Sensor running" : "Sensor Stop";
        }

        private void btn_SimulateTrigger_Click(object sender, EventArgs e)
        {
            Sensor1.WriteIO(DigitalOutput.Channel2);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.isSensorConnected)
            {
                Sensor1.NumberOfProfileToCapture = uint.Parse(tbx_NumberOfProfile.Text);
            }
        }

        private void tBx_PacketSize_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.isSensorConnected)
            {
                Sensor1.PackSize = uint.Parse(tBx_PacketSize.Text);
            }
        }

        private void tBx_PacketTimeout_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.isSensorConnected)
            {
                Sensor1.PacketTimeout = TimeSpan.Parse(tBx_PacketTimeout.Text);
            }
        }
    }
}
