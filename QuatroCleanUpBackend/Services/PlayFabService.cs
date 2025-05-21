using PlayFab;
using PlayFab.ClientModels;

namespace QuatroCleanUpBackend.Services
{
    public class PlayFabService
    {
        public async Task<string> RegisterUserWithPlayFab(string username, string email)
        {
            var request = new RegisterPlayFabUserRequest
            {
                Username = username,
                Email = email,
                RequireBothUsernameAndEmail = false
            };

            try
            {
                var result = await PlayFabClientAPI.RegisterPlayFabUserAsync(request);
                return result.Result.PlayFabId;
            }
            catch (PlayFabException ex)
            {
                throw new Exception($"Faild to register user: {ex.Message}");

            }
        }
    }
}

