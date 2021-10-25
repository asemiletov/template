using System;
using System.Collections.Generic;
using System.Reflection;
using LementPro.Server.Common;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.Common.Repository.Attributes;
using LementPro.Server.Common.Repository.Extensions;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Entities;
using Newtonsoft.Json;

namespace LementPro.Server.SvcTemplate.Repository.Seed.ByServices
{
    [Seed("services_blockwork")]
    public class SeedBlockWorkTemplate : ISeedTemplate
    {
        private readonly DataDbContext _context;

        public SeedBlockWorkTemplate(DataDbContext ctx)
        {
            _context = ctx;
        }

        public void Start()
        {
            var now = DateTimeOffset.UtcNow;

            #region BlockWork

            var items = JsonConvert.DeserializeObject<List<BlockWorkEntity>>(EmbeddedResourceReader.Read(Assembly.GetAssembly(typeof(SeedBlockWorkTemplate)), "Seed.Data.BlockWork.Common.json"));

            items.ForEach(x => x.DateCreated = x.DateUpdated = now);

            _context.AddOrUpdate(x => x.Id, items.ToArray());

            #endregion
        }
    }
}