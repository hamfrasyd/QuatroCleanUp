using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuatroCleanUpBackend
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

