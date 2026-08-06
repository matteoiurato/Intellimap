using Intellimap.Core.Knowledge;
using Intellimap.Core.Knowledge.Identity;

namespace Intellimap.Core.Modules.Standard.GitHub
{
    /// <summary>
    /// Result produced by <see cref="GitHubModule"/>.
    /// </summary>
    public class GitHubModuleResult : AbstractModuleResult
    {
        public ProfileKnowledge ProfileKnowledge { get; set; } = null!;

        public override IReadOnlyList<IKnowledge> Knowledge => [
            ProfileKnowledge
        ];
    }
}
