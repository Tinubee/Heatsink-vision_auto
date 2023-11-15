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

namespace VISION.UI.Controls
{
    public partial class Config : XtraUserControl
    {
        private LocalizationConfig 번역 = new LocalizationConfig();
        public Config()
        {
            InitializeComponent();
            this.BindLocalization.DataSource = this.번역;
            //this.g환경설정.Text = 환경설정.로그영역.GetString();
        }
        public void Init()
        {
            this.Bind환경설정.DataSource = Global.환경설정;
            this.d기본경로.SelectedPath = Global.환경설정.기본경로;
            this.d문서저장.SelectedPath = Global.환경설정.데이터저장경로;
            this.e기본경로.ButtonClick += E기본경로_ButtonClick;
            this.e문서저장.ButtonClick += E문서저장_ButtonClick;
            this.e사진저장.ButtonClick += E사진저장_ButtonClick;
            this.b설정저장.Click += b설정저장_Click;
        }

        public void Close()
        {

        }

        private void E사진저장_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d사진저장.ShowDialog() == DialogResult.OK)
                this.e사진저장.Text = this.d사진저장.SelectedPath;
        }

        private void E문서저장_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d문서저장.ShowDialog() == DialogResult.OK)
                this.e문서저장.Text = this.d문서저장.SelectedPath;
        }

        private void E기본경로_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d기본경로.ShowDialog() == DialogResult.OK)
                this.e기본경로.Text = this.d기본경로.SelectedPath;
        }

        private void b설정저장_Click(object sender, EventArgs e)
        {
            this.Bind환경설정.EndEdit();
            if (!Utils.Confirm(번역.저장확인, Localization.확인.GetString())) return;
            Global.환경설정.Save();
            //Global.정보로그(환경설정.로그영역.GetString(), 번역.설정저장, 번역.저장완료, true);
        }


        private class LocalizationConfig
        {
            private enum Items
            {
                [Translation("Save", "설정저장")]
                설정저장,
                [Translation("It's saved.", "저장되었습니다.")]
                저장완료,
                [Translation("Save your preferences?", "환경설정을 저장하시겠습니까?")]
                저장확인,
            }

            public String 기본경로 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.기본경로))); } }
            public String 문서저장 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.데이터저장경로))); } }
            public String 사진저장 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.이미지저장경로))); } }
            public String 사진저장OK { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.사진저장OK))); } }
            public String 사진저장NG { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.사진저장NG))); } }
            public String 결과보관 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.결과보관))); } }
            public String 로그보관 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.로그보관))); } }
            public String 결과자릿수 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.결과자릿수))); } }
            public String 검사여부 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.검사여부))); } }
            //public String 크롭여부 { get { return Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.크롭여부))); } }

            public String 설정저장 { get { return Localization.GetString(Items.설정저장); } }
            public String 저장완료 { get { return Localization.GetString(Items.저장완료); } }
            public String 저장확인 { get { return Localization.GetString(Items.저장확인); } }
        }
    }
}
