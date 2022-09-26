using ChargeService.DAL.Context;
using ChargeService.DAL.Entities;
using ChargeService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeService.DAL.Repositories.Concrete
{
    public class FillingRepository : RepositoryBase<Filling>, IFillingRepository
    {
        public FillingRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task<long> GetFillingSequence()
        {
            var fillingSequence = nameof(Filling) + "Numbers";
            return await GetSequenceNumber(fillingSequence);
        }
    }
}
