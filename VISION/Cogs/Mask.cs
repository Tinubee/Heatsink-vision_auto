﻿using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION.Cogs
{
    public class Mask
    {
        private CogJob job = new CogJob();
        private CogMaskCreatorTool Tool;

        /// <summary>
        /// 툴의 기본 초기화를 담당 함. 입력 되는 이름은 툴 파일 저장과 읽어 오는 작업에 쓰임.
        /// </summary>
        /// <param name="Toolnumber">툴의 이름.</param>
        public Mask(int Toolnumber = 0)
        {
            Tool = new CogMaskCreatorTool();
            Tool.Name = "Mask - " + Toolnumber.ToString();
        }
        /// <summary>
        /// 파일에서 툴을 불러 옴. 파일이 있는 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">파일이 있는  폴더의 경로</param>
        /// <returns></returns>
        public bool Loadtool(string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                CogSerializer.SaveObjectToFile(this.Tool, path);
            }
            Tool = (CogMaskCreatorTool)CogSerializer.LoadObjectFromFile(path);
            CogToolGroup group = new CogToolGroup();
            job.VisionTool = group;
            group.Tools.Add(Tool);
            return true;
        }

        /// <summary>
        /// 파일에 툴의 정보를 씀. 대상 파일이 위치 할 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">저장 할 대상 폴더의 경로</param>
        /// <returns></returns>
        public bool SaveTool(string path, string camName)
        {
            string Savepath = path;

            if (System.IO.Directory.Exists(Savepath) == false)
            {
                return false;
            }

            Savepath = Savepath + "\\" + camName;

            CogSerializer.SaveObjectToFile(Tool, Savepath);

            return true;
        }

        /// <summary>
        /// 툴에 이미지 입력
        /// </summary>
        /// <param name="image">툴에 입력 할 이미지</param>
        /// <returns></returns>
        public bool InputImage(Cognex.VisionPro.CogImage8Grey image)
        {
            if (image == null)
            {
                return false;
            }

            Tool.InputImage = image;
            return true;
        }


        public void Close()
        {
            this.job.Shutdown();
        }

        /// <summary>
        /// 툴 동작. 검사를 수행함.
        /// </summary>
        /// <param name="image">툴에 입력 할 이미지.</param>
        /// <returns>결과</returns>
        public bool Run(Cognex.VisionPro.CogImage8Grey image)
        {
            if (InputImage(image) == false)
            {
                return false;
            }

            Tool.Run();

            return true;
        }

        public bool Run(Cognex.VisionPro.CogImage8Grey image, Points Point, string SpaceName)
        {
            if (InputImage(image) == false)
            {
                return false;
            }

            Tool.Run();

            if (Tool.Result.Mask == null)
            {
                return false;
            }

            return true;
        }

        public Cognex.VisionPro.CogImage8Grey MaskArea()
        {
            if (Tool.Result == null) return null;

            return this.Tool.Result.Mask;
        }

        public void Area_Affine_Main1(ref CogDisplay display, Cognex.VisionPro.CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }
            int count = this.Tool.RunParams.MaskAreas.Count;
        }

        public void ToolSetup()
        {
            Form Window = new Form();
            CogMaskCreatorEditV2 Edit = new CogMaskCreatorEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.FormClosed += Window_FormClosed;

            Window.Show(); // 폼 실행
        }

        public void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
           this.Tool.Run();
        }
    }
}

