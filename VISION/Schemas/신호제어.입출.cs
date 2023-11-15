using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public partial class 신호제어
    {
        //public event Global.BaseEvent 동작상태알림;
        //public event Global.BaseEvent 검사시작알림;
        public event Global.BaseEvent 입출변경알림;

        // 로컬 변수
        //private String 로그영역 = "신호제어";
        private const Int32 IRQNO = 7; // 입력, 출력 보드 IRQ
        private 모듈정보 입력모듈 = null;
        private 모듈정보 출력모듈 = null;

        #region IO 관련 변수 및 Propertys
        private 입력신호분석 입력자료 = new 입력신호분석();
        private 출력신호분석 출력자료 = new 출력신호분석();

        public enum 입력번호
        {

        }

        public enum 출력번호
        {

        }

        public Boolean 동작여부 = false;
        public Int32 실행주기 = 5;

        #endregion


        public String Init()
        {
            try
            {
                //HW Reset될 경우 현 위치가 0으로 바뀌므로 AxlOpen에서 AxlOpenNoReset으로 수정함. by LHD - 22.12.21
                if (CAXL.AxlOpenNoReset(IRQNO) != (UInt32)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return "Driver Open Error!";
                //if (CAXL.AxlOpen(IRQNO) != (UInt32)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return "Driver Open Error!";
                UInt32 status = 0;
                if (CAXD.AxdInfoIsDIOModule(ref status) != (UInt32)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return "IO Module not exist.";
                if ((AXT_EXISTENCE)status != AXT_EXISTENCE.STATUS_EXIST) return "IO Module not exist.";
                Int32 boardNo = 0, modulePos = 0;
                UInt32 moduleID = 0;

                CAXD.AxdInfoGetModule(0, ref boardNo, ref modulePos, ref moduleID);
                this.입력모듈 = new 모듈정보(0, modulePos, boardNo, moduleID);
                CAXD.AxdInfoGetModule(1, ref boardNo, ref modulePos, ref moduleID);
                this.출력모듈 = new 모듈정보(1, modulePos, boardNo, moduleID);

                //장치초기화();

                if (입출상태갱신())
                    this.입출변경알림?.Invoke();
            }
            catch (Exception ex)
            {
                return "IO, Motion 모듈 초기화에 실패하였습니다. 확인 후 다시 실행 시켜 주세요.\n" + ex.Message;
            }

            return String.Empty;
        }

        private Boolean 입출상태갱신()
        {
            UInt32 iVal = 0;
            UInt32 oVal = 0;
            CAXD.AxdiReadInportDword(this.입력모듈.모듈번호, 0, ref iVal);
            CAXD.AxdoReadOutportDword(this.출력모듈.모듈번호, 0, ref oVal);
            BitArray 입력 = new BitArray(BitConverter.GetBytes(iVal));
            BitArray 출력 = new BitArray(BitConverter.GetBytes(oVal));
            Dictionary<입력번호, Boolean> 입력변경 = this.입력자료.Set(입력); // 입력 변경내용 반환
            Dictionary<출력번호, Boolean> 출력변경 = this.출력자료.Set(출력); // 출력 변경내용 반환
            Boolean 변경 = 입력변경.Count > 0 || 출력변경.Count > 0;
            //if (!this.동작여부 || !this.파워상태) return 변경;

            //if (입력변경.ContainsKey(입력번호.리셋버튼) && 입력변경[입력번호.리셋버튼])
            //{
            //    this.장치리셋();
            //    return 변경;
            //}

            //// 상태 이상 일 경우
            //if (!장치정상체크(false))
            //{
            //    if (!this.정지램프) this.장치정지();
            //    if (!this.알람부저) this.알람부저 = true;
            //    //장치정상체크(true);
            //    _ = CheckDevice();
            //    return 변경;
            //}

            //// 정지버튼 True로 변경 입력 시
            //if (입력변경.ContainsKey(입력번호.정지버튼) && 입력변경[입력번호.정지버튼]) this.장치정지();
            //// Auto, Manual 변경 시 일단 Stop
            //else if (입력변경.ContainsKey(입력번호.동작구분)) this.장치정지();
            //// 시작버튼 True로 변경 입력 시
            //else if (입력변경.ContainsKey(입력번호.시작버튼) && 입력변경[입력번호.시작버튼]) this.장치시작();

            //if (입력변경.ContainsKey(입력번호.검사시작) && 입력변경[입력번호.검사시작] && this.동작구분)
            //    if (!this.작업수행여부) Task.Run(() => this.검사수행());

            return 변경;
        }



        #region Local Class
        public class 모듈정보
        {
            public Int32 모듈번호 { get; set; }
            public Int32 모듈위치 { get; set; } = 0;
            public Int32 보드번호 { get; set; } = 0;
            public UInt32 모듈코드 { get; set; } = 0;

            public 모듈정보(Int32 번호, Int32 위치, Int32 보드, UInt32 ID)
            {
                this.모듈번호 = 번호;
                this.모듈위치 = 위치;
                this.보드번호 = 보드;
                this.모듈코드 = ID;
                Debug.WriteLine($"번호={this.모듈번호}, ID={this.모듈코드}, 위치={this.모듈위치}, 보드={this.보드번호}", "IO Board");
            }
        }

        private class 신호정보
        {
            public Boolean 이전 { get; set; } = false;
            public Boolean 현재 { get; set; } = false;
            public Boolean 변경 { get { return 이전 != 현재; } }

            public Boolean Set(Boolean 상태)
            {
                이전 = 현재;
                현재 = 상태;
                return 변경;
            }
        }

        private class 입력신호분석 : Dictionary<입력번호, 신호정보>
        {
            public 입력신호분석()
            {
                foreach (입력번호 번호 in Enum.GetValues(typeof(입력번호)))
                    this.Add(번호, new 신호정보());
            }

            public Dictionary<입력번호, Boolean> Set(BitArray 입력)
            {
                Dictionary<입력번호, Boolean> 변경 = new Dictionary<입력번호, Boolean>();
                foreach (var 정보 in this)
                {
                    Int32 번호 = (int)정보.Key;
                    Boolean 상태 = 입력[번호];
                    if (정보.Value.Set(상태))
                        변경.Add(정보.Key, 상태);
                }
                return 변경;
            }

            public Boolean Get(입력번호 번호)
            {
                if (this.ContainsKey(번호)) return this[번호].현재;
                return false;
            }
        }

        private class 출력신호분석 : Dictionary<출력번호, 신호정보>
        {
            public 출력신호분석()
            {
                foreach (출력번호 번호 in Enum.GetValues(typeof(출력번호)))
                    this.Add(번호, new 신호정보());
            }

            public Dictionary<출력번호, Boolean> Set(BitArray 출력)
            {
                Dictionary<출력번호, Boolean> 변경 = new Dictionary<출력번호, Boolean>();
                foreach (var 정보 in this)
                {
                    Int32 번호 = (int)정보.Key;
                    Boolean 상태 = 출력[번호];
                    if (정보.Value.Set(상태))
                        변경.Add(정보.Key, 상태);
                }
                return 변경;
            }

            public Boolean Get(출력번호 번호)
            {
                if (this.ContainsKey(번호)) return this[번호].현재;
                return false;
            }
        }
        #endregion
    }
}
