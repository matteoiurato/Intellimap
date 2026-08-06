using Intellimap.Core.Exceptions;
using Intellimap.Core.Infrastructure.Http;
using Intellimap.Core.Knowledge.Identity;
using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets.Generic;
using System.Net;
using System.Net.Http.Json;

namespace Intellimap.Core.Modules.Standard.GitHub
{
    /// <summary>
    /// Resolves public profile information for a username using the GitHub user API.
    /// </summary>
    public sealed class GitHubModule : AbstractModule<GitHubModuleResult>
    {
        public override string ModuleId => "github";

        public override IReadOnlyCollection<Type> SupportedTargetTypes =>
            [
                typeof(UsernameTarget)
            ];

        public override IReadOnlyCollection<Type> ProducedKnowledgeTypes =>
            [
                typeof(ProfileKnowledge)
            ];

        protected override async Task<GitHubModuleResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            if (context.Target is not UsernameTarget usernameTarget)
                throw new InvalidModuleTargetException(ModuleId, context.Target.GetType());

            var httpOptions = context.Options as IHttpOptions;
            var client = HttpClientCache.Get(httpOptions);

            var profileKnowledge = await FetchProfileAsync(client, usernameTarget.Username, context, cancellationToken);

            if (profileKnowledge.AvatarUrl is { Length: > 0 })
                await FetchAvatarAsync(client, profileKnowledge, context, cancellationToken);

            var result = CreateResult();
            result.ProfileKnowledge = profileKnowledge;
            return result;
        }

        /// <summary>
        /// Fetches the primary profile data for a username. GitHub responds with 404 for a username that doesn't exist,
        /// which is treated as a valid empty result rather than a failure.
        /// </summary>
        private async Task<ProfileKnowledge> FetchProfileAsync(HttpClient client, string username, ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            var url = $"https://api.github.com/users/{Uri.EscapeDataString(username)}";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd("Intellimap");

                using var httpResponse = await client.SendAsync(request, cancellationToken);

                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return new ProfileKnowledge { Username = username };

                httpResponse.EnsureSuccessStatusCode();

                var response = await httpResponse.Content.ReadFromJsonAsync<GitHubUserResponse>(cancellationToken);
                if (response is null)
                    throw new ModuleExecutionException(ModuleId, "No response from service");

                return new ProfileKnowledge
                {
                    Username = response.Login,
                    DisplayName = response.Name,
                    Bio = response.Bio,
                    Company = response.Company,
                    Location = response.Location,
                    Website = response.Blog,
                    AvatarUrl = response.AvatarUrl,
                    CreatedAt = response.CreatedAt
                };
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModuleExecutionException(ModuleId, "Service call failed", ex);
            }
        }

        /// <summary>
        /// Fetches the avatar image referenced by the profile. Optional: any failure is swallowed and leaves
        /// the image fields unset, since a missing picture shouldn't invalidate the rest of the profile.
        /// </summary>
        private static async Task FetchAvatarAsync(HttpClient client, ProfileKnowledge profileKnowledge, ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            try
            {
                using var httpResponse = await client.GetAsync(profileKnowledge.AvatarUrl, cancellationToken);
                httpResponse.EnsureSuccessStatusCode();

                profileKnowledge.AvatarImage = await httpResponse.Content.ReadAsByteArrayAsync(cancellationToken);
                profileKnowledge.AvatarImageContentType = httpResponse.Content.Headers.ContentType?.MediaType;
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                // The avatar is optional context, not core profile data - a failure here shouldn't fail the module.
            }
        }
    }
}
