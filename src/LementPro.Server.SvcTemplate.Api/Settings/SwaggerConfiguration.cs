using System.Collections.Generic;
using System.Reflection;
using LementPro.Server.Common;

namespace LementPro.Server.SvcTemplate.Api.Settings
{
    public class SwaggerConfiguration : List<SwaggerConfiguration.SwaggerDoc>
    {
        /// <summary>
        /// Doc assembly description
        /// </summary>
        public class SwaggerDoc
        {
            /// <summary>
            /// Assembly name
            /// </summary>
            public string Assembly { get; private set; }

            /// <summary>
            /// Assembly doc sections
            /// </summary>
            public IEnumerable<DocSection> Sections { get; private set; }
        }

        /// <summary>
        /// Doc section description 
        /// </summary>
        public class DocSection
        {
            /// <summary>
            /// Section name
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Section description
            /// </summary>
            public string Description { get; private set; }

            public string DescriptionFile
            {
                //get => throw new NotImplementedException();
                get => string.Empty;
                set => Description = EmbeddedResourceReader.Read(Assembly.GetExecutingAssembly(), value);
            }
        }
    }
}