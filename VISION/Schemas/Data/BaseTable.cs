using MvUtils;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace VISION.Schemas.Data
{
    public enum MyBool
    {
        None,
        True,
        False
    }

    public enum 저장구분
    {
        없음,
        신규,
        수정,
        삭제
    }

    public class 저장결과
    {
        public Int32 적용갯수 = -1;
        public 저장구분 저장구분 = 저장구분.없음;
        public string 오류내용;
        public string 오류항목;

        public 저장결과() { }
        public 저장결과(string 항목, string 내용)
        {
            this.오류항목 = 항목;
            this.오류내용 = 내용;
        }
        public 저장결과(저장구분 구분, string 내용)
        {
            this.저장구분 = 구분;
            this.오류내용 = 내용;
        }
        public 저장결과(저장구분 구분, string 항목, string 내용)
        {
            this.저장구분 = 구분;
            this.오류항목 = 항목;
            this.오류내용 = 내용;
        }
    }

    public abstract class BaseTable : DbContext
    {
        protected const String SchemaName = "public";
        protected NpgsqlConnection DbConn = null;

        public BaseTable() : base()
        {
            this.DbConn = 환경설정.CreateDbConnection();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this.DbConn);
        }

        public 저장결과 ExcuteUpdate(저장구분 구분)
        {
            return this.ExcuteUpdate(구분, string.Empty);
        }

        public 저장결과 ExcuteUpdate(저장구분 구분, string 항목)
        {
            return this.ExcuteUpdate(구분, 항목, string.Empty);
        }

        public 저장결과 ExcuteUpdate(저장구분 구분, string 항목, string 오류)
        {
            저장결과 r = new 저장결과() { 저장구분 = 구분, 오류항목 = 항목 };
            try
            {
                r.적용갯수 = this.SaveChanges();
                //Debug.WriteLine(r.적용갯수, 구분.ToString());
                if (r.적용갯수 == 0)
                {
                    if (string.IsNullOrEmpty(오류))
                    {
                        if (구분 == 저장구분.삭제) r.오류내용 = "해당 자료가 없습니다.";
                        else if (구분 == 저장구분.수정) r.오류내용 = "해당 자료가 없거나 변경사항이 없습니다.";
                        else if (구분 == 저장구분.신규) r.오류내용 = "신규 자료 등록에 실패하였습니다.";
                        else r.오류내용 = "알수 없는 오류가 발생하였습니다.";
                    }
                    else
                        r.오류내용 = 오류;
                }
            }
            catch (Exception ex)
            {
                r.오류내용 = ex.Message;
                Utils.DebugException(ex, 1, "SaveChanges");
            }
            return r;
        }

        public static List<string> GetColumnNames(Type Table)
        {
            List<string> Columns = new List<string>();
            foreach (System.Reflection.PropertyInfo 속성 in Table.GetProperties())
            {
                ColumnAttribute a = (ColumnAttribute)속성.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
                if (a != null)
                    Columns.Add(a.Name);
            }
            return Columns;
        }
    }
}
