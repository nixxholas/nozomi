using Nozomi.Base.Core.Commands;

namespace Nozomi.Data.Commands.Sources
{
    public abstract class SourceCommand : Command
    {
        public long Id { get; set; }

        // Short form for the currency source if needed.
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string APIDocsURL { get; set; }
    }
}