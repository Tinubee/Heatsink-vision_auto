using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public enum 유저권한구분
    {
        [Description("Nothing"), Translation("Nothing", "없음")]
        없음 = 0,
        [Description("Worker"), Translation("Worker", "작업자")]
        작업자 = 3,
        [Description("Manager"), Translation("Manager", "관리자")]
        관리자 = 5,
        [Description("Admin"), Translation("Admin", "시스템")]
        시스템 = 9,
    }

    [Table("user")]
    public class 유저정보
    {
        [Column("uname"), Required, Key, DisplayName("사용자성명"), Translation("Name", "성명")]
        public string 성명 { get; set; } = string.Empty;
        [Column("upass"), DisplayName("비밀번호"), Translation("Password", "비밀번호")]
        public string 암호 { get; set; } = string.Empty;
        [Column("unote"), Translation("Note", "비고")]
        public string 비고 { get; set; } = string.Empty;
        [Column("uperm"), DisplayName("접근권한"), Translation("Authority", "접근권한")]
        public 유저권한구분 권한 { get; set; } = 유저권한구분.작업자;
        [Column("uallow"), DisplayName("접근허용"), Translation("Permit", "접근허용")]
        public bool 허용 { get; set; } = true;
    }

    public class 유저자료 : BindingList<유저정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Users", "사용자 관리");
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "Users.conf"); } }

        public Boolean Init()
        {
            return this.Load();
        }

        public void Close()
        {

        }

        public Boolean Load()
        {
            if (!File.Exists(저장파일))
            {
                this.Add(new 유저정보() { 성명 = "user", 암호 = "0000", 권한 = 유저권한구분.작업자 });
                this.Add(new 유저정보() { 성명 = "manager", 암호 = "0000", 권한 = 유저권한구분.관리자 });
                this.Add(new 유저정보() { 성명 = "admin", 암호 = "0000", 권한 = 유저권한구분.시스템 });
                this.Save();
                return true;
            }

            try
            {
                String json = StringCipher.Decrypt(File.ReadAllText(저장파일), Global.GetGuid());
                List<유저정보> 자료 = JsonConvert.DeserializeObject<List<유저정보>>(json);
                자료.ForEach(e => this.Add(e));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역.GetString(), "정보로드", "사용자 정보를 불러올 수 없습니다.\n" + ex.Message, true);
            }
            return false;
        }

        public void Save()
        {
            File.WriteAllText(저장파일, StringCipher.Encrypt(JsonConvert.SerializeObject(this), Global.GetGuid()));
        }

        public 유저정보 GetItem(string 성명)
        {
            return this.Where(e => e.성명 == 성명).FirstOrDefault();
        }

        public List<string> 사용자목록()
        {
            List<string> 사용자명 = new List<string>();
            foreach (유저정보 정보 in this)
            {
                if (String.IsNullOrEmpty(정보.성명) || String.IsNullOrEmpty(정보.암호) || !정보.허용) continue;
                사용자명.Add(정보.성명);
            }
            return 사용자명;
        }

        public 유저권한구분 비밀번호확인(string 사용자명, string 비밀번호)
        {
            if (String.IsNullOrEmpty(사용자명) || String.IsNullOrEmpty(비밀번호)) return 유저권한구분.없음;
            유저정보 정보 = this.GetItem(사용자명);
            if (정보 == null || !정보.허용 || 정보.암호 != 비밀번호) return 유저권한구분.없음;
            Properties.Settings.Default.UserName = 사용자명;
            Global.환경설정.사용자명 = 사용자명;
            Global.환경설정.사용권한 = 정보.권한;
            //Global.정보로그(로그영역.GetString(), "로그인", $"[{사용자명}] 로그인 하였습니다.", false);
            return 정보.권한;
        }
    }
}
