
namespace APIX_Winform_Demo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbx_SaveImage = new System.Windows.Forms.GroupBox();
            this.ckb_EnableSaveFiles = new System.Windows.Forms.CheckBox();
            this.btn_ChooseSaveFilePath = new System.Windows.Forms.Button();
            this.tbx_SaveImageFilePath = new System.Windows.Forms.TextBox();
            this.gbx_binning = new System.Windows.Forms.GroupBox();
            this.ckb_EnableVerticalBinning = new System.Windows.Forms.CheckBox();
            this.ckb_EnableHorizentalBinning = new System.Windows.Forms.CheckBox();
            this.gbx_SensorInfo = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbx_FirmwareVersion = new System.Windows.Forms.TextBox();
            this.tbx_SensorModel = new System.Windows.Forms.TextBox();
            this.tbx_APIXVersion = new System.Windows.Forms.TextBox();
            this.tbx_SensorTempetature = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbx_StartAcquisition = new System.Windows.Forms.GroupBox();
            this.comboBox_ImageType = new System.Windows.Forms.ComboBox();
            this.btn_SimulateTrigger = new System.Windows.Forms.Button();
            this.btn_StartAcquisition = new System.Windows.Forms.Button();
            this.gbx_SensorPar = new System.Windows.Forms.GroupBox();
            this.btn_LoadConfig = new System.Windows.Forms.Button();
            this.btn_SaveFilePath = new System.Windows.Forms.Button();
            this.tbx_ConfigFilePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBx_PacketTimeout = new System.Windows.Forms.TextBox();
            this.tBx_PacketSize = new System.Windows.Forms.TextBox();
            this.tbx_NumberOfProfile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_InitialSensor = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.smartRayApiExceptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cv_imageBox1 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbx_SaveImage.SuspendLayout();
            this.gbx_binning.SuspendLayout();
            this.gbx_SensorInfo.SuspendLayout();
            this.gbx_StartAcquisition.SuspendLayout();
            this.gbx_SensorPar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smartRayApiExceptionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbx_SaveImage);
            this.splitContainer1.Panel2.Controls.Add(this.gbx_binning);
            this.splitContainer1.Panel2.Controls.Add(this.gbx_SensorInfo);
            this.splitContainer1.Panel2.Controls.Add(this.gbx_StartAcquisition);
            this.splitContainer1.Panel2.Controls.Add(this.gbx_SensorPar);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar1);
            this.splitContainer1.Size = new System.Drawing.Size(1350, 729);
            this.splitContainer1.SplitterDistance = 989;
            this.splitContainer1.TabIndex = 0;
            // 
            // gbx_SaveImage
            // 
            this.gbx_SaveImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_SaveImage.Controls.Add(this.ckb_EnableSaveFiles);
            this.gbx_SaveImage.Controls.Add(this.btn_ChooseSaveFilePath);
            this.gbx_SaveImage.Controls.Add(this.tbx_SaveImageFilePath);
            this.gbx_SaveImage.Location = new System.Drawing.Point(4, 513);
            this.gbx_SaveImage.Name = "gbx_SaveImage";
            this.gbx_SaveImage.Size = new System.Drawing.Size(323, 80);
            this.gbx_SaveImage.TabIndex = 7;
            this.gbx_SaveImage.TabStop = false;
            this.gbx_SaveImage.Text = "Save Images";
            // 
            // ckb_EnableSaveFiles
            // 
            this.ckb_EnableSaveFiles.AutoSize = true;
            this.ckb_EnableSaveFiles.Location = new System.Drawing.Point(7, 20);
            this.ckb_EnableSaveFiles.Name = "ckb_EnableSaveFiles";
            this.ckb_EnableSaveFiles.Size = new System.Drawing.Size(126, 16);
            this.ckb_EnableSaveFiles.TabIndex = 9;
            this.ckb_EnableSaveFiles.Text = "Enable Save Files";
            this.ckb_EnableSaveFiles.UseVisualStyleBackColor = true;
            // 
            // btn_ChooseSaveFilePath
            // 
            this.btn_ChooseSaveFilePath.Location = new System.Drawing.Point(280, 46);
            this.btn_ChooseSaveFilePath.Name = "btn_ChooseSaveFilePath";
            this.btn_ChooseSaveFilePath.Size = new System.Drawing.Size(38, 23);
            this.btn_ChooseSaveFilePath.TabIndex = 8;
            this.btn_ChooseSaveFilePath.Text = "..";
            this.btn_ChooseSaveFilePath.UseVisualStyleBackColor = true;
            this.btn_ChooseSaveFilePath.Click += new System.EventHandler(this.btn_ChooseSaveFilePath_Click);
            // 
            // tbx_SaveImageFilePath
            // 
            this.tbx_SaveImageFilePath.Location = new System.Drawing.Point(7, 47);
            this.tbx_SaveImageFilePath.Name = "tbx_SaveImageFilePath";
            this.tbx_SaveImageFilePath.Size = new System.Drawing.Size(267, 21);
            this.tbx_SaveImageFilePath.TabIndex = 0;
            // 
            // gbx_binning
            // 
            this.gbx_binning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_binning.Controls.Add(this.ckb_EnableVerticalBinning);
            this.gbx_binning.Controls.Add(this.ckb_EnableHorizentalBinning);
            this.gbx_binning.Location = new System.Drawing.Point(4, 431);
            this.gbx_binning.Name = "gbx_binning";
            this.gbx_binning.Size = new System.Drawing.Size(324, 75);
            this.gbx_binning.TabIndex = 6;
            this.gbx_binning.TabStop = false;
            this.gbx_binning.Text = "Binning Mode[ECCO X Series:]:";
            // 
            // ckb_EnableVerticalBinning
            // 
            this.ckb_EnableVerticalBinning.AutoSize = true;
            this.ckb_EnableVerticalBinning.Location = new System.Drawing.Point(11, 43);
            this.ckb_EnableVerticalBinning.Name = "ckb_EnableVerticalBinning";
            this.ckb_EnableVerticalBinning.Size = new System.Drawing.Size(162, 16);
            this.ckb_EnableVerticalBinning.TabIndex = 0;
            this.ckb_EnableVerticalBinning.Text = "Enable Vertical Binning";
            this.ckb_EnableVerticalBinning.UseVisualStyleBackColor = true;
            // 
            // ckb_EnableHorizentalBinning
            // 
            this.ckb_EnableHorizentalBinning.AutoSize = true;
            this.ckb_EnableHorizentalBinning.Location = new System.Drawing.Point(11, 21);
            this.ckb_EnableHorizentalBinning.Name = "ckb_EnableHorizentalBinning";
            this.ckb_EnableHorizentalBinning.Size = new System.Drawing.Size(174, 16);
            this.ckb_EnableHorizentalBinning.TabIndex = 0;
            this.ckb_EnableHorizentalBinning.Text = "Enable Horizental Binning";
            this.ckb_EnableHorizentalBinning.UseVisualStyleBackColor = true;
            // 
            // gbx_SensorInfo
            // 
            this.gbx_SensorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_SensorInfo.Controls.Add(this.label9);
            this.gbx_SensorInfo.Controls.Add(this.label8);
            this.gbx_SensorInfo.Controls.Add(this.label7);
            this.gbx_SensorInfo.Controls.Add(this.tbx_FirmwareVersion);
            this.gbx_SensorInfo.Controls.Add(this.tbx_SensorModel);
            this.gbx_SensorInfo.Controls.Add(this.tbx_APIXVersion);
            this.gbx_SensorInfo.Controls.Add(this.tbx_SensorTempetature);
            this.gbx_SensorInfo.Controls.Add(this.label5);
            this.gbx_SensorInfo.Location = new System.Drawing.Point(4, 306);
            this.gbx_SensorInfo.Name = "gbx_SensorInfo";
            this.gbx_SensorInfo.Size = new System.Drawing.Size(324, 119);
            this.gbx_SensorInfo.TabIndex = 5;
            this.gbx_SensorInfo.TabStop = false;
            this.gbx_SensorInfo.Text = "Sensor Info";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "Firmware Version:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "Sensor Model:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "APIX Version:";
            // 
            // tbx_FirmwareVersion
            // 
            this.tbx_FirmwareVersion.Enabled = false;
            this.tbx_FirmwareVersion.Location = new System.Drawing.Point(135, 88);
            this.tbx_FirmwareVersion.Name = "tbx_FirmwareVersion";
            this.tbx_FirmwareVersion.Size = new System.Drawing.Size(183, 21);
            this.tbx_FirmwareVersion.TabIndex = 1;
            // 
            // tbx_SensorModel
            // 
            this.tbx_SensorModel.Enabled = false;
            this.tbx_SensorModel.Location = new System.Drawing.Point(135, 65);
            this.tbx_SensorModel.Name = "tbx_SensorModel";
            this.tbx_SensorModel.Size = new System.Drawing.Size(183, 21);
            this.tbx_SensorModel.TabIndex = 1;
            // 
            // tbx_APIXVersion
            // 
            this.tbx_APIXVersion.Enabled = false;
            this.tbx_APIXVersion.Location = new System.Drawing.Point(135, 42);
            this.tbx_APIXVersion.Name = "tbx_APIXVersion";
            this.tbx_APIXVersion.Size = new System.Drawing.Size(183, 21);
            this.tbx_APIXVersion.TabIndex = 1;
            // 
            // tbx_SensorTempetature
            // 
            this.tbx_SensorTempetature.Enabled = false;
            this.tbx_SensorTempetature.Location = new System.Drawing.Point(135, 18);
            this.tbx_SensorTempetature.Name = "tbx_SensorTempetature";
            this.tbx_SensorTempetature.Size = new System.Drawing.Size(183, 21);
            this.tbx_SensorTempetature.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sensor Temperature:";
            // 
            // gbx_StartAcquisition
            // 
            this.gbx_StartAcquisition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_StartAcquisition.Controls.Add(this.comboBox_ImageType);
            this.gbx_StartAcquisition.Controls.Add(this.btn_SimulateTrigger);
            this.gbx_StartAcquisition.Controls.Add(this.btn_StartAcquisition);
            this.gbx_StartAcquisition.Location = new System.Drawing.Point(4, 599);
            this.gbx_StartAcquisition.Name = "gbx_StartAcquisition";
            this.gbx_StartAcquisition.Size = new System.Drawing.Size(324, 125);
            this.gbx_StartAcquisition.TabIndex = 3;
            this.gbx_StartAcquisition.TabStop = false;
            this.gbx_StartAcquisition.Text = "Initial sensor";
            // 
            // comboBox_ImageType
            // 
            this.comboBox_ImageType.FormattingEnabled = true;
            this.comboBox_ImageType.Items.AddRange(new object[] {
            "Live",
            "Profile",
            "Profile & Intensity & LaserLineThickness",
            "Zmap",
            "Zmap & Intensity & LaserLineThickness",
            "PointCloud"});
            this.comboBox_ImageType.Location = new System.Drawing.Point(8, 20);
            this.comboBox_ImageType.Name = "comboBox_ImageType";
            this.comboBox_ImageType.Size = new System.Drawing.Size(301, 20);
            this.comboBox_ImageType.TabIndex = 1;
            this.comboBox_ImageType.SelectedValueChanged += new System.EventHandler(this.comboBox_ImageType_SelectedValueChanged);
            // 
            // btn_SimulateTrigger
            // 
            this.btn_SimulateTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SimulateTrigger.Location = new System.Drawing.Point(6, 86);
            this.btn_SimulateTrigger.Name = "btn_SimulateTrigger";
            this.btn_SimulateTrigger.Size = new System.Drawing.Size(306, 34);
            this.btn_SimulateTrigger.TabIndex = 0;
            this.btn_SimulateTrigger.Text = "Simulate Start Trigger(Outpu2-->Input1)";
            this.btn_SimulateTrigger.UseVisualStyleBackColor = true;
            this.btn_SimulateTrigger.Click += new System.EventHandler(this.btn_SimulateTrigger_Click);
            // 
            // btn_StartAcquisition
            // 
            this.btn_StartAcquisition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_StartAcquisition.Location = new System.Drawing.Point(6, 46);
            this.btn_StartAcquisition.Name = "btn_StartAcquisition";
            this.btn_StartAcquisition.Size = new System.Drawing.Size(306, 34);
            this.btn_StartAcquisition.TabIndex = 0;
            this.btn_StartAcquisition.Text = "Start Acquisition";
            this.btn_StartAcquisition.UseVisualStyleBackColor = true;
            this.btn_StartAcquisition.Click += new System.EventHandler(this.btn_StartAcquisition_Click);
            // 
            // gbx_SensorPar
            // 
            this.gbx_SensorPar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_SensorPar.Controls.Add(this.btn_LoadConfig);
            this.gbx_SensorPar.Controls.Add(this.btn_SaveFilePath);
            this.gbx_SensorPar.Controls.Add(this.tbx_ConfigFilePath);
            this.gbx_SensorPar.Controls.Add(this.label6);
            this.gbx_SensorPar.Controls.Add(this.label4);
            this.gbx_SensorPar.Controls.Add(this.label3);
            this.gbx_SensorPar.Controls.Add(this.label2);
            this.gbx_SensorPar.Controls.Add(this.tBx_PacketTimeout);
            this.gbx_SensorPar.Controls.Add(this.tBx_PacketSize);
            this.gbx_SensorPar.Controls.Add(this.tbx_NumberOfProfile);
            this.gbx_SensorPar.Controls.Add(this.label1);
            this.gbx_SensorPar.Location = new System.Drawing.Point(4, 93);
            this.gbx_SensorPar.Name = "gbx_SensorPar";
            this.gbx_SensorPar.Size = new System.Drawing.Size(330, 218);
            this.gbx_SensorPar.TabIndex = 2;
            this.gbx_SensorPar.TabStop = false;
            this.gbx_SensorPar.Text = "Sensor Parameters";
            // 
            // btn_LoadConfig
            // 
            this.btn_LoadConfig.Location = new System.Drawing.Point(286, 150);
            this.btn_LoadConfig.Name = "btn_LoadConfig";
            this.btn_LoadConfig.Size = new System.Drawing.Size(38, 23);
            this.btn_LoadConfig.TabIndex = 7;
            this.btn_LoadConfig.Text = "..";
            this.btn_LoadConfig.UseVisualStyleBackColor = true;
            this.btn_LoadConfig.Click += new System.EventHandler(this.btn_LoadConfig_Click);
            // 
            // btn_SaveFilePath
            // 
            this.btn_SaveFilePath.Location = new System.Drawing.Point(6, 178);
            this.btn_SaveFilePath.Name = "btn_SaveFilePath";
            this.btn_SaveFilePath.Size = new System.Drawing.Size(318, 23);
            this.btn_SaveFilePath.TabIndex = 4;
            this.btn_SaveFilePath.Text = "Save Configration File";
            this.btn_SaveFilePath.UseVisualStyleBackColor = true;
            this.btn_SaveFilePath.Click += new System.EventHandler(this.btn_SaveConfigrationFile_Click);
            // 
            // tbx_ConfigFilePath
            // 
            this.tbx_ConfigFilePath.Location = new System.Drawing.Point(6, 151);
            this.tbx_ConfigFilePath.Name = "tbx_ConfigFilePath";
            this.tbx_ConfigFilePath.Size = new System.Drawing.Size(273, 21);
            this.tbx_ConfigFilePath.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Configration FilePath:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "ms";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pack timeout:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Packsize:";
            // 
            // tBx_PacketTimeout
            // 
            this.tBx_PacketTimeout.Location = new System.Drawing.Point(96, 100);
            this.tBx_PacketTimeout.Name = "tBx_PacketTimeout";
            this.tBx_PacketTimeout.Size = new System.Drawing.Size(160, 21);
            this.tBx_PacketTimeout.TabIndex = 1;
            this.tBx_PacketTimeout.TextChanged += new System.EventHandler(this.tBx_PacketTimeout_TextChanged);
            // 
            // tBx_PacketSize
            // 
            this.tBx_PacketSize.Location = new System.Drawing.Point(71, 65);
            this.tBx_PacketSize.Name = "tBx_PacketSize";
            this.tBx_PacketSize.Size = new System.Drawing.Size(253, 21);
            this.tBx_PacketSize.TabIndex = 1;
            this.tBx_PacketSize.TextChanged += new System.EventHandler(this.tBx_PacketSize_TextChanged);
            // 
            // tbx_NumberOfProfile
            // 
            this.tbx_NumberOfProfile.Location = new System.Drawing.Point(7, 37);
            this.tbx_NumberOfProfile.Name = "tbx_NumberOfProfile";
            this.tbx_NumberOfProfile.Size = new System.Drawing.Size(317, 21);
            this.tbx_NumberOfProfile.TabIndex = 1;
            this.tbx_NumberOfProfile.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Profles to capture:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btn_InitialSensor);
            this.groupBox1.Location = new System.Drawing.Point(4, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initial sensor";
            // 
            // btn_InitialSensor
            // 
            this.btn_InitialSensor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InitialSensor.Location = new System.Drawing.Point(7, 21);
            this.btn_InitialSensor.Name = "btn_InitialSensor";
            this.btn_InitialSensor.Size = new System.Drawing.Size(317, 34);
            this.btn_InitialSensor.TabIndex = 0;
            this.btn_InitialSensor.Text = "Initial sensor";
            this.btn_InitialSensor.UseVisualStyleBackColor = true;
            this.btn_InitialSensor.Click += new System.EventHandler(this.btn_InitialSensor_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(337, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 729);
            this.vScrollBar1.TabIndex = 0;
            // 
            // smartRayApiExceptionBindingSource
            // 
            this.smartRayApiExceptionBindingSource.DataSource = typeof(SmartRay.Api.SmartRayApiException);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cv_imageBox1);
            this.splitContainer2.Size = new System.Drawing.Size(989, 729);
            this.splitContainer2.SplitterDistance = 572;
            this.splitContainer2.TabIndex = 0;
            // 
            // cv_imageBox1
            // 
            this.cv_imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_imageBox1.Location = new System.Drawing.Point(0, 0);
            this.cv_imageBox1.Name = "cv_imageBox1";
            this.cv_imageBox1.Size = new System.Drawing.Size(572, 729);
            this.cv_imageBox1.TabIndex = 2;
            this.cv_imageBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "APIX-Winform";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbx_SaveImage.ResumeLayout(false);
            this.gbx_SaveImage.PerformLayout();
            this.gbx_binning.ResumeLayout(false);
            this.gbx_binning.PerformLayout();
            this.gbx_SensorInfo.ResumeLayout(false);
            this.gbx_SensorInfo.PerformLayout();
            this.gbx_StartAcquisition.ResumeLayout(false);
            this.gbx_SensorPar.ResumeLayout(false);
            this.gbx_SensorPar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.smartRayApiExceptionBindingSource)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_imageBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_InitialSensor;
        private System.Windows.Forms.GroupBox gbx_SensorPar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_NumberOfProfile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBx_PacketSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBx_PacketTimeout;
        private System.Windows.Forms.GroupBox gbx_StartAcquisition;
        private System.Windows.Forms.Button btn_StartAcquisition;
        private System.Windows.Forms.Button btn_SimulateTrigger;
        private System.Windows.Forms.Button btn_SaveFilePath;
        private System.Windows.Forms.GroupBox gbx_SensorInfo;
        private System.Windows.Forms.TextBox tbx_SensorTempetature;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbx_ConfigFilePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_LoadConfig;
        private System.Windows.Forms.GroupBox gbx_binning;
        private System.Windows.Forms.CheckBox ckb_EnableHorizentalBinning;
        private System.Windows.Forms.CheckBox ckb_EnableVerticalBinning;
        private System.Windows.Forms.GroupBox gbx_SaveImage;
        private System.Windows.Forms.Button btn_ChooseSaveFilePath;
        private System.Windows.Forms.TextBox tbx_SaveImageFilePath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbx_FirmwareVersion;
        private System.Windows.Forms.TextBox tbx_SensorModel;
        private System.Windows.Forms.TextBox tbx_APIXVersion;
        private System.Windows.Forms.ComboBox comboBox_ImageType;
        private System.Windows.Forms.BindingSource smartRayApiExceptionBindingSource;
        private System.Windows.Forms.CheckBox ckb_EnableSaveFiles;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Emgu.CV.UI.ImageBox cv_imageBox1;
    }
}

