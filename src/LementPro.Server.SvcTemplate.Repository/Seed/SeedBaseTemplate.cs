using LementPro.Server.Common.Repository.Seed.Abstract;
using LementPro.Server.Common.Repository.Seed.Attributes;
using LementPro.Server.SvcTemplate.Repository.Context;

namespace LementPro.Server.SvcTemplate.Repository.Seed
{
    /// <summary>
    /// Common seed template
    /// </summary>
    [Seed("base")]
    public class SeedBaseTemplate : ISeedTemplate
    {
        private readonly DataDbContext _context;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="ctx"></param>
        public SeedBaseTemplate(DataDbContext ctx)
        {
            _context = ctx;
        }

        public void Start()
        {
            SeedCommon();
        }

        /// <inheritdoc />
        private void SeedCommon()
        {

        }

        
    }
}