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

//define log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace APIX_Winform_Demo
{
    public partial class Form1 : Form
    {
        private readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isStarted = false;
        private readonly HiPerfTimer HiPerfTimer = new HiPerfTimer();

        APIXSensor Sensor1 = new APIXSensor();
        readonly SensorHelper sensorHelper = new SensorHelper();
        public Form1()
        {
            InitializeComponent();
            tbx_NumberOfProfile.Enabled = false;
            tBx_PacketSize.Enabled = false;
            tBx_PacketTimeout.Enabled = false;

            gbx_binning.Visible = false;
            gbx_SensorInfo.Visible = false;
            gbx_SensorPar.Visible = false;
            gbx_SaveImage.Visible = false;
            gbx_StartAcquisition.Visible = false;

            btn_StartAcquisition.Enabled= false;
            btn_SimulateTrigger.Enabled= false;


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
            //Sensor1.WriteIO(DigitalOutput.Channel2);

            var tempSensor = sensor as APIXSensor;

            //Cross thread to display image and save files
            new Task(new Action(() =>
            {
                cv_imageBox1.Invoke(new Action(() =>
                {
                    switch (SRimageHandlerArgument.imagetype)
                    {
                        case ImageDataType.Profile:
                            cv_imageBox1.Image = SRimageHandlerArgument.profile_image;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.profile_image.Save(tbx_SaveImageFilePath.Text + "\\ProfileImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                            }

                            break;
                        case ImageDataType.ProfileIntensityLaserLineThickness:
                            cv_imageBox1.Image = SRimageHandlerArgument.intensity_image;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.profile_image.Save(tbx_SaveImageFilePath.Text + "\\ProfileImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\IntensityImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\LaserLineThicknessImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");

                            }

                            break;
                        case ImageDataType.ZMap:
                            cv_imageBox1.Image = SRimageHandlerArgument.profile_image;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.profile_image.Save(tbx_SaveImageFilePath.Text + "\\Zmap" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                            }

                            break;
                        case ImageDataType.ZMapIntensityLaserLineThickness:
                            cv_imageBox1.Image = SRimageHandlerArgument.intensity_image;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.profile_image.Save(tbx_SaveImageFilePath.Text + "\\Zmap" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\ZmapIntensityImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\ZmapLaserLineThicknessImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                            }


                            break;
                        case ImageDataType.LiveImage:
                            cv_imageBox1.Image = SRimageHandlerArgument.liveimage;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.liveimage.Save(tbx_SaveImageFilePath.Text + "\\ProfileImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".bmp");
                            }


                            break;
                        case ImageDataType.PointCloud:
                            cv_imageBox1.Image = SRimageHandlerArgument.intensity_image;
                            if (ckb_EnableSaveFiles.Checked)
                            {
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\ProfileIntensityImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                SRimageHandlerArgument.intensity_image.Save(tbx_SaveImageFilePath.Text + "\\ProfileLaserLineThicknessImage" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".png");
                                sensorHelper.SaveToPly(tbx_SaveImageFilePath.Text + "\\PointCloud" + DateTime.Now.ToString("MMddHH-mm-ss-fff") + ".ply", SRimageHandlerArgument.pointcloud);
                            }

                            break;
                        default:
                            break;
                    }


                }));

                tbx_SensorTempetature.Invoke((Action)(() =>
                {
                    //ECCO X025不支持获取传感器温度
                    if (Sensor1.SensorModel.Contains("ECCO 95"))
                    {
                        tbx_SensorTempetature.Text = tempSensor.SensorTemperature.ToString("0.00") + "℃";//it's not working with ECCO X series sensor
                    }
                }));
                this.Invoke((Action)(() =>
                {
                    this.Text= "Current Scan rate: "+tempSensor.CurrentScanRate.CurrentScanRate.ToString()+",is overflow: "+ tempSensor.CurrentScanRate.isTriggerOverflow.ToString();
                    //tbx_SensorTempetature.Text = tempSensor.SensorTemperature.ToString("0.00") + "℃";//it's not working with ECCO X series sensor
                }));
            })).Start();
            GC.Collect();
            //Sensor1.WriteIO(DigitalOutput.Channel2);

        }

        private void OnSensorMessgae(MessageType aMsgType, SubMessageType aSubMsgType, int aMsgData, string aMsg)
        {

            //throw new NotImplementedException();
        }

        private async void btn_InitialSensor_Click(object sender, EventArgs e)
        {
            HiPerfTimer.Start();
            var result = await Sensor1.Connect();
            if (result)
            {
                log.Info(Sensor1.SensorFWVersion);
                HiPerfTimer.Stop();
                log.Info("Connect sensor taken:" + HiPerfTimer.Duration + "ms");
                HiPerfTimer.Start();
                Sensor1.AcquisitionType = ImageAcquisitionType.ZMapIntensityLaserLineThickness;
                Sensor1.NumberOfProfileToCapture = 2000;
                Sensor1.PackSize = 100;
                Sensor1.PacketTimeout = new TimeSpan(0, 0, 0, 0, 0);
                Sensor1.SensorDataTriggerMode = DataTriggerMode.Internal;
                Sensor1.DataTriggerSource = DataTriggerSource.QuadEncoder;
                Sensor1.externalTriggerParameter = new ExternalTriggerParameter(12, 0,TriggerEdgeMode.RisingEdge);
                Sensor1.SensorInternalTriggerFreq = 2000;
                Sensor1.StartTriggerEnable = Enabled;
                Sensor1.AcquisitionMode = AcquisitionMode.RepeatSnapshot;
                Sensor1.TiltAnglePitch = -19f;
                Sensor1.TiltAngleYaw = -19f;
                Sensor1.TransportResolution = 0.012f;
                Sensor1.MetaDataLevel = MetaDataLevel.Version2;
                Sensor1.ZmapResolution = new ZmapResolution(0.001f, 0.012f);
                log.Info($"{Sensor1.SensorModel}");
                //if (Sensor1.SensorModel.Contains("ECCO X")) //binning mode just support the ECCO X series sensors
                //{
                //    Sensor1.HorizentalBinningMode = BinningMode.X2;
                //    Sensor1.VerticalBinningMode = BinningMode.X2;

                //}
                List<ExposureGain> exposureGains = new List<ExposureGain>
                {
                    //exposureGains.Add(new ExposureGain(4d, 3));
                   // new ExposureGain(10d, 2),
                    new ExposureGain(10d, 3),new ExposureGain(60d, 3),
                };
                Sensor1.ExposuresAndGains = exposureGains;

                Sensor1.SensorROI = new ROI(0, 1920, 380, 300);


                //Sensor1.ConfigFilePath=tbx_ConfigFilePath.Text="";

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

                gbx_SaveImage.Visible = true;
                gbx_SensorInfo.Visible = true;
                gbx_SensorPar.Visible = true;
                gbx_StartAcquisition.Visible = true;

                btn_StartAcquisition.Enabled = true;
                btn_SimulateTrigger.Enabled = true;


                tbx_APIXVersion.Text = ApiManager.Version;
                tbx_SensorModel.Text = Sensor1.SensorModel;
                tbx_FirmwareVersion.Text = Sensor1.SensorFWVersion;

                if (Sensor1.SensorModel != null)
                {
                    gbx_binning.Visible = Sensor1.SensorModel.Contains("ECCO X");
                }

                comboBox_ImageType.SelectedIndex = 2;
            }
        }

        private void btn_SaveConfigrationFile_Click(object sender, EventArgs e)
        {
            //Sensor1.SaveParameterSet("MyParameters.json");
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "please select your file path:",
                RestoreDirectory = true,
                Filter = "*.json|*.json|*.par|*.par|All files(*.*)|*.*",

            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Sensor1.SaveSensorParameters(saveFileDialog.FileName);
            }
            //Sensor1.SaveSensorParameters(Sensor1.SensorModel.Substring(0, 12) + "_" + DateTime.Now.ToString("yyyyMMddHHmm-ss-fff") + "_PC.json");
        }

        private async void btn_StartAcquisition_Click(object sender, EventArgs e)
        {
            isStarted = !isStarted;
            if (isStarted)
            {
                Sensor1.Clearbuffer();
                log.Info("Number of profile to capture:" + Sensor1.NumberOfProfileToCapture);
                log.Info("Packsize:" + Sensor1.PackSize);
                log.Info("Image Type:" + Sensor1.AcquisitionType);
                log.Info("exposure and gain:" + Sensor1.ExposuresAndGains.Count);
                log.Info("Sensor acquisition mode:" + Sensor1.AcquisitionMode);
                log.Info("Sensor pitch angle:" + Sensor1.TiltAnglePitch);
                log.Info("Sensor Yaw angle:" + Sensor1.TiltAngleYaw);
                log.Info("Sensor X enhancement:"+Sensor1.XEhancement);
                log.Info("Sensor Data Trigger mode:" + Sensor1.SensorDataTriggerMode);
                log.Info("Sensor Data Trigger source:" + Sensor1.DataTriggerSource);
                log.Info("Sensor data trigger parameters:" + "Trigger divider:" + Sensor1.externalTriggerParameter.TriggerDivider + ", Trigger delay:" + Sensor1.externalTriggerParameter.TriggerDelay + ", Trigger Edge mode:" + Sensor1.externalTriggerParameter.TriggerEdgeMode);
                log.Info("Sensor Maximun scan rate:" + Sensor1.MaximumScanRate + ", Distance Pre circle:" + Sensor1.DistancePreCircle + ", Trigger divider:" + Sensor1.externalTriggerParameter.TriggerDivider);
                log.Info("Sensor Maximun running speed is: MaximumScanRate x DistancePreCircle x TriggerDivider=" + Sensor1.MaximumScanRate * Sensor1.DistancePreCircle * Sensor1.externalTriggerParameter.TriggerDivider);
                log.Info("Sensor Zmap Resolution: Vertical Resolution:"+Sensor1.ZmapResolution.VerticalResolution.ToString()+", Laterval Resolution:"+Sensor1.ZmapResolution.LatervalResolution);
                var s = await Sensor1.StartAcquisition();
                if (Sensor1.SensorModel!=null) 
                {
                    if (Sensor1.SensorModel.Contains("ECCO X"))//binning mode just support the ECCO X series sensors
                    {
                        log.Info("HorizentalBinning:" + Sensor1.HorizentalBinningMode);
                        log.Info("VerticalBinning:" + Sensor1.VerticalBinningMode);
                    }
                }
                // var s1 = await Sensor1.WriteIO(DigitalOutput.Channel2);

            }
            else if (!isStarted)
            {
                log.Info("Sensor stop acquisition");
                var s1 = await Sensor1.StopAcquisition();
                Sensor1.Clearbuffer();
            }

            tbx_NumberOfProfile.Enabled = !isStarted;
            tBx_PacketSize.Enabled = !isStarted;
            tBx_PacketTimeout.Enabled = !isStarted;
            ckb_EnableHorizentalBinning.Enabled = !isStarted;
            ckb_EnableVerticalBinning.Enabled = !isStarted;
            ckb_XEnahancement.Enabled = !isStarted;

            //set the button color
            btn_StartAcquisition.BackColor = isStarted ? Color.Green : Color.Red;
            btn_StartAcquisition.Text = isStarted ? "Sensor running" : "Sensor Stop";
            btn_LoadConfig.Enabled = !isStarted;
            comboBox_ImageType.Enabled = !isStarted;
        }

        private void btn_SimulateTrigger_Click(object sender, EventArgs e)
        {
            Sensor1.WriteIO(DigitalOutput.Channel2);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.IsSensorConnected)
            {
                Sensor1.NumberOfProfileToCapture = uint.Parse(tbx_NumberOfProfile.Text);
            }
        }

        private void tBx_PacketSize_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.IsSensorConnected)
            {
                Sensor1.PackSize = uint.Parse(tBx_PacketSize.Text);
            }
        }

        private void tBx_PacketTimeout_TextChanged(object sender, EventArgs e)
        {
            if (Sensor1.IsSensorConnected)
            {
                Sensor1.PacketTimeout = TimeSpan.Parse(tBx_PacketTimeout.Text);
            }
        }

        private void btn_LoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Please select the sensor:" + Sensor1.SensorModel + " configuration file:",
                Filter = "*.json|*.json|*.par|*.par|All files(*.*)|*.*",
                Multiselect = false,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Sensor1.ConfigFilePath = openFileDialog.FileName;
                tbx_ConfigFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btn_ChooseSaveFilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog mSaveFileDialog = new FolderBrowserDialog
            {
                Description = "please select your save file path:",
                ShowNewFolderButton = true,

            };
            if (mSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbx_SaveImageFilePath.Text = mSaveFileDialog.SelectedPath;
            }
        }

        private void comboBox_ImageType_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex == 0)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.LiveImage;
            }
            else if (comboBox.SelectedIndex == 1)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.Profile;
            }
            else if (comboBox.SelectedIndex == 2)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.ProfileIntensityLaserLineThickness;
            }
            else if (comboBox.SelectedIndex == 3)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.ZMap;
            }
            else if (comboBox.SelectedIndex == 4)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.ZMapIntensityLaserLineThickness;
            }
            else if (comboBox.SelectedIndex == 5)
            {
                Sensor1.AcquisitionType = ImageAcquisitionType.PointCloud;
            }

        }

        private void ckb_EnableHorizentalBinning_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Name == "ckb_EnableHorizentalBinning")
            {
                Sensor1.HorizentalBinningMode = ckb_EnableHorizentalBinning.Checked ? BinningMode.X2 : BinningMode.Off;
            }
            else if ((sender as CheckBox).Name == "ckb_EnableVerticalBinning")
            {
                Sensor1.VerticalBinningMode = ckb_EnableVerticalBinning.Checked ? BinningMode.X2 : BinningMode.Off;
            }
            else if ((sender as CheckBox).Name == "ckb_XEnahancement")
            {
                Sensor1.XEhancement = ckb_EnableVerticalBinning.Checked;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Sensor1!=null & Sensor1.IsSensorConnected)
            {
                Sensor1.Dispose();
                Sensor1 = null;
                ApiManager.CleanUp();
            }
        }
    }
}
