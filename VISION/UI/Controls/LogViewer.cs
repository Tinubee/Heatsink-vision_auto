using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.Schemas;

namespace VISION.UI
{
    public partial class LogViewer : DevExpress.XtraEditors.XtraUserControl
    {
        private LocalizationLogs 번역 = new LocalizationLogs();
        public LogViewer() => InitializeComponent();

        public void Init()
        {
            this.BindLocalization.DataSource = 번역;
            Localization.SetColumnCaption(this.GridView1, typeof(로그정보));

            e시작.DateTime = DateTime.Today;
            e종료.DateTime = DateTime.Today;
            b검색.ImageOptions.SvgImage = Resources.GetSvgImage(SvgImageType.검색);
            GridView1.Init();
            GridControl1.DataSource = Global.로그자료;
            this.b검색.Click += B검색_Click;
        }

        public void Shown()
        {
            this.GridView1.BestFitColumns();
        }

        public void Close() { }

        private void B검색_Click(object sender, EventArgs e)
        {
            Global.로그자료.Load(this.e시작.DateTime, this.e종료.DateTime);
        }

        private class LocalizationLogs
        {
            private enum Items
            {
                [Translation("Start Day", "시작일자")]
                시작일자,
                [Translation("End Day", "종료일자")]
                종료일자,
                [Translation("Search", "조  회")]
                조회버튼,
            }

            public String 시작일자 { get { return Localization.GetString(Items.시작일자); } }
            public String 종료일자 { get { return Localization.GetString(Items.종료일자); } }
            public String 조회버튼 { get { return Localization.GetString(Items.조회버튼); } }
        }
    }
}
