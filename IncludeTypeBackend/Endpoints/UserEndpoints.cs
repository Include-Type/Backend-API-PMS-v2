namespace IncludeTypeBackend.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpoints = builder.MapGroup("user");

        endpoints.MapGet("/", ([FromServices] UserService userService) => GetAllUsers(userService))
        .WithName("GetAllUsers")
        .WithDescription("Get all current users.")
        .WithOpenApi();

        endpoints.MapPost(
            "/adduser",
            ([FromServices] UserService userService, [FromBody] User user) => AddUser(userService, user)
        )
        .WithName("AddUser")
        .WithDescription("Add a new user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getuser/{key}",
            ([FromServices] UserService userService, [FromRoute] string key) => GetUser(userService, key)
        )
        .WithName("GetUser")
        .WithDescription("Get a user's complete details.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getuserprofessionalprofile/{userId}",
            ([FromServices] UserService userService, [FromRoute] string userId) =>
                GetUserProfessionalProfile(userService, userId)
        )
        .WithName("GetUserProfessionalProfile")
        .WithDescription("Get a user's professional profile.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getuserprivacyprofile/{userId}",
            ([FromServices] UserService userService, [FromRoute] string userId) =>
                GetUserPrivacyProfile(userService, userId)
        )
        .WithName("GetUserPrivacyProfile")
        .WithDescription("Get a user's privacy profile.")
        .WithOpenApi();

        endpoints.MapPut(
            "/updateuser/{key}",
            ([FromServices] UserService userService, [FromRoute] string key, [FromBody] User user) =>
                UpdateUser(userService, key, user)
        )
        .WithName("UpdateUser")
        .WithDescription("Update a user's details.")
        .WithOpenApi();

        endpoints.MapPut(
            "/updateuserprofessionalprofile/{key}",
            ([FromServices] UserService userService,
                [FromRoute] string key,
                [FromBody] ProfessionalProfile profile) =>
                UpdateUserProfessionalProfile(userService, key, profile)
        )
        .WithName("UpdateUserProfessionalProfile")
        .WithDescription("Update a user's professional profile.")
        .WithOpenApi();

        endpoints.MapPut(
            "/updateuserprivacyprofile/{key}",
            ([FromServices] UserService userService, [FromRoute] string key, [FromBody] Privacy privacy) =>
                UpdateUserPrivacyProfile(userService, key, privacy)
        )
        .WithName("UpdateUserPrivacyProfile")
        .WithDescription("Update a user's privacy profile.")
        .WithOpenApi();

        endpoints.MapDelete(
            "/deleteuser/{key}",
            ([FromServices] UserService userService, [FromRoute] string key) => DeleteUser(userService, key)
        )
        .WithName("DeleteUser")
        .WithDescription("Delete a user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/checkforuser/{key}",
            ([FromServices] UserService userService, [FromRoute] string key) => CheckForUser(userService, key)
        )
        .WithName("CheckForUser")
        .WithDescription("Check if a user is there in the system or not.")
        .WithOpenApi();

        endpoints.MapGet(
            "/checkpassword/{keyWithPassword}",
            ([FromServices] UserService userService, [FromRoute] string keyWithPassword) =>
                CheckPassword(userService, keyWithPassword)
        )
        .WithName("CheckPassword")
        .WithDescription("Check the password of a user.")
        .WithOpenApi();

        endpoints.MapPost(
            "/register",
            ([FromServices] UserService userService, [FromBody] User user) => Register(userService, user)
        )
        .WithName("Register")
        .WithDescription("Register a new user.")
        .WithOpenApi();

        endpoints.MapPost(
            "/login",
            ([FromServices] UserService userService,
                JwtService jwtService,
                HttpContext httpContext,
                [FromBody] UserDto userDto) =>
                Login(userService, jwtService, httpContext, userDto)
        )
        .WithName("Login")
        .WithDescription("Login as a user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/authenticateduser",
            ([FromServices] UserService userService, JwtService jwtService, HttpContext httpContext) =>
                AuthenticatedUser(userService, jwtService, httpContext)
        )
        .WithName("AuthenticatedUser")
        .WithDescription("Get the authenticated user after login.")
        .WithOpenApi();

        endpoints.MapPost("/logout", (HttpContext httpContext) => Logout(httpContext))
        .WithName("Logout")
        .WithDescription("Logout as a user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getallpendinguserverifications",
            ([FromServices] UserService userService) => GetAllPendingUserVerifications(userService)
        )
        .WithName("GetAllPendingUserVerifications")
        .WithDescription("Get all the user verifications that are pending.")
        .WithOpenApi();

        endpoints.MapPost(
            "/requestpasswordreset",
            ([FromServices] UserService userService,
                [FromServices] EmailService emailService,
                [FromBody] UserVerificationRequestDto verificationRequestDto) =>
                RequestPasswordReset(userService, emailService, verificationRequestDto)
        )
        .WithName("RequestPasswordReset")
        .WithDescription("Request a password reset link.")
        .WithOpenApi();

        endpoints.MapPost(
            "/authorizepasswordreset",
            ([FromServices] UserService userService, [FromBody] UserVerificationDto verificationDto) =>
                AuthorizePasswordReset(userService, verificationDto)
        )
        .WithName("AuthorizePasswordReset")
        .WithDescription("Authorize a password reset request.")
        .WithOpenApi();

        return endpoints;
    }

    private static async Task<IResult> GetAllUsers(UserService userService) =>
        TypedResults.Ok(await userService.GetAllUsersAsync());

    private static async Task<IResult> AddUser(UserService userService, User user)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            user.Id = $"{guid}";
            user.Password = HashPassword(user.Password);
            await userService.AddUserAsync(user);
            return TypedResults.Ok("User successfully added.");
        }
        catch
        {
            return TypedResults.BadRequest($"Invalid user credentials!");
        }
    }

    private static async Task<IResult> GetUser(UserService userService, string key)
    {
        try
        {
            return TypedResults.Ok(await userService.GetCompleteUserAsync(key));
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> GetUserProfessionalProfile(UserService userService, string userId)
    {
        try
        {
            User user = await userService.GetUserByIdAsync(userId);
            return TypedResults.Ok(await userService.GetUserProfessionalProfileAsync(user.Id));
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> GetUserPrivacyProfile(UserService userService, string userId)
    {
        try
        {
            User user = await userService.GetUserByIdAsync(userId);
            return TypedResults.Ok(await userService.GetUserPrivacyProfileAsync(user.Id));
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(UserService userService, string key, User user)
    {
        try
        {
            User existingUser = await userService.GetUserAsync(key);
            if (user.Password.Equals(""))
            {
                user.Password = existingUser.Password;
            }
            else
            {
                user.Password = HashPassword(user.Password);
            }
            await userService.UpdateUserAsync(existingUser, user);
            return TypedResults.Ok("User successfully updated.");
        }
        catch
        {
            return TypedResults.BadRequest($"Invalid user credentials!");
        }
    }

    private static async Task<IResult> UpdateUserProfessionalProfile(UserService userService, string key, ProfessionalProfile profile)
    {
        try
        {
            User existingUser = await userService.GetUserAsync(key);
            ProfessionalProfile existingProfile = await userService.GetUserProfessionalProfileAsync(existingUser.Id);
            await userService.UpdateUserProfessionalProfileAsync(existingProfile, profile);
            return TypedResults.Ok("User Professional Profile successfully updated.");
        }
        catch
        {
            return TypedResults.BadRequest($"Invalid user credentials!");
        }
    }

    private static async Task<IResult> UpdateUserPrivacyProfile(UserService userService, string key, Privacy privacy)
    {
        try
        {
            User existingUser = await userService.GetUserAsync(key);
            Privacy existingPrivacy = await userService.GetUserPrivacyProfileAsync(existingUser.Id);
            await userService.UpdateUserPrivacyProfileAsync(existingPrivacy, privacy);
            return TypedResults.Ok("User Privacy Profile successfully updated.");
        }
        catch
        {
            return TypedResults.BadRequest($"Invalid user credentials!");
        }
    }

    private static async Task<IResult> DeleteUser(UserService userService, string key)
    {
        try
        {
            User user = await userService.GetUserAsync(key);
            await userService.DeleteUserAsync(user);
            return TypedResults.Ok("User successfully deleted.");
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> CheckForUser(UserService userService, string key)
    {
        try
        {
            User user = await userService.GetUserAsync(key);
            return TypedResults.Ok(true);
        }
        catch
        {
            return TypedResults.Ok(false);
        }
    }

    private static async Task<IResult> CheckPassword(UserService userService, string keyWithPassword)
    {
        try
        {
            string[] temp = keyWithPassword.Split('-');
            User requestedUser = await userService.GetUserAsync(temp[0]);
            if (!Verify(temp[1], requestedUser.Password))
            {
                return TypedResults.Ok(false);
            }
            else
            {
                return TypedResults.Ok(true);
            }
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> Register(UserService userService, User user)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            user.Id = $"{guid}";
            user.Password = HashPassword(user.Password);
            await userService.AddUserAsync(user);
            return TypedResults.Ok("User successfully registered.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid user credentials!");
        }
    }

    private static async Task<IResult> Login(UserService userService, JwtService jwtService, HttpContext httpContext, UserDto userDto)
    {
        try
        {
            User requestedUser = await userService.GetUserAsync(userDto.Key);
            if (!Verify(userDto.Password, requestedUser.Password))
            {
                throw new Exception();
            }

            string jwt = jwtService.Generate(requestedUser.Id);
            httpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return TypedResults.Ok("Login Successfull.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid credentials!");
        }
    }

    private static async Task<IResult> AuthenticatedUser(UserService userService, JwtService jwtService, HttpContext httpContext)
    {
        try
        {
            string jwt = httpContext.Request.Cookies["jwt"] ?? throw new Exception("No user is logged in!");
            JwtSecurityToken verifiedToken = jwtService.Verify(jwt);
            string userId = verifiedToken.Issuer;
            CompleteUserDto completeUser = await userService.GetCompleteUserAsync(userId);
            return TypedResults.Ok(completeUser);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static IResult Logout(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies["jwt"] is null)
        {
            return TypedResults.NotFound("No user is logged in!");
        }
        httpContext.Response.Cookies.Delete("jwt", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
        return TypedResults.Ok("Logout Successfull.");
    }

    private static async Task<IResult> GetAllPendingUserVerifications(UserService userService) =>
        TypedResults.Ok(await userService.GetAllPendingUserVerificationsAsync());

    private static async Task<IResult> RequestPasswordReset(UserService userService, EmailService emailService, UserVerificationRequestDto verificationRequestDto)
    {
        try
        {
            User requestedUser = await userService.GetUserAsync(verificationRequestDto.Email);
            string uniqueString = await userService.AddPendingUserVerificationAsync(requestedUser.Id);
            EmailForm emailForm = new()
            {
                ToEmailAddress = verificationRequestDto.Email,
                Subject = @"Reset Password • #include <TYPE>",
                Body = $"""
                        <h4>
                            Click <a href='https://include-type.github.io/reset-password/{requestedUser.Id}/{uniqueString}'>HERE</a> to reset your password.
                        </h4>
                        <p>The above link will <b>expire in 15 mins.</b></p>
                        """
            };

            await emailService.SendEmailAsync(emailForm);
            return TypedResults.Ok("Password reset link has been sent via email.");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> AuthorizePasswordReset(UserService userService, UserVerificationDto verificationDto)
    {
        try
        {
            UserVerification userVerification = await userService.GetPendingUserVerificationAsync(verificationDto.UserId);
            if (userVerification.UserId.Equals("") || !Verify(verificationDto.UniqueString, userVerification.UniqueString))
            {
                throw new Exception("Authentication failed, invalid credentials!");
            }

            if (DateTime.Compare(DateTime.Now, DateTime.Parse(userVerification.ExpirationTime)) > 0)
            {
                throw new Exception("Link has expired!");
            }

            await userService.DeletePendingUserVerificationAsync(verificationDto.UserId);
            await userService.UpdateUserPasswordAsync(verificationDto.UserId, verificationDto.NewPassword);
            return TypedResults.Ok("Password has been reset successfully.");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}