using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Lab;
using Models.DTO.Lab.EditTemplate;
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
        private readonly ApplicationDbContext _cntx;
        public TestService(IRepository<Test> test,IRepository<ReportFields> tmplt, ApplicationDbContext cntx)
        {
            _test = test;
            _tmplt = tmplt;
            _cntx = cntx;
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
            Test x = await _cntx.tests.Where(e => e.TestName == data.TestName).FirstOrDefaultAsync();
            if (x == null)
            {
                using (var transaction = await _cntx.Database.BeginTransactionAsync())
                {

                    var tst = new Test
                    {
                        TestName = data.TestName,
                        Abb = data.Abb,
                        Price = data.Price,
                        Provider = data.Provider,
                    };

                    await _cntx.tests.AddAsync(tst);
                    await _cntx.SaveChangesAsync();

                    int tstId = tst.Id;

                    foreach (var i in data.ReportFields)
                    {
                        await _cntx.reportFields.AddAsync(new ReportFields
                        {
                            Fieldname = i.Fieldname,
                            Index = i.Index,
                            MinRef = i.MinRef,
                            MaxRef = i.MaxRef,
                            Unit = i.Unit,
                            TestId = tstId
                        });
                    }

                    await _cntx.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
            }
            else
            {
                throw new Exception();
            }

 
        }

        public async Task EditField(ReportFields item)
        {
            await _tmplt.Update(item);
        }

        public async Task EditTemplate(EdittemplateObj data)
        {
            using (var transaction = await _cntx.Database.BeginTransactionAsync())
            {
                foreach (var item in data.Fields)
                {

                    if (item.Stat == "exist")//update existing, protecting the Id
                    {
                        var existingField = await _cntx.reportFields.FindAsync(item.Id);
                        if (existingField != null)
                        {
                            existingField.Fieldname = item.Fieldname;
                            existingField.Index = item.Index;
                            existingField.MinRef = item.MinRef;
                            existingField.MaxRef = item.MaxRef;
                            existingField.Unit = item.Unit;
                            existingField.TestId = data.TestId;

                            _cntx.reportFields.Update(existingField);
                        }
                    }
                    else if (item.Stat == "new")//add new fields
                    {
                        ReportFields x = new ReportFields
                        {
                            Id = 0,
                            Fieldname = item.Fieldname,
                            Index = item.Index,
                            MinRef = item.MinRef,
                            MaxRef = item.MaxRef,
                            Unit = item.Unit,
                            TestId = data.TestId
                        };
                        await _cntx.reportFields.AddAsync(x);
                    }
                    else if(item.Stat == "deleted") 
                    {
                        _cntx.records.RemoveRange(_cntx.records.Where(a=>a.ReportFieldId==item.Id));
                        ReportFields x = new ReportFields
                        {
                            Id = item.Id,
                            Fieldname = item.Fieldname,
                            Index = item.Index,
                            MinRef = item.MinRef,
                            MaxRef = item.MaxRef,
                            Unit = item.Unit,
                            TestId = data.TestId
                        };
                        _cntx.reportFields.Remove(x);
                    }
                }
                await _cntx.SaveChangesAsync();
                await transaction.CommitAsync();

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