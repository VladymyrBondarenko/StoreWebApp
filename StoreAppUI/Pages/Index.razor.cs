using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using StoreAppUI;
using StoreAppUI.Shared;

namespace StoreAppUI.Pages
{
    public partial class Index
    {
        private UserModel loggedInUser;

        protected override async Task OnInitializedAsync()
        {
            await loadAndVerifyUser();
        }

        private async Task loadAndVerifyUser()
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var authUserClaims = authState.User.Claims;
            var objectId = authUserClaims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
            if (!string.IsNullOrWhiteSpace(objectId))
            {
                loggedInUser = await userData.GetUser(objectId) ?? new();
                string firstName = authUserClaims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value;
                string lastName = authUserClaims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value;
                string displayName = authUserClaims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
                string email = authUserClaims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
                var isAuthDataChanged = false;
                if (!objectId.Equals(loggedInUser.ObjectIdentifier))
                {
                    loggedInUser.ObjectIdentifier = objectId;
                    isAuthDataChanged = true;
                }

                if (!firstName.Equals(loggedInUser.FirstName))
                {
                    loggedInUser.FirstName = firstName;
                    isAuthDataChanged = true;
                }

                if (!lastName.Equals(loggedInUser.LastName))
                {
                    loggedInUser.LastName = lastName;
                    isAuthDataChanged = true;
                }

                if (!displayName.Equals(loggedInUser.DisplayName))
                {
                    loggedInUser.DisplayName = displayName;
                    isAuthDataChanged = true;
                }

                if (!email.Equals(loggedInUser.EmailAddress))
                {
                    loggedInUser.EmailAddress = email;
                    isAuthDataChanged = true;
                }

                if (isAuthDataChanged)
                {
                    if (loggedInUser.UserId == 0)
                    {
                        await userData.CreateUser(loggedInUser);
                    }
                    else
                    {
                        await userData.UpdateUser(loggedInUser);
                    }
                }
            }
        }
    }
}