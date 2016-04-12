using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arches.BusinessLogic.Abstract;
using Arches.Models;
using Arches.DataAccess;
using System.Data;

namespace Arches.BusinessLogic.Concrete
{
    public class TestImpl : ITest
    {
        readonly DatabaseConnection _databaseConnection;
        private DataTable _dt;
        private int _res;

        public TestImpl()
        {
            _databaseConnection = new DatabaseConnection("myConnection");
        }
		
		 public Test Find(Test test)
        {
            _dt = _databaseConnection.Select("Select TOP(1) Col1, Col2, Col3 from Test where Col1=" + test.Col1); // and Col2 = test.Col2...

            if (_dt.Rows.Count > 0)
            {
                Test _test = new Test()
                {
                    Col1 = _dt.Rows[0]["Col1"].ToString(),
                    Col2 = _dt.Rows[0]["Col2"].ToString(),
                    Col3 = _dt.Rows[0]["Col3"].ToString(),
                };

                _dt.Clear();
                return _test;
            }
        }

        public List<Test> FindAll()
        {
            _dt = _databaseConnection.Select("Select Col1, Col2, Col3 from Test");

            
            return (from DataRow row in _dt.Rows
                                        select new Test()
                                        {
                                            Col1 = Convert.ToInt32(row["Col1"]),
                                            Col2 = row["Col2"].ToString(),
                                            Col3 = row["Col3"].ToString()
                                        }).ToList();
        }

        public void Insert(Test test)
        {
            _res = _databaseConnection.ExecQuery("Insert Into Test(Col1, Col2, Col3) values(" + test.Col1 + ",'" + test.Col2 + "','" + test.Col3 + "'");
        }

        public void Update(Test test)
        {
            _res = _databaseConnection.ExecQuery("Update Test Set Col1=" + test.Col1 + ", Col2='" + test.Col2 + "', Col3='" + test.Col3 + "' where Id='"+test.Id+"'");
        }

        public bool Delete(int id)
        {
            _res = _databaseConnection.ExecQuery("Delete From Test Where Id=" + id);
            return _res > 0;
        }
    }
}
