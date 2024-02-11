using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TemplateService
    {
        private readonly IDBop<Test> _test;
        public TemplateService(IDBop<Test> test) 
        { 
            _test = test;
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
            var product = await _test.Get(id);
            if (product != null)
            {
                await _test.Delete(id);
            }
        }
    }
}
