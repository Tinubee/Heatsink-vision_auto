using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VISION
{
    public partial class Frm_CamSet : Form
    {
        Frm_Main Main;
        private Class_Common cm { get { return Program.cm; } } //에러 메세지 보여주기.
        public Frm_CamSet(Frm_Main main)
        {
            InitializeComponent();
            Main = main;
        }

        private void Frm_CamList_Load(object sender, EventArgs e)
        {
            //그리드뷰에 데이터 바인딩 되어있음(CAMLIST 클래스)
            camListBindingSource.DataSource = Program.CameraList; //Main에서 로드시 Cam정보를 로드함. 이것을 데이터소스에 넣어준다.
            camListBindingSource.ResetBindings(false);
            num_CamNumber.Value = Program.CameraList.Count();
        }

        private void btn_CamListAdd_Click(object sender, EventArgs e)
        {
            bool AddCam = true;
            foreach (CamList info in Program.CameraList) //카메라 리스트에서 확인.
            {
                if (num_CamNumber.Value.ToString() == info.Number) //순서번호가 같은게 있으면 리턴
                {
                    cm.info("Check Camera Number");
                    AddCam = false;
                    return;
                }
                if (tb_CamSerialNumber.Text == info.SerialNum || tb_CamSerialNumber.Text == "") //중복되는 고유번호나, 비어있으면 리턴
                {
                    cm.info("Check Camera Serial Number");
                    AddCam = false;
                    return;
                }
            }
            if (AddCam) //위의 모든조건을 통과했음 - 등록절차.
            {
                Program.CameraList.Add(new CamList() //CAMLIST에 정보를 추가해준다.
                {
                    Number = num_CamNumber.Value.ToString(),
                    SerialNum = tb_CamSerialNumber.Text
                });
                camListBindingSource.DataSource = Program.CameraList; //바인딩된 데이터소스에 넣어줌.
                camListBindingSource.ResetBindings(false);
                CamList.SaveCamInfo(Program.CameraList); //CAMLIST 저장(XML파일로)
                tb_CamSerialNumber.Text = ""; //입력창 초기화.
                num_CamNumber.Value++; //번호 1 증가.
            }
        }

        private void Frm_CamSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("카메라 Setting 변경사항을 저장하시겠습니까?", "SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                CamList.SaveCamInfo(Program.CameraList);
            }

            if (MessageBox.Show("프로그램을 재시작 하시겠습니까?", "RESTART", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                DialogResult = DialogResult.No;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }

    //CAMLIST 클래스 생성
    public class CamList
    {
        public CamList() //캠리스트에 들어가는 변수들
        {
            Number = ""; //순서번호
            SerialNum = ""; //카메라 고유번호(시리얼넘버)
        }
        public string Number { get; set; }
        public string SerialNum { get; set; }
        public static bool SaveCamInfo(List<CamList> item) //카메라 정보저장
        {
            bool Result = true;
            try
            {
                //XML 형태 파일로 저장 - 파일을 열어야 할때에는 메모장으로 연결하여 열면됨
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\CamInfo.xml", false, Encoding.UTF8))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<CamList>));
                    xs.Serialize(sw, item);
                    sw.Close();
                }
            }
            catch (Exception)
            {
                Result = false;
            }
            return Result;
        }

        public static List<CamList> LoadCamInfo() //카메라 정보 불러오기. (시리얼넘버)
        {
            List<CamList> Result = new List<CamList>();
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<CamList>));
                StreamReader sr = new StreamReader(Application.StartupPath + "\\CamInfo.xml");
                Result = (List<CamList>)xs.Deserialize(sr);
                sr.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show($"{ee.Message}");
                Application.Exit();
            }
            return Result;
        }
    }
}
