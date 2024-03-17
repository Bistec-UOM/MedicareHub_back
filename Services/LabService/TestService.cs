using DataAccessLayer;
using Models;
using Models.DTO.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public class TestService : ITestService
    {
        private readonly IRepository<Test> _test;
        private readonly IRepository<ReportFields> _tmplt;
        public TestService(IRepository<Test> test,IRepository<ReportFields> tmplt)
        {
            _test = test;
            _tmplt = tmplt;
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            return await _test.GetAll();
        }

        public async Task<Test> GetTest(int id)
        {
            return await _test.Get(id);
        }

        public async Task Addtest(Test item)
        {
            await _test.Add(item);
        }

        public async Task EditTest(Test item)
        {
            await _test.Update(item);
        }

        public async Task DeleteTest(int id)
        {
            var x = await _test.Get(id);
            if (x != null)
            {
                await _test.Delete(id);
            }
        }

        public async Task<IEnumerable<ReportFields>> GetAllFields()
        {
            return await _tmplt.GetAll();
        }

        public async Task<ReportFields> GetField(int id)
        {
            return await _tmplt.Get(id);
        }

        public async Task<IEnumerable<ReportFields>> GetFieldByTest(int id)
        {
            return await _tmplt.GetByProp("TestId", id);
        }

        public async Task AddTemplate(TemplateObj data)
        {
            var x = new Test
            {
                TestName = data.TestName,
                Abb = data.Abb,
                Price = data.Price,
                Provider = data.Provider,
            };
            await _test.Add(x);

            var Id = x.Id;

            foreach (var i in data.ReportFields)
            {
                await _tmplt.Add(new ReportFields
                {
                    Fieldname = i.Fieldname,
                    Index = i.Index,
                    MinRef = i.MinRef,
                    MaxRef = i.MaxRef,
                    Unit = i.Unit,
                    TestId = Id
                });
            }
        }

        public async Task EditField(ReportFields item)
        {
            await _tmplt.Update(item);
        }

        public async Task EditTemplate(EdittemplateObj data)
        {
            var existingF = await _tmplt.GetByProp("TestId", data.TestId);

            //delete all existing fields
            foreach (var item in existingF)
            {
                await _tmplt.Delete(item.Id);
            }

            //Add all sent fields
            foreach (var item in data.Fields)
            {
                await _tmplt.Add(new ReportFields
                {
                    Fieldname = item.Fieldname,
                    Index = item.Index,
                    MinRef = item.MinRef,
                    MaxRef = item.MaxRef,
                    Unit = item.Unit,
                    TestId = data.TestId
                });
            }
        }

        public async Task UpdateFields(List<ReportFields> data)
        {
            foreach (var i in data)
            {
                await _tmplt.Update(i);
            }
        }

        public async Task DeleteField(int id)
        {
            var x = await _tmplt.Get(id);
            if (x != null)
            {
                await _tmplt.Delete(id);
            }
        }
    }
}
