using Cognex.VisionPro;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using KimLib;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VISION.Class;

namespace VISION
{
    public partial class Frm_ToolSetUp : Form
    {
        private Class_Common cm { get { return Program.cm; } }

        //INIControl setting = new INIControl(PGgloble.getInstance.SETTING);
        internal MultiPatternResult frm_MultiPatternResult; //캘리브레이션 화면
        private Frm_Main Main;
        private PGgloble Glob;
        double[] gainvalue;
        double[] exposurevalue;

        private CogImage8Grey Monoimage = new CogImage8Grey();
        private CogImage8Grey Fiximage;
        private string FimageSpace;

        private string[,] TempDistance_Tool1_Number;
        private string[,] TempDistance_Tool2_Number;

        private bool Dataset = false;
        public bool liveflag = false;
        bool FormLoad = false;
        bool ImageDelete = false;
        int SelectNumber;

        public Frm_ToolSetUp(Frm_Main main)
        {
            InitializeComponent();
            Glob = PGgloble.getInstance;
            gainvalue = new double[Glob.CamCount];
            exposurevalue = new double[Glob.CamCount];
            btn_Livestop.Enabled = false;
            Dataset = true;
            Main = main;
            TempDistance_Tool1_Number = Glob.코그넥스파일.모델.Distance_UseTool1_Numbers();
            TempDistance_Tool2_Number = Glob.코그넥스파일.모델.Distance_UseTool2_Numbers();
            string[] Polarty = { "White to Black", "Black to  White", "Don't Care" };
            string[] Blob = { "White Blob", "Black Blob" };
            string[] Direction = { "Inward", "Outward" };
            string[] AreaShape = { "CogCircle", "CogEllipse", "CogRectangleAffine", "CogCircularAnnulusSection" };
            cb_BlobPolarty.Items.AddRange(Blob);
            for (int i = 0; i < 30; i++)
            {
                cb_MultiPatternName.Items.Add(Glob.코그넥스파일.패턴툴[Glob.CamNumber, i].ToolName());
                Num1_DimensionTool.Items.Add(Glob.코그넥스파일.라인툴[Glob.CamNumber, i].ToolName());
                Num2_DimensionTool.Items.Add(Glob.코그넥스파일.라인툴[Glob.CamNumber, i].ToolName());
            }
            for (int i = 0; i < 9; i++)
            {
                Num1_DimensionTool.Items.Add(Glob.코그넥스파일.써클툴[Glob.CamNumber, i].ToolName());
                Num2_DimensionTool.Items.Add(Glob.코그넥스파일.써클툴[Glob.CamNumber, i].ToolName());
            }
            for (int i = 0; i < 9; i++)
            {
                Num1_DimensionTool.Items.Add(Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, i].ToolName());
                Num2_DimensionTool.Items.Add(Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, i].ToolName());
            }
            num_BlobToolNum.Value = 0;
            num_MultiPatternToolNumber.Value = 0;
            num_DimensionToolNum.Value = 1;

            ChangeBlobToolNumber();
            ChangeMultiPatternToolNumber();
            ChangeDistanceToolNumber();
            lb_CurruntModelName.Text = Glob.RunnModel.Modelname(); //현재사용중인 모델명 체크

            Dataset = false;
        }

        private void Frm_ToolSetUp_Load(object sender, EventArgs e)
        {
            FormLoad = true;
            Glob.CamNumber = 0;
            Glob.InspectOrder = 1;
            LoadSetup();
            UpdateCamStats();
            UpdateCameraSet();
            dgv_ToolSetUp.DoubleBuffered(true);
            DGVUpadte();
            Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }

        private void DGVUpadte()
        {
            dgv_ToolSetUp.Rows.Clear();
            for (int i = 0; i < 30; i++)
            {
                dgv_ToolSetUp.Rows.Add(Glob.RunnModel.MultiPatterns()[Glob.CamNumber, i].ToolName());
                dgv_ToolSetUp.Rows[i].Cells[2].Value = Glob.RunnModel.Blob()[Glob.CamNumber, i].ToolName();
            }
        }
        private void LoadSetup()
        {
            try
            {
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");
                INIControl CalibrationValue = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CalibrationValue.ini");
                num_Exposure.Value = Convert.ToDecimal(CamSet.ReadData($"Camera{Glob.CamNumber}", "Exposure"));
                num_Gain.Value = Convert.ToDecimal(CamSet.ReadData($"Camera{Glob.CamNumber}", "Gain"));
                Dataset = true;
                num_LightControlNumber.Value = 0;
                num_LightCH1.Value = Glob.LightChAndValue[0, 0];
                num_LightCH2.Value = Glob.LightChAndValue[0, 1];
                Dataset = false;
                for (int i = 0; i < Glob.CamCount; i++)
                {
                    if (CamSet.ReadData($"Camera{i}", "Exposure") == "") CamSet.WriteData($"Camera{i}", "Exposure", "0.1");
                    if (CamSet.ReadData($"Camera{i}", "Gain") == "") CamSet.WriteData($"Camera{i}", "Gain", "0");

                    gainvalue[i] = Convert.ToDouble(CamSet.ReadData($"Camera{i}", "Exposure"));
                    exposurevalue[i] = Convert.ToDouble(CamSet.ReadData($"Camera{i}", "Gain"));
                }
                FormLoad = false;
            }
            catch (Exception ee)
            {
                Main.log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {ee.Message}");
                cm.info(ee.Message);
            }
        }
        private void btn_ImageOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ImageDelete = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //Glob.ImageFilePath = ofd.FileName.Substring(0, ofd.FileName.Length - ofd.SafeFileName.Length);
                    Glob.ImageFilePath = ofd.FileName;
                    string type = Path.GetExtension(ofd.FileName);
                    string[] ImageFileName = ofd.FileNames;
                    if (type == ".bmp")
                    {
                        CogImageFileBMP Imageopen = new CogImageFileBMP();
                        Imageopen.Open(ofd.FileName, CogImageFileModeConstants.Read);
                        Imageopen.Close();
                    }
                    else
                    {
                        CogImageFileJPEG Imageopen2 = new CogImageFileJPEG();
                        Imageopen2.Open(ofd.FileName, CogImageFileModeConstants.Read);
                        CogImageConvertTool ImageConvert = new CogImageConvertTool();
                        ImageConvert.InputImage = Imageopen2[0];
                        ImageConvert.RunParams.RunMode = CogImageConvertRunModeConstants.Plane2;
                        Monoimage = (CogImage8Grey)ImageConvert.OutputImage;
                        Imageopen2.Close();
                    }
                    for (int i = 0; i < ImageFileName.Count(); i++)
                    {
                        ImageList.Items.Add(ImageFileName[i]);
                    }
                    ImageList.SelectedIndex = ImageList.Items.Count - 1;
                    cdyDisplay.InteractiveGraphics.Clear();
                    cdyDisplay.StaticGraphics.Clear();
                }
            }
            catch (Exception ee)
            {
                Main.log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {ee.Message}");
                cm.info(ee.Message);
            }
        }

        private void 검사설정창종료_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("셋팅창을 종료 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            try
            {
                for (int i = 0; i < Glob.CamCount; i++)
                {
                    Glob.RunnModel.Loadmodel(Glob.RunnModel.Modelname(), Glob.MODELROOT, i);
                }

                if (Main.frm_toolsetup != null)
                {
                    Main.frm_toolsetup = null;
                }
                Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                GC.Collect();
                Dispose();
                Close();
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
                GC.Collect();
                Dispose();
                Close();
            }
        }

        private void BlobEnableChange(int toolnumber)
        {
            cb_BlobToolUsed.Text = cb_BlobToolUsed.Checked == true ? "USE" : "UNUSED";
            cb_BlobToolUsed.ForeColor = cb_BlobToolUsed.Checked == true ? Color.Lime : Color.Red;
        }

        private void BlobNGOKChangeChange(int toolnumber)
        {
            cb_ngokchange.Text = cb_ngokchange.Checked == true ? "USE" : "UNUSED";
            cb_ngokchange.ForeColor = cb_ngokchange.Checked == true ? Color.Lime : Color.Red;
        }

        public void Pattern_Train()
        {
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].Run((CogImage8Grey)cdyDisplay.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ToolName(), Glob.CamNumber, out FimageSpace, usePatternNumber, Glob.InspectOrder);
            }
        }

        public void Mask_Train(int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].Run((CogImage8Grey)cdyDisplay.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Mask_FixtureImage1((CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].ToolName(), Glob.CamNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        public void Bolb_Train(int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].Run((CogImage8Grey)cdyDisplay.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage((CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnumber].ToolName(), Glob.CamNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        private void ImageClear()
        {
            cdyDisplay.StaticGraphics.Clear();
            cdyDisplay.InteractiveGraphics.Clear();
        }

        private void 검사툴저장_Click(object sender, EventArgs e)
        {
            INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");
            INIControl CalibrationValue = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CalibrationValue.ini");
            if (MessageBox.Show("셋팅값을 저장 하시겠습니까?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            Glob.G_MainForm.SubFromStart("툴 설정값을 저장중 입니다.", Glob.PROGRAM_VERSION);

            Glob.RunnModel.Line(Glob.코그넥스파일.라인툴); //라인툴저장
            Glob.RunnModel.LineEnables(Glob.코그넥스파일.라인툴사용여부); //라인툴사용여부
            Glob.RunnModel.Blob(Glob.코그넥스파일.블롭툴); //블롭툴저장
            Glob.RunnModel.BlobNGOKChanges(Glob.코그넥스파일.블롭툴역검사);
            Glob.RunnModel.BlobEnables(Glob.코그넥스파일.블롭툴사용여부); //블롭툴사용여부
            Glob.RunnModel.BlobOKCounts(Glob.코그넥스파일.블롭툴양품갯수); //블롭카운트
            Glob.RunnModel.BlobFixPatternNumbers(Glob.코그넥스파일.블롭툴픽스쳐번호);
            Glob.RunnModel.MultiPatterns(Glob.코그넥스파일.패턴툴); // 멀티패턴툴저장
            Glob.RunnModel.MultiPatternEnables(Glob.코그넥스파일.패턴툴사용여부); // 멀티패턴툴사용여부젖장
            Glob.RunnModel.Distancess(Glob.코그넥스파일.거리측정툴);
            Glob.RunnModel.DistanceEnables(Glob.코그넥스파일.거리측정툴사용여부);
            Glob.RunnModel.Distance_UseTool1_Numbers(TempDistance_Tool1_Number);
            Glob.RunnModel.Distance_UseTool2_Numbers(TempDistance_Tool2_Number);
            Glob.RunnModel.Cams(Glob.코그넥스파일.카메라);
            Glob.RunnModel.MaskTools(Glob.코그넥스파일.마스크툴);

            Glob.RunnModel.SaveModel(Glob.MODELROOT + "\\" + Glob.RunnModel.Modelname() + "\\" + $"Cam{Glob.CamNumber}", Glob.CamNumber); //모델명
            CamSet.WriteData($"Camera{Glob.CamNumber}", "Exposure", num_Exposure.Value.ToString()); //카메라 노출값
            CamSet.WriteData($"Camera{Glob.CamNumber}", "Gain", num_Gain.Value.ToString()); //카메라 Gain값
            //CamSet.WriteData($"LightControl_Cam{Glob.CamNumber}", "CH1", Glob.LightCH1); //카메라별 조명값 1
            //CamSet.WriteData($"LightControl_Cam{Glob.CamNumber}", "CH2", Glob.LightCH2); //카메라별 조명값 2

            for (int lop = 0; lop < Main.LightControl.Count(); lop++)
            {
                CamSet.WriteData($"LightControl{lop}", "CH1", Glob.LightChAndValue[lop, 0].ToString("D4"));
                CamSet.WriteData($"LightControl{lop}", "CH2", Glob.LightChAndValue[lop, 1].ToString("D4"));
            }

            Glob.G_MainForm.SubFromClose();

            Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            MessageBox.Show("저장 완료", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChangeBlobToolNumber()
        {
            Dataset = true;
            int Toolnumber = (int)num_BlobToolNum.Value;

            cb_BlobPolarty.SelectedIndex = Glob.코그넥스파일.블롭툴[Glob.CamNumber, Toolnumber].Polarity();
            cb_MultiPatternName.SelectedIndex = Glob.코그넥스파일.블롭툴[Glob.CamNumber, Toolnumber].SpaceName();
            num_BlobMinipixel.Value = Glob.코그넥스파일.블롭툴[Glob.CamNumber, Toolnumber].Minipixel();
            num_BlobThreshold.Value = Glob.코그넥스파일.블롭툴[Glob.CamNumber, Toolnumber].Threshold();
            num_AreaPoinrNumber.Value = Glob.코그넥스파일.블롭툴[Glob.CamNumber, Toolnumber].PointNumber();
            num_BlobMaxCout.Value = Glob.코그넥스파일.블롭툴양품갯수[Glob.CamNumber, Toolnumber];

            cb_BlobToolUsed.Checked = Glob.코그넥스파일.블롭툴사용여부[Glob.CamNumber, Toolnumber];
            cb_ngokchange.Checked = Glob.코그넥스파일.블롭툴역검사[Glob.CamNumber, Toolnumber];

            BlobEnableChange(Toolnumber);
            BlobNGOKChangeChange(Toolnumber);
            Dataset = false;
        }
        private void btn_ToolRun_Click(object sender, EventArgs e)
        {
            if (cdyDisplay.Image == null)
                return;

            //Glob.코그넥스파일.카메라[Glob.CamNumber].ToolBlockRun();
            bool 검사결과 = false;
            ImageClear();
            Pattern_Train();
            검사결과 = Main.비전검사.Run(cdyDisplay, Glob.CamNumber, Glob.InspectOrder);
            lb_Tool_InspectResult.Text = 검사결과 ? "O K" : "N G";
            lb_Tool_InspectResult.BackColor = 검사결과 ? Color.Lime : Color.Red;

            Invoke(new Action(delegate ()
            {
                Main.DgvResult(dgv_ToolSetUp, Glob.CamNumber, 1); //-추가된함수
            }));
        }

        private void btn_OneShot_Click(object sender, EventArgs e)
        {
            try
            {
                Glob.G_MainForm.조명온오프제어(true);
                cdyDisplay.Image = null;
                cdyDisplay.InteractiveGraphics.Clear();
                cdyDisplay.StaticGraphics.Clear();
                Main.SnapShot(Glob.CamNumber, cdyDisplay);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }

        private void btn_Live_Click(object sender, EventArgs e)
        {
            try
            {
                if (liveflag == false)
                {
                    cdyDisplay.InteractiveGraphics.Clear();
                    cdyDisplay.StaticGraphics.Clear();
                    cdyDisplay.Fit();
                }
                Glob.코그넥스파일.카메라[Glob.CamNumber].StartLive();
                while (true)
                {
                    if (liveflag == false) break;
                    CogImage8Grey image = Glob.코그넥스파일.카메라[Glob.CamNumber].Run();
                    cdyDisplay.Image = image;
                    Application.DoEvents();
                }
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
                return;
            }
        }

        private void num_Exposure_ValueChanged(object sender, EventArgs e)
        {
            if (Glob.CamNumber == 3 || Glob.CamNumber == 4)
            {
                //Glob.코그넥스파일.카메라[Glob.CamNumber].SetExposure((double)num_Exposure.Value);
            }
        }

        private void btn_Livestop_Click(object sender, EventArgs e)
        {
            try
            {
                liveflag = false;
                cdyDisplay.StaticGraphics.Clear();
                cdyDisplay.InteractiveGraphics.Clear();
                btn_OneShot.Enabled = true;
                btn_Live.Enabled = true;
                num_Exposure.Enabled = false;
                num_Gain.Enabled = false;
                btn_Exit.Enabled = true;

            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }

        private void num_Gain_ValueChanged(object sender, EventArgs e)
        {
            if (Glob.CamNumber == 3 || Glob.CamNumber == 4)
            {
                Glob.코그넥스파일.카메라[Glob.CamNumber].SetBrightness((double)num_Gain.Value);
            }
        }

        private void btn_ImageSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                CogImageFileBMP ImageSave = new CogImageFileBMP();
                ImageSave.Open(sfd.FileName + ".bmp", CogImageFileModeConstants.Write);
                ImageSave.Append(cdyDisplay.Image);
                ImageSave.Close();
            }
        }
        private void num_BlobToolNum_ValueChanged(object sender, EventArgs e)
        {
            ChangeBlobToolNumber();
        }

        private void cb_BlobToolUsed_CheckedChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
            {
                Glob.코그넥스파일.블롭툴사용여부[Glob.CamNumber, (int)num_BlobToolNum.Value] = cb_BlobToolUsed.Checked;
                BlobEnableChange((int)num_BlobToolNum.Value);
            }
        }

        private void cb_BlobPolarty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].Polarity(cb_BlobPolarty.SelectedIndex);
        }

        private void num_BlobMinipixel_ValueChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].Minipixel((int)num_BlobMinipixel.Value);
        }

        private void num_BlobThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].Threshold((int)num_BlobThreshold.Value);
        }

        private void btn_BlobInspectionArea_Click(object sender, EventArgs e)
        {
            ImageClear();
            Bolb_Train((int)num_BlobToolNum.Value);
            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].Area_Affine(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, cb_MultiPatternName.SelectedIndex.ToString());
        }

        private void btn_BlobInspection_Click(object sender, EventArgs e)
        {
            ImageClear();
            Bolb_Train((int)num_BlobToolNum.Value);
            CogGraphicCollection Collection = new CogGraphicCollection();

            Glob.코그넥스파일.마스크툴[Glob.CamNumber, Glob.InspectOrder - 1].Run((CogImage8Grey)cdyDisplay.Image); //MaskTool Run

            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].MaskAreaSet(Glob.코그넥스파일.마스크툴[Glob.CamNumber, Glob.InspectOrder - 1].MaskArea()); //검사 제외영역 입력.

            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].Run((CogImage8Grey)cdyDisplay.Image);
            if (Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ResultBlobCount() != Glob.코그넥스파일.블롭툴양품갯수[Glob.CamNumber, (int)num_BlobToolNum.Value]) //BlobTool 실행.
            {
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ResultAllBlobDisplayPLT(Collection, false);
            }
            else
            {
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ResultAllBlobDisplayPLT(Collection, true);
            }

            cdyDisplay.StaticGraphics.AddList(Collection, "");
            tb_BlobCount.Text = Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ResultBlobCount().ToString();
        }

        private void BLOBINSPECTION_DoubleClick_1(object sender, EventArgs e)
        {
            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ToolSetup();
        }

        private void num_BlobMaxCout_ValueChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
                Glob.코그넥스파일.블롭툴양품갯수[Glob.CamNumber, (int)num_BlobToolNum.Value] = (int)num_BlobMaxCout.Value;
        }

        private void ImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ImageDelete == false)
            {
                string ImageType;
                ListBox temp_lbx = (ListBox)sender;
                //string FilePath = Glob.ImageFilePath;
                //string ImageName = FilePath + temp_lbx.SelectedItem.ToString();
                string ImageName = temp_lbx.SelectedItem.ToString();

                cdyDisplay.InteractiveGraphics.Clear();
                cdyDisplay.StaticGraphics.Clear();

                CogImageFileTool curimage = new CogImageFileTool();
                curimage.Operator.Open(ImageName, CogImageFileModeConstants.Read);
                curimage.Run();
                ImageType = curimage.OutputImage.GetType().ToString();
                //CogIPOneImageTool FlipImageTool = (CogIPOneImageTool)CogSerializer.LoadObjectFromFile(Glob.MODELROOT + $"\\FlipImage2.vpp");
                if (ImageType.Contains("CogImage24PlanarColor"))
                {
                    CogImageConvertTool imageconvert = new CogImageConvertTool();
                    imageconvert.InputImage = curimage.OutputImage;
                    imageconvert.RunParams.RunMode = CogImageConvertRunModeConstants.Plane2;
                    imageconvert.Run();
                    cdyDisplay.Image = (CogImage8Grey)imageconvert.OutputImage;
                }
                else
                {
                    cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage; //JPG 파일
                }
                //cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage;
                cdyDisplay.Fit();
                GC.Collect();
            }
            else
            {
                if (ImageList.Items.Count == 0)
                {
                    ImageClear();
                    cdyDisplay.Image = null;
                }
                else
                {
                    ImageDelete = false;
                    ImageList.SelectedIndex = SelectNumber == 0 ? SelectNumber : SelectNumber - 1;
                }
            }
        }

        private void btn_AllClear_Click(object sender, EventArgs e)
        {
            ImageList.Items.Clear();
            ImageClear();
            cdyDisplay.Image = null;
        }

        private void btn_DeleteImage_Click(object sender, EventArgs e)
        {
            if (ImageList.Items.Count == 0)
                return;
            ImageDelete = true;
            SelectNumber = ImageList.SelectedIndex;
            ImageList.Items.RemoveAt(ImageList.SelectedIndex);
        }

        private void Frm_ToolSetUp_KeyDown(object sender, KeyEventArgs e)
        {
            //************************************단축키 모음************************************//
            if (e.Control && e.KeyCode == Keys.S) //ctrl + s : 셋팅값 저장.
                검사툴저장_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.O) //ctrl + o : 이미지 열기. 
                btn_ImageOpen_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.D1) //ctrl + 1 : 1번카메라 화면.
                btn_Cam1.PerformClick();
            if (e.Control && e.KeyCode == Keys.D2) //ctrl + 2 : 2번카메라 화면.
                btn_Cam2.PerformClick();
            if (e.Control && e.KeyCode == Keys.D3) //ctrl + 3 : 3번카메라 화면.
                btn_Cam3.PerformClick();
            if (e.Control && e.KeyCode == Keys.D4) //ctrl + 4 : 4번카메라 화면.
                btn_Cam4.PerformClick();
            if (e.Control && e.KeyCode == Keys.D5) //ctrl + 5 : 5번카메라 화면.
                btn_Cam5.PerformClick();
            if (e.Control && e.KeyCode == Keys.D6) //ctrl + 6 : 6번카메라 화면.
                btn_Cam6.PerformClick();
        }

        private void ImageList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btn_DeleteImage_Click(sender, e);
        }
        private void UpdateCamStats()
        {
            btn_Cam1.BackColor = Glob.CamNumber == 0 ? Color.Lime : Color.Silver;
            btn_Cam2.BackColor = Glob.CamNumber == 1 ? Color.Lime : Color.Silver;
            btn_Cam3.BackColor = Glob.CamNumber == 2 ? Color.Lime : Color.Silver;
            btn_Cam4.BackColor = Glob.CamNumber == 3 ? Color.Lime : Color.Silver;
            btn_Cam5.BackColor = Glob.CamNumber == 4 ? Color.Lime : Color.Silver;
            btn_Cam6.BackColor = Glob.CamNumber == 5 ? Color.Lime : Color.Silver;
            btn_Cam7.BackColor = Glob.CamNumber == 6 ? Color.Lime : Color.Silver;
            btn_Cam8.BackColor = Glob.CamNumber == 7 ? Color.Lime : Color.Silver;
        }
        private void UpdateCameraSet()
        {
            try
            {
                if (Glob.CamNumber == 3 || Glob.CamNumber == 4)
                {
                    카메라설정값보여주기(true);
                    INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");

                    num_Exposure.Value = Convert.ToDecimal(CamSet.ReadData($"Camera{Glob.CamNumber}", "Exposure"));
                    num_Gain.Value = Convert.ToDecimal(CamSet.ReadData($"Camera{Glob.CamNumber}", "Gain"));

                    //Glob.코그넥스파일.카메라[Glob.CamNumber].SetExposure((double)num_Exposure.Value);
                    //Glob.코그넥스파일.카메라[Glob.CamNumber].SetBrightness((double)num_Gain.Value);
                }
                else
                {
                    카메라설정값보여주기(false);
                }

                FormLoad = false;
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }

        private void 카메라설정값보여주기(bool state)
        {
            lbExposure.Visible = state;
            lbGain.Visible = state;
            num_Gain.Visible = state;
            num_Exposure.Visible = state;
        }

        private void ReLoadVisionTool()
        {
            try
            {
                PGgloble gls = PGgloble.getInstance;
                if (gls.RunnModel.Loadmodel(Glob.RunnModel.Modelname(), gls.MODELROOT, Glob.CamNumber) == true)
                {
                    Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                }
                else Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 실패.");
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }
        private void btn_Cam1_Click(object sender, EventArgs e)
        {
            try
            {
                FormLoad = true;
                int job = Convert.ToInt32((sender as Button).Tag); //클리한 버튼의 TAG값 받아오기.
                Glob.CamNumber = Convert.ToInt32(job);

                cdyDisplay.InteractiveGraphics.Clear();
                cdyDisplay.StaticGraphics.Clear();
                cdyDisplay.Image = null;

                UpdateCamStats(); //카메라 상태표시 변경
                UpdateCameraSet(); //카메라 셋팅 변경
                ReLoadVisionTool(); //카메라별로 Vision Tool 이 저장되어있기 때문에 CamNumber가 바뀔때 마다 불러와준다.

                ChangeMultiPatternToolNumber();
                ChangeBlobToolNumber();
                ChangeDistanceToolNumber();
                //LineChangeToolNumber();
                //CircleChangeToolNumber();
                DGVUpadte();
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }

        }

        private void num_AreaPoinrNumber_ValueChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].AreaPointNumber((int)num_AreaPoinrNumber.Value);
        }

        private void label27_DoubleClick(object sender, EventArgs e)
        {
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].InputImage((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ToolSetup();
        }
        private void num_TrainImageNumber_ValueChanged(object sender, EventArgs e)
        {
            int Toolnumber = (int)num_MultiPatternToolNumber.Value;
            cdyMultiPTTrained.Image = Glob.코그넥스파일.패턴툴[Glob.CamNumber, Toolnumber].TrainedImage((int)num_TrainImageNumber.Value);
        }

        private void num_MultiPatternToolNumber_ValueChanged(object sender, EventArgs e)
        {
            ChangeMultiPatternToolNumber();
            CheckMultiPatternStatus();
        }

        private void ChangeMultiPatternToolNumber()
        {
            Dataset = true;
            int Toolnumber = (int)num_MultiPatternToolNumber.Value;
            cb_MultiPatternToolUsed.Checked = Glob.코그넥스파일.패턴툴사용여부[Glob.CamNumber, Toolnumber];
            cdyMultiPTTrained.Image = Glob.코그넥스파일.패턴툴[Glob.CamNumber, Toolnumber].TrainedImage(0);
            num_MultiPatternScore.Value = Convert.ToDecimal(Glob.코그넥스파일.패턴툴[Glob.CamNumber, Toolnumber].Threshold() * 100);
            lb_PatternCount.Text = $"등록된 패턴 수 : {Glob.코그넥스파일.패턴툴[Glob.CamNumber, Toolnumber].PatternCount()}개";
            lb_FindPatternCount.Text = $"찾은 패턴 수 : {Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultCount()}개";
            num_TrainImageNumber.Maximum = Glob.코그넥스파일.패턴툴[Glob.CamNumber, Toolnumber].PatternCount() - 1;
            num_TrainImageNumber.Minimum = 0;
            MultiPatternEnableChange(Toolnumber);
            MultiPatternOrderChange(Toolnumber);
            Dataset = false;
        }
        private void MultiPatternEnableChange(int toolnumber)
        {
            cb_MultiPatternToolUsed.Text = cb_MultiPatternToolUsed.Checked == true ? "USE" : "UNUSED";
            cb_MultiPatternToolUsed.ForeColor = cb_MultiPatternToolUsed.Checked == true ? Color.Lime : Color.Red;
        }
        private void CheckMultiPatternStatus()
        {
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].istrain((int)num_MultiPatternToolNumber.Value) == true)
            {
                cdyMultiPTTrained.Image = Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].TrainedImage(0);
            }
            else
            {
                cdyMultiPTTrained.Image = null;
            }
        }

        private void cb_MultiPatternToolUsed_CheckedChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
            {
                Glob.코그넥스파일.패턴툴사용여부[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value] = cb_MultiPatternToolUsed.Checked;
                MultiPatternEnableChange((int)num_MultiPatternToolNumber.Value);
            }
        }

        private void btn_MultiPatternToolRun_Click(object sender, EventArgs e)
        {
            ImageClear();
            Pattern_Train();
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].Run((CogImage8Grey)cdyDisplay.Image) == true)
            {
                for (int i = 0; i < Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultCount(); i++)
                {
                    Glob.MultiPatternResultData[Glob.CamNumber, i] = Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultScore(i);
                }

                int usePatternNumber = Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].HighestResultToolNumber();

                Glob.MultiInsPat_Result[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value] = Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultScore(usePatternNumber);
                Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultDisplay(ref cdyDisplay, usePatternNumber);
                lb_FindPatternCount.Text = $"찾은 패턴 수 : {Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultCount()}개";
                lb_MultiScore.ForeColor = Color.Lime;
                lb_MultiScore.Text = $"{Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultScore(usePatternNumber).ToString("F2")}%";
            }
            else
            {
                ImageClear();
                lb_MultiScore.Text = "패턴 찾기 실패";
                lb_FindPatternCount.Text = $"찾은 패턴 수 : {Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultCount()}개";
                lb_MultiScore.ForeColor = Color.Red;
                Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultDisplay(ref cdyDisplay, (int)num_MultiPatternToolNumber.Value);
            }
        }

        private void btn_PatternInput_Click(object sender, EventArgs e)
        {
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].InputImage((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ToolSetup();
            ChangeMultiPatternToolNumber();
        }

        private void btn_ResultShow_Click(object sender, EventArgs e)
        {
            frm_MultiPatternResult = new MultiPatternResult(this, Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].ResultCount());
            frm_MultiPatternResult.Show();
        }

        private void num_MultiPatternScore_ValueChanged(object sender, EventArgs e)
        {
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].Threshold((double)num_MultiPatternScore.Value / 100);
        }

        private void btn_MultiPTSearchArea_Click(object sender, EventArgs e)
        {
            ImageClear();
            Pattern_Train();
            Glob.코그넥스파일.패턴툴[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value].SearchArea(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, Glob.CamNumber, (int)num_MultiPatternToolNumber.Value);
        }


        private void cb_MultiPatternName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int toolnum = cb_MultiPatternName.SelectedIndex;
            Glob.코그넥스파일.블롭툴픽스쳐번호[Glob.CamNumber, (int)num_BlobToolNum.Value] = toolnum;
            ImageClear();
            if (Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnum].Run((CogImage8Grey)cdyDisplay.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnum].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage((CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnum].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[Glob.CamNumber, toolnum].ToolName(), Glob.CamNumber, toolnum, out FimageSpace, usePatternNumber);
            }
            else
            {
                //cm.info("이미지가 없습니다.");
            }
        }

        private void label55_DoubleClick(object sender, EventArgs e)
        {
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputImage((int)num_DimensionToolNum.Value, (CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].ToolSetup((int)num_DimensionToolNum.Value);
        }

        private void num_DimensionToolNum_ValueChanged(object sender, EventArgs e)
        {
            ChangeDistanceToolNumber();
        }

        private void Num1_DimensionTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            string toolname = Num1_DimensionTool.SelectedItem.ToString();
            string[] s_toolname = toolname.Split('-');
            TempDistance_Tool1_Number[Glob.CamNumber, (int)num_DimensionToolNum.Value] = toolname;
            switch (s_toolname[0])
            {
                case "Line ":
                    Glob.코그넥스파일.라인툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
                case "Caliper ":
                    Glob.코그넥스파일.캘리퍼툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
                case "Circle ":
                    Glob.코그넥스파일.써클툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
            }
        }
        private void Num2_DimensionTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            string toolname = Num2_DimensionTool.SelectedItem.ToString();
            string[] s_toolname = toolname.Split('-');
            TempDistance_Tool2_Number[Glob.CamNumber, (int)num_DimensionToolNum.Value] = toolname;
            switch (s_toolname[0])
            {
                case "Line ":
                    Glob.코그넥스파일.라인툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
                case "Caliper ":
                    Glob.코그넥스파일.캘리퍼툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
                case "Circle ":
                    Glob.코그넥스파일.써클툴사용여부[Glob.CamNumber, Convert.ToInt32(s_toolname[1])] = true;
                    break;
            }
        }
        private void ChangeDistanceToolNumber()
        {
            Dataset = true;
            int Toolnumber = (int)num_DimensionToolNum.Value;
            Num1_DimensionTool.SelectedItem = TempDistance_Tool1_Number[Glob.CamNumber, Toolnumber];
            Num2_DimensionTool.SelectedItem = TempDistance_Tool2_Number[Glob.CamNumber, Toolnumber];
            tb_DemensionTool_Measure.Text = string.Empty;
            tb_DemensionTool_Pix.Text = string.Empty;
            tb_DemensionTool_Calibration.Text = Glob.코그넥스파일.보정값[Glob.CamNumber, Toolnumber].ToString("F5");
            tb_LowValue.Text = Glob.코그넥스파일.최소값[Glob.CamNumber, Toolnumber].ToString("F3");
            tb_HighValue.Text = Glob.코그넥스파일.최대값[Glob.CamNumber, Toolnumber].ToString("F3");

            cb_DimensionToolUsed.Checked = Glob.코그넥스파일.거리측정툴사용여부[Glob.CamNumber, Toolnumber];
            DistanceEnableChange(Toolnumber);
            Dataset = false;
        }
        private void DistanceEnableChange(int toolnumber)
        {
            cb_DimensionToolUsed.Text = cb_DimensionToolUsed.Checked == true ? "USE" : "UNUSED";
            cb_DimensionToolUsed.ForeColor = cb_DimensionToolUsed.Checked == true ? Color.Lime : Color.Red;
        }

        private void cb_DimensionToolUsed_CheckedChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
            {
                Glob.코그넥스파일.거리측정툴사용여부[Glob.CamNumber, (int)num_DimensionToolNum.Value] = cb_DimensionToolUsed.Checked;
                DistanceEnableChange((int)num_DimensionToolNum.Value);
            }
        }

        private void Num1_DimensionTool_Area_Click(object sender, EventArgs e)
        {
            string ToolName = Num1_DimensionTool.SelectedItem.ToString();
            string[] Tool1_Name = ToolName.Split('-');

            if (Tool1_Name[0] == "Line ")
            {
                ImageClear();
                Pattern_Train();
                Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool1_Name[1])].Area(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ToolName());
            }
        }
        private void LineInspection(string ToolName)
        {
            CogGraphicCollection Collection = new CogGraphicCollection();
            string[] Tool1_Name = ToolName.Split('-');

            Pattern_Train();

            Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool1_Name[1])].Run((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool1_Name[1])].ResultDisplay(cdyDisplay, Collection);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputLine((int)num_DimensionToolNum.Value, Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool1_Name[1])].GetLine());
        }

        private void CaliperXY(string ToolName)
        {
            CogGraphicCollection Collection = new CogGraphicCollection();
            string[] Tool2_Name = ToolName.Split('-');

            Pattern_Train();

            Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Run((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].ResultDisplay(ref cdyDisplay);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputXY(Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Result_Corner_X(), Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Result_Corner_Y());
        }
        private void AveragePointXY(string ToolName)
        {
            CogGraphicCollection Collection = new CogGraphicCollection();
            string[] Tool2_Name = ToolName.Split('-');

            Pattern_Train();

            Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Run((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].ResultDisplay(cdyDisplay, Collection);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputXY(Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Average_PointX(), Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Average_PointY());
        }
        private void btn_Tool1Inspect_Click(object sender, EventArgs e)
        {
            //CogGraphicCollection Collection = new CogGraphicCollection();
            string ToolName = Num1_DimensionTool.SelectedItem.ToString();
            string[] Tool1_Name = ToolName.Split('-');
            if (Tool1_Name[0] == "Line ")
            {
                ImageClear();
                LineInspection(ToolName);
            }
        }
        private void Num2_DimensionTool_Area_Click(object sender, EventArgs e)
        {
            string ToolName = Num2_DimensionTool.SelectedItem.ToString();
            string[] Tool2_Name = ToolName.Split('-');
            ImageClear();
            Pattern_Train();
            switch (Tool2_Name[0])
            {
                case "Line ":
                    Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Area(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ToolName());
                    break;
                case "Caliper ":
                    Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Area(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ToolName());
                    break;
                case "Circle ":
                    Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Area(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, Glob.코그넥스파일.패턴툴[Glob.CamNumber, 0].ToolName());
                    break;
            }
        }
        private void btn_Tool2Inspect_Click(object sender, EventArgs e)
        {
            CogGraphicCollection Collection = new CogGraphicCollection();
            string ToolName = Num2_DimensionTool.SelectedItem.ToString();
            string[] Tool2_Name = ToolName.Split('-');
            ImageClear();
            switch (Tool2_Name[0])
            {
                case "Line ":
                    AveragePointXY(ToolName);
                    break;
                case "Caliper ":
                    CaliperXY(ToolName);
                    break;
                case "Circle ":
                    Circle(ToolName);
                    break;
            }
        }
        private void Circle(string ToolName)
        {
            CogGraphicCollection Collection = new CogGraphicCollection();
            string[] Tool2_Name = ToolName.Split('-');

            Pattern_Train();

            Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].Run((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].ResultDisplay(ref cdyDisplay);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputCircle(Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(Tool2_Name[1])].GetCircle());
        }
        private void btn_DimensionTool_Inspection_Click(object sender, EventArgs e)
        {
            if (cdyDisplay.Image == null)
                return;

            CogGraphicCollection Collection = new CogGraphicCollection();
            CogCreateGraphicLabelTool Label = new CogCreateGraphicLabelTool();
            double ResultValue = 0;
            ImageClear();
            Pattern_Train();
            string Tool1_Name = TempDistance_Tool1_Number[Glob.CamNumber, (int)num_DimensionToolNum.Value];
            string Tool2_Name = TempDistance_Tool2_Number[Glob.CamNumber, (int)num_DimensionToolNum.Value];
            string[] splitTool2Name = Tool2_Name.Split('-');

            LineInspection(Tool1_Name);
            switch (splitTool2Name[0])
            {
                case "Line ":
                    AveragePointXY(Tool2_Name);
                    break;
                case "Caliper ":
                    CaliperXY(Tool2_Name);
                    break;
                case "Circle ":
                    Circle(Tool2_Name);
                    break;
            }

            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].Run((int)num_DimensionToolNum.Value, (CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].ResultDisplay((int)num_DimensionToolNum.Value, cdyDisplay, Collection);
            ResultValue = Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].DistanceValue((int)num_DimensionToolNum.Value) * Glob.코그넥스파일.보정값[Glob.CamNumber, (int)num_DimensionToolNum.Value];
            tb_DemensionTool_Pix.Text = Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].DistanceValue((int)num_DimensionToolNum.Value).ToString("F3");
            tb_DemensionTool_Measure.Text = ResultValue.ToString("F3");

            if (Glob.코그넥스파일.최소값[Glob.CamNumber, (int)num_DimensionToolNum.Value] <= ResultValue && Glob.코그넥스파일.최대값[Glob.CamNumber, (int)num_DimensionToolNum.Value] >= ResultValue)
            {
                Label.OutputColor = CogColorConstants.Green;
            }
            else
            {
                Label.OutputColor = CogColorConstants.Red;
            }

            Label.InputImage = cdyDisplay.Image;
            Label.InputGraphicLabel.X = 1200;
            Label.InputGraphicLabel.Y = 600;
            Label.InputGraphicLabel.Text = $"{Glob.코그넥스파일.거리측정툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].ToolName((int)num_DimensionToolNum.Value)} : {tb_DemensionTool_Measure.Text}";
            Label.Run();
            cdyDisplay.StaticGraphics.Add(Label.GetOutputGraphicLabel(), "");
        }

        private void label57_DoubleClick(object sender, EventArgs e)
        {
            string toolname = Num1_DimensionTool.SelectedItem.ToString();
            string[] s_toolname = toolname.Split('-');
            switch (s_toolname[0])
            {
                case "Line ":
                    Glob.코그넥스파일.라인툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputImage((CogImage8Grey)cdyDisplay.Image);
                    Glob.코그넥스파일.라인툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].ToolSetup();
                    break;
                case "Caliper ":
                    Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].InputImage((CogImage8Grey)cdyDisplay.Image);
                    Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, (int)num_DimensionToolNum.Value].ToolSetup();
                    break;
            }
        }

        private void label59_DoubleClick(object sender, EventArgs e)
        {
            string toolname = Num2_DimensionTool.SelectedItem.ToString();
            string[] s_toolname = toolname.Split('-');
            switch (s_toolname[0])
            {
                case "Line ":
                    Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].InputImage((CogImage8Grey)cdyDisplay.Image);
                    Glob.코그넥스파일.라인툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].ToolSetup();
                    break;
                case "Caliper ":
                    Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].InputImage((CogImage8Grey)cdyDisplay.Image);
                    Glob.코그넥스파일.캘리퍼툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].ToolSetup();
                    break;
                case "Circle ":
                    Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].InputImage((CogImage8Grey)cdyDisplay.Image);
                    Glob.코그넥스파일.써클툴[Glob.CamNumber, Convert.ToInt32(s_toolname[1])].ToolSetup();
                    break;
            }
        }

        private void btn_DemensionTool_GetCalValue_Click(object sender, EventArgs e)
        {
            try
            {
                Glob.코그넥스파일.보정값[Glob.CamNumber, (int)num_DimensionToolNum.Value] = Convert.ToDouble(tb_DemensionTool_Measure.Text) / Convert.ToDouble(tb_DemensionTool_Pix.Text);
                tb_DemensionTool_Calibration.Text = Glob.코그넥스파일.보정값[Glob.CamNumber, (int)num_DimensionToolNum.Value].ToString("F5");
            }
            catch
            {
                Glob.코그넥스파일.보정값[Glob.CamNumber, (int)num_DimensionToolNum.Value] = Convert.ToDouble(tb_DemensionTool_Calibration.Text);
                //tb_DemensionTool_Calibration.Text = tb_DemensionTool_Calibration.Text;
            }
        }

        private void btn_SpecApply_Click(object sender, EventArgs e)
        {
            Glob.코그넥스파일.최소값[Glob.CamNumber, (int)num_DimensionToolNum.Value] = Convert.ToDouble(tb_LowValue.Text);
            Glob.코그넥스파일.최대값[Glob.CamNumber, (int)num_DimensionToolNum.Value] = Convert.ToDouble(tb_HighValue.Text);
        }

        private void 마스터이미지등록_Click(object sender, EventArgs e)
        {
            string fileName;
            if (cdyDisplay.Image == null)
            {
                MessageBox.Show("이미지가 없습니다. 카메라촬영을 진행해 주시거나, 이미지파일을 불러오시기 바랍니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var result = MessageBox.Show($"현재 이미지를 모델 {Glob.RunnModel.Modelname()}의 CAM{Glob.CamNumber + 1} Master {Glob.InspectOrder} 이미지로 저장 하시겠습니까?", " 이미지 저장 ",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                fileName = $"CAM{Glob.CamNumber}_Master";
                CogImageFileBMP ImageSave = new CogImageFileBMP();
                ImageSave.Open($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\Cam{Glob.CamNumber}\\CAM{Glob.CamNumber}_Master_{Glob.InspectOrder}.bmp", CogImageFileModeConstants.Write);
                ImageSave.Append(cdyDisplay.Image);
                ImageSave.Close();
                Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                MessageBox.Show($"CAM{Glob.CamNumber + 1} Master Image 등록 완료", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_OpenMaster_Click(object sender, EventArgs e)
        {
            string ImageType;
            string ImageName = $"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\Cam{Glob.CamNumber}\\CAM{Glob.CamNumber}_Master_{Glob.InspectOrder}.bmp";
            FileInfo fileInfo = new FileInfo(ImageName);

            cdyDisplay.InteractiveGraphics.Clear();
            cdyDisplay.StaticGraphics.Clear();

            if (!fileInfo.Exists)
            {
                MessageBox.Show("마스터 이미지가 없습니다. 마스터이미지를 등록해주시기 바랍니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CogImageFileTool curimage = new CogImageFileTool();
            curimage.Operator.Open(ImageName, CogImageFileModeConstants.Read);
            curimage.Run();
            ImageType = curimage.OutputImage.GetType().ToString();
            if (ImageType.Contains("CogImage24PlanarColor"))
            {
                CogImageConvertTool imageconvert = new CogImageConvertTool();
                imageconvert.InputImage = curimage.OutputImage;
                imageconvert.RunParams.RunMode = CogImageConvertRunModeConstants.Plane2;
                imageconvert.Run();
                cdyDisplay.Image = (CogImage8Grey)imageconvert.OutputImage;
            }
            else
            {
                cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage; //JPG 파일
            }
            //cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage;
            cdyDisplay.Fit();
            //GC.Collect();
        }

        private void btn_OpenCamSetfile_Click(object sender, EventArgs e)
        {
            Glob.코그넥스파일.카메라[Glob.CamNumber].ToolSetup();
        }

        private void num_InspectOrder_ValueChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
            {
                Glob.코그넥스파일.패턴툴검사순서번호[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value] = Convert.ToInt32(num_InspectOrder.Value);
                MultiPatternOrderChange((int)num_MultiPatternToolNumber.Value);
            }
        }

        private void MultiPatternOrderChange(int toolnumber)
        {
            num_InspectOrder.Value = Glob.코그넥스파일.패턴툴검사순서번호[Glob.CamNumber, toolnumber];
        }

        private void num_GlobOrderNumber_ValueChanged(object sender, EventArgs e)
        {
            Glob.InspectOrder = (int)num_GlobOrderNumber.Value;
        }

        private void btn_MaskAreaSet_Click(object sender, EventArgs e)
        {
            if (Glob.코그넥스파일.마스크툴[Glob.CamNumber, Glob.InspectOrder - 1].InputImage((CogImage8Grey)cdyDisplay.Image) == false) return;
            //shotNumber = 1 : 0 / 2: 1 / 3: 2
            Mask_Train(Glob.InspectOrder - 1);
            Glob.코그넥스파일.마스크툴[Glob.CamNumber, Glob.InspectOrder - 1].Area_Affine_Main1(ref cdyDisplay, (CogImage8Grey)cdyDisplay.Image, cb_MultiPatternName.SelectedIndex.ToString());

            Glob.코그넥스파일.마스크툴[Glob.CamNumber, Glob.InspectOrder - 1].ToolSetup();
        }

        private void cb_ngokchange_CheckedChanged(object sender, EventArgs e)
        {
            if (Dataset == false)
            {
                Glob.코그넥스파일.블롭툴역검사[Glob.CamNumber, (int)num_BlobToolNum.Value] = cb_ngokchange.Checked;
                BlobNGOKChangeChange((int)num_BlobToolNum.Value);
            }
        }

        private void num_LightControlNumber_ValueChanged(object sender, EventArgs e)
        {
            Dataset = true;
            Glob.LightControlNumber = (int)num_LightControlNumber.Value;
            num_LightCH1.Maximum = Glob.LightControlNumber == 3 ? 1023 : 100;
            num_LightCH2.Maximum = Glob.LightControlNumber == 3 ? 1023 : 100;
            Dataset = false;

            num_LightCH1.Value = Convert.ToDecimal(Glob.LightChAndValue[Glob.LightControlNumber, 0]);
            num_LightCH2.Value = Convert.ToDecimal(Glob.LightChAndValue[Glob.LightControlNumber, 1]);
        }

        private void num_LightCH1_ValueChanged(object sender, EventArgs e)
        {
            //LCP_100DC는 채널번호 1부터시작.
            //LCP24_150DC는 채널번호 0부터시작.
            if (Dataset == true) return;
            if (cb조명사용여부.Checked == false) return;

            Glob.LightChAndValue[Glob.LightControlNumber, 0] = (int)num_LightCH1.Value;
            if (Main.LightControl[Glob.LightControlNumber].IsOpen == false)
            {
                return;
            }
            if (Glob.LightControlNumber == 3)
            {
                Main.LCP24_150DC(Main.LightControl[Glob.LightControlNumber], "0", Glob.LightChAndValue[Glob.LightControlNumber, 0].ToString("D4"));
            }
            else
            {
                Main.LCP_100DC(Main.LightControl[Glob.LightControlNumber], "1", "d", Glob.LightChAndValue[Glob.LightControlNumber, 0].ToString("D4"));
            }
        }

        private void num_LightCH2_ValueChanged(object sender, EventArgs e)
        {
            //LCP_100DC는 채널번호 1부터시작.
            //LCP24_150DC는 채널번호 0부터시작.
            if (Dataset == true) return;

            Glob.LightChAndValue[Glob.LightControlNumber, 1] = (int)num_LightCH2.Value;
            if (Main.LightControl[Glob.LightControlNumber].IsOpen == false)
            {
                return;
            }
            if (Glob.LightControlNumber == 3)
            {
                Main.LCP24_150DC(Main.LightControl[Glob.LightControlNumber], "1", Glob.LightChAndValue[Glob.LightControlNumber, 1].ToString("D4"));
            }
            else
            {
                Main.LCP_100DC(Main.LightControl[Glob.LightControlNumber], "2", "d", Glob.LightChAndValue[Glob.LightControlNumber, 1].ToString("D4"));
            }
        }

        private void 패턴설정값일괄적용(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Pattern - {num_MultiPatternToolNumber.Value} 설정값을 일괄 적용 하시겠습니까?\n적용 되는 값 : 스코어", "Setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            for (int lop = 0; lop < Glob.코그넥스파일.패턴툴.GetLength(1); lop++)
            {
                Glob.코그넥스파일.패턴툴[Glob.CamNumber, lop].Threshold((double)num_MultiPatternScore.Value / 100);
            }

            MessageBox.Show($"일괄 적용 완료\n스코어값 : {num_MultiPatternScore.Value}", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 블롭설정값일괄적용(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Blob - {num_BlobToolNum.Value} 설정값을 일괄 적용 하시겠습니까?\n적용 되는 값 : 검사 최소 픽셀, 명암값", "Setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            for (int lop = 0; lop < Glob.코그넥스파일.블롭툴.GetLength(1); lop++)
            {
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, lop].Minipixel((int)num_BlobMinipixel.Value);
                Glob.코그넥스파일.블롭툴[Glob.CamNumber, lop].Threshold((int)num_BlobThreshold.Value);
            }

            MessageBox.Show($"일괄 적용 완료\n검사 최소 픽셀 : {num_BlobMinipixel.Value}\n명암값 : {num_BlobThreshold.Value}", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label6_DoubleClick(object sender, EventArgs e)
        {
            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].InputImage((CogImage8Grey)cdyDisplay.Image);
            Glob.코그넥스파일.블롭툴[Glob.CamNumber, (int)num_BlobToolNum.Value].ToolSetup();
        }

        private void btn_MakeToolGroup_Click(object sender, EventArgs e)
        {
            int CameraNumber = Glob.CamNumber;
            CogImage8Grey image = (CogImage8Grey)cdyDisplay.Image;
            for (int lop = 0; lop < 30; lop++)
            {
                if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == Glob.InspectOrder)
                {
                    Glob.코그넥스파일.카메라[CameraNumber].패턴툴추가(Glob.코그넥스파일.패턴툴[CameraNumber, lop].PMTool());
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CogImage8Grey image = (CogImage8Grey)cdyDisplay.Image;
            Glob.코그넥스파일.카메라[Glob.CamNumber].InputImage = image;
            Form Window = new Form();
            CogToolBlockEditV2 viewForm = new CogToolBlockEditV2() { Subject = Glob.코그넥스파일.카메라[Glob.CamNumber].OpenToolBlock(), Dock = DockStyle.Fill };

            Window.Controls.Add(viewForm); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.ShowDialog(); // 폼 실행
            //viewForm.Init(도구);
            //viewForm.Show(Global.MainForm);
        }

        private void btn_ToolGroupSave_Click(object sender, EventArgs e)
        {
            CogJob Job = new CogJob();
            string savePath = string.Empty;

            Job = Glob.코그넥스파일.카메라[Glob.CamNumber].JobFile();
            savePath = Glob.코그넥스파일.카메라[Glob.CamNumber].savePath;

            CogSerializer.SaveObjectToFile(Job, savePath, typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
            //Debug.WriteLine(this.도구경로, "도구저장");
        }
    }
}
