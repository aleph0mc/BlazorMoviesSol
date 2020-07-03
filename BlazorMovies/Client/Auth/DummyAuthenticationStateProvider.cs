using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Auth
{
    public class DummyAuthenticationStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(3000);
            //var anonymous = new ClaimsIdentity(); //Not authorized
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));

            //var dummyUser = new ClaimsIdentity("test"); // Authorized
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(dummyUser)));

            var dummyUserWithClaims = new ClaimsIdentity(
                new List<Claim>()
                {
                    new Claim("key1", "val1"),
                    new Claim(ClaimTypes.Name, "Milko"),
                    new Claim(ClaimTypes.Role, "Admin")
                },
                "test"); // Authorized with claims
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(dummyUserWithClaims)));
        }
    }
}
