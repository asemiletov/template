using System;
using System.Collections.Generic;
using System.Text;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Context;

namespace LementPro.Server.SvcTemplate.Repository.Seed
{
    /// <summary>
    /// Base seed template
    /// </summary>
    public abstract class BaseSeedTemplate : ISeedTemplate
    {
        protected DataDbContext Context;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="ctx"></param>
        public BaseSeedTemplate(DataDbContext ctx)
        {
            Context = ctx;
        }

        /// <inheritdoc />
        public abstract void Start();
    }
}
