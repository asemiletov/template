using System;
using LementPro.Server.SvcTemplate.Common.Enums;

namespace LementPro.Server.SvcTemplate.Sdk.Models.BlockWork
{
    /// <summary>
    /// Model sample
    /// </summary>
    public class BlockWorkModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public BlockWorkStatus Status { get; set; }

        /// <summary>
        /// DateCreated
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }
    }

    /// <summary>
    /// Model sample
    /// </summary>
    public class BlockWorkModelSimple
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public BlockWorkStatus Status { get; set; }

        /// <summary>
        /// DateCreated
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }
    }
}
