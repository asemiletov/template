using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Seed;
using Microsoft.EntityFrameworkCore;

namespace LementPro.Server.SvcTemplate.Repository.Extensions
{
    public static class SvcTemplateSeedExtensions
    {
        public static void EnsureSeedData(string template, DbContext ctx)
        {
            var tpl = GetSeedTemplate(template, ctx);
            tpl.Start();
        }

        private static ISeedTemplate GetSeedTemplate(string template, DbContext ctx) =>
            (template, ctx.GetType().Name) switch
            {
                ("common", nameof(DataDbContext)) => new CommonSeedTemplate(ctx as DataDbContext),
                _ => throw new NotImplementedException(
                    $"Seed template not implemented '{template}' ctx '{ctx.GetType().Name}'")
            };
    }
}
