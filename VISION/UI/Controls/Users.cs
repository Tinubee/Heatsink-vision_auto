using DevExpress.XtraBars;
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
    public partial class Users : XtraUserControl
    {
        LocalizationUsers 번역 = new LocalizationUsers();
        public Users() => InitializeComponent();

        public void Init()
        {
            this.GridView1.Init(this.barManager1);
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.AddDeleteMenuItem(유저삭제_Click);
            this.GridControl1.DataSource = Global.유저자료;
            this.b유저저장.Click += 유저저장_Click;
            Localization.SetColumnCaption(this.GridView1, typeof(유저정보));
            this.g유저관리.Text = 유저자료.로그영역.GetString();
            this.b유저저장.Text = 번역.유저저장;
        }

        private void 유저삭제_Click(object sender, ItemClickEventArgs e)
        {
            유저정보 정보 = this.GridView1.GetFocusedRow() as 유저정보;
            if (정보 == null) return;
            if (!Utils.Confirm($"[{정보.성명}] {번역.삭제확인}", Localization.확인.GetString())) return;
            //if (Global.유저자료.Remove(정보)) Global.정보로그(유저자료.로그영역.GetString(), 번역.유저삭제, $"[{정보.성명}] {번역.유저제거}", false);
        }

        private void 유저저장_Click(object sender, EventArgs e)
        {
            if (!Utils.Confirm(번역.저장확인, Localization.확인.GetString())) return;
            Global.유저자료.Save();
            //Global.정보로그(유저자료.로그영역.GetString(), 번역.정보저장, 번역.저장완료, this.FindForm());
        }

        public void Close()
        {

        }


        private class LocalizationUsers
        {
            private enum Items
            {
                [Translation("Save", "정보저장")]
                정보저장,
                [Translation("It's saved.", "저장되었습니다.")]
                저장완료,
                [Translation("Save users information?", "사용자정보를 저장하시겠습니까?")]
                저장확인,
                [Translation("Delete this selected user?", "선택 사용자를 삭제하시겠습니까?")]
                삭제확인,
                [Translation("Remove user", "사용자 삭제")]
                유저삭제,
                [Translation("Removed.", "삭제되었습니다.")]
                유저제거,
            }

            public String 정보저장 { get { return Localization.GetString(Items.정보저장); } }
            public String 저장완료 { get { return Localization.GetString(Items.저장완료); } }
            public String 저장확인 { get { return Localization.GetString(Items.저장확인); } }
            public String 삭제확인 { get { return Localization.GetString(Items.삭제확인); } }
            public String 유저삭제 { get { return Localization.GetString(Items.유저삭제); } }
            public String 유저제거 { get { return Localization.GetString(Items.유저제거); } }
            public String 유저저장 { get { return Localization.저장.GetString(); } }
        }
    }
}
