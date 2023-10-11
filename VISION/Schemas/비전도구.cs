using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VISION.Schemas
{
    public class 비전도구
    {
        public void Init() => this.Load();

        public void Load()
        {
            //if (File.Exists(this.도구경로))
            //{
            //    this.Job = (CogJob)CogSerializer.LoadObjectFromFile(this.도구경로);
            //    this.ToolBlock = (this.Job.VisionTool as CogToolGroup).Tools[0] as CogToolBlock;
            //}
            //else
            //{
            //    this.Job = new CogJob($"Job{도구명칭}");
            //    CogToolGroup group = new CogToolGroup() { Name = $"Group{도구명칭}" };
            //    this.ToolBlock = new CogToolBlock();
            //    this.ToolBlock.Name = this.도구명칭;
            //    group.Tools.Add(this.ToolBlock);
            //    this.Job.VisionTool = group;
            //    this.ToolBlock.Inputs.Add(new CogToolBlockTerminal("InputImage", typeof(CogImage8Grey)));
            //    CogToolBlock alignTools = new CogToolBlock() { Name = "AlignTools" };
            //    alignTools.Inputs.Add(new CogToolBlockTerminal("InputImage", typeof(CogImage8Grey)));
            //    alignTools.Inputs.Add(new CogToolBlockTerminal("PositionY", 0d));
            //    alignTools.Inputs.Add(new CogToolBlockTerminal("OriginY", 0d));
            //    alignTools.Outputs.Add(new CogToolBlockTerminal("OutputImage", typeof(CogImage8Grey)));
            //    alignTools.Outputs.Add(new CogToolBlockTerminal("OffsetY", 0d));
            //    alignTools.Tools.Add(new CogAffineTransformTool { Name = "CropImage" });
            //    this.ToolBlock.Tools.Add(alignTools);
            //    this.Save();
            //}

            //if (this.ToolBlock != null) this.ToolBlock.DataBindings.Clear();
            //else this.ToolBlock = new CogToolBlock();
            //this.ToolBlock.Name = this.도구명칭;

            //// Output 파라메터 설정, 일단 CTQ 항목만
            //검사설정자료 자료 = Global.모델자료.GetItem(this.모델구분)?.검사설정;
            //if (자료 == null) return;
            //List<검사정보> 목록 = 자료.Where(e => (Int32)e.검사장치 == (Int32)this.카메라 && !String.IsNullOrEmpty(e.변수명칭)).ToList();
            //foreach (검사정보 검사 in 목록)
            //{
            //    if (this.ToolBlock.Outputs.Contains(검사.변수명칭)) continue;
            //    if (검사.검사그룹 == 검사그룹.CTQ)
            //        this.ToolBlock.Outputs.Add(new CogToolBlockTerminal(검사.변수명칭, null, typeof(Double)));
            //    else { }
            //    //Debug.WriteLine($"{검사.검사항목} = {검사.변수명칭}", this.카메라.ToString());
            //}
        }
    }
}
