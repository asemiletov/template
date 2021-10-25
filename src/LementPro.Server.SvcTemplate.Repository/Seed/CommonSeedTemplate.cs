using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LementPro.Server.Common;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.Common.Repository.Extensions;
using LementPro.Server.SvcTemplate.Common;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Entities;
using LementPro.Server.SvcTemplate.Repository.Extensions;
using Newtonsoft.Json;

namespace LementPro.Server.SvcTemplate.Repository.Seed
{
    /// <summary>
    /// Common seed template
    /// </summary>
    public class CommonSeedTemplate : BaseSeedTemplate, ISeedTemplate
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="ctx"></param>
        public CommonSeedTemplate(DataDbContext ctx) : base(ctx) {}

        /// <inheritdoc />
        public override void Start()
        {
            var now = DateTimeOffset.UtcNow;

            #region BlockWork

            var items = JsonConvert.DeserializeObject<List<BlockWorkEntity>>(
                EmbeddedResourceReader.Read(Assembly.GetAssembly(typeof(CommonSeedTemplate)), "Seed.BlockWork.Common.json"));

            items.ForEach(x => x.DateCreated = x.DateUpdated = now);
            
            Context.AddOrUpdate(x => x.Id, items.ToArray());

            #endregion

        }
    }
}
