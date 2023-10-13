using Cognex.VisionPro;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Display;
using KimLib;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VISION.Cogs;

namespace VISION.Class
{
    public class Inspection
    {
        private PGgloble Glob;
        private CogImage8Grey OriginImage;
        private CogImage8Grey Fiximage;
        private string FimageSpace;
        private double[] 이미지회전각도;

        public void Init()
        {
            Glob = PGgloble.getInstance;
            OriginImage = new CogImage8Grey();
            이미지회전각도 = new double[8];
        }

        public CogImage8Grey CamShot(int funCamNumber)
        {
            OriginImage =  Glob.코그넥스파일.카메라[funCamNumber].Run();
            Glob.G_MainForm.TempCogDisplay[funCamNumber].Image = OriginImage;
            //log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
            Glob.G_MainForm.TempCogDisplay[funCamNumber].Fit();
            Glob.G_MainForm.TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
            Glob.G_MainForm.TempCogDisplay[funCamNumber].StaticGraphics.Clear();

            return OriginImage;
        }

        public bool Run(CogDisplay cog, int CameraNumber, int shotNumber)
        {
            if (CameraNumber == 1 || CameraNumber == 2 || CameraNumber == 5)
                이미지회전각도[CameraNumber] = Glob.G_MainForm.frm_toolsetup == null ? 1.5708 : 0;
            else
                이미지회전각도[CameraNumber] = 0;

            Debug.WriteLine($"Cam - {CameraNumber} 회전각도 : {이미지회전각도[CameraNumber]}");

            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            Glob.G_MainForm.InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber1(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 이미지회전각도[CameraNumber])) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            Glob.G_MainForm.InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                Glob.G_MainForm.InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }
            //Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                Glob.G_MainForm.InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도[CameraNumber], "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도[CameraNumber], "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도[CameraNumber], "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도[CameraNumber], "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");

            //Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            GC.Collect();
            return Glob.G_MainForm.InspectResult[CameraNumber];
        }

        public int FindFirstPatternNumber1(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }

        public void Bolb_Train(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage1((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        public void DisplayLabelShow(CogGraphicCollection Collection, CogDisplay cog, int X, int Y, double rotate, string Text)
        {
            CogCreateGraphicLabelTool Label = new CogCreateGraphicLabelTool();
            Label.InputGraphicLabel.Color = CogColorConstants.Green;
            Label.InputImage = cog.Image;
            Label.InputGraphicLabel.X = rotate == 0 ? X : Y;
            Label.InputGraphicLabel.Y = rotate == 0 ? Y : X;
            Label.InputGraphicLabel.Rotation = rotate;
            Label.InputGraphicLabel.Text = Text;
            Label.Run();
            Collection.Add(Label.GetOutputGraphicLabel());
        }
    }

}
