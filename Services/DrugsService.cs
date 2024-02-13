using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DrugsService
    {
        private readonly IRepository<Drug> _drg;
        public DrugsService(IRepository<Drug> drg)
        {
            _drg = drg;
        }

        public async Task Adddrug(Test item)
        {
            await _drg.Add(item);
        }

    }
}
