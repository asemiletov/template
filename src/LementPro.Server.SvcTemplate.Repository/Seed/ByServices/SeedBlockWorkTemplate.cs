using LementPro.Server.Common;
using LementPro.Server.Common.Repository.Seed.Abstract;
using LementPro.Server.Common.Repository.Seed.Attributes;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using LementPro.Server.Common.Repository.Extension;
using LementPro.Server.Common.Serializer.Concrete;

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

            var items = JsonHelper.Deserialize<List<BlockWorkEntity>>(EmbeddedResourceReader.Read(Assembly.GetAssembly(typeof(SeedBlockWorkTemplate)), "Seed.Data.BlockWork.Common.json"));

            items.ForEach(x => x.DateCreated = x.DateUpdated = now);

            _context.AddOrUpdate(x => x.Id, items.ToArray());

            #endregion
        }
    }
}