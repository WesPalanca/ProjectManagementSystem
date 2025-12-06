using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Auth;

public interface IAuthenticationController
{
    User LogIn();
    User Register();
}