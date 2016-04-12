using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arches.Models;

namespace Arches.BusinessLogic.Abstract
{
    public interface ITest
    {
	Test Find(Test test);
        List<Test> FindAll();
        void Insert(Test test);
        void Update(Test test);
        bool Delete(int id);
    }
}
