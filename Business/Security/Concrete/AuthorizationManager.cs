using Business.Security.Abstract;
using Business.Security.Models;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Concrete
{
    public class AuthorizationManager : IAuthorizationService
    {
        private readonly ContextDb _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthorizationManager(ContextDb context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        public async Task<IDataResult<LoginResponseModel>> Login(LoginModel loginModel)
        {
            LoginResponseModel loginResponseModel = new LoginResponseModel();

            try
            {
                //if (ModelState.IsValid == false)
                //{
                //    loginResponseModel.Status = false;
                //    loginResponseModel.Message = "Try Again.";
                //    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                //}

                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user == null)
                {
                    loginResponseModel.Status = false;
                    loginResponseModel.Message = "Try Again.";
                    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                if (signInResult.Succeeded == false)
                {
                    loginResponseModel.Status = false;
                    loginResponseModel.Message = "User name or password is incorrect.";

                    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                }

                loginResponseModel.Status = true;
                loginResponseModel.Message = "User is Logged in.";
                loginResponseModel.UserId = user.Id;
                loginResponseModel.Email = loginModel.Email;
                loginResponseModel.UserName = user.UserName;


                return new SuccessDataResult<LoginResponseModel>(loginResponseModel);
            }
            catch (Exception ex)
            {
                loginResponseModel.Status = false;
                loginResponseModel.Message = ex.Message;

                return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
            }
        }

        public async Task<IDataResult<RegisterResponseModel>> RegisterAdmin(RegisterModel registerModel)
        {
            RegisterResponseModel registerResponseModel = new RegisterResponseModel();

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    registerResponseModel.Status = false;
                //    registerResponseModel.Message = "Try Again.";

                //    return BadRequest(registerResponseModel);
                //}

                User existsUser = await _userManager.FindByEmailAsync(registerModel.Email);

                if (existsUser != null)
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = "This user already exists.";

                    return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
                }

                User user = new User();

                user.UserName = registerModel.UserName;
                user.Email = registerModel.Email;
                user.UserRole = "Admin";


                var result = await _userManager.CreateAsync(user, registerModel.Password.Trim());


                if (result.Succeeded)
                {
                    bool roleExists1 = await _roleManager.RoleExistsAsync(_config["Roles:Admin"]);
                    bool roleExists2 = await _roleManager.RoleExistsAsync(_config["Roles:User"]);

                    if (!roleExists1)
                    {
                        IdentityRole role1 = new IdentityRole(_config["Roles:Admin"]);
                        role1.NormalizedName = _config["Roles:Admin"];

                        await _roleManager.CreateAsync(role1);
                    }
                    if (!roleExists2)
                    {
                        IdentityRole role2 = new IdentityRole(_config["Roles:User"]);
                        role2.NormalizedName = _config["Roles:User"];

                        await _roleManager.CreateAsync(role2);
                    }

                    //await _userManager.AddToRoleAsync(user, _config["Roles:Admin"]);
                    //await _userManager.AddToRoleAsync(user, _config["Roles:User"]);

                    registerResponseModel.Email = user.Email;
                    registerResponseModel.UserName = user.UserName;
                    registerResponseModel.UserId = user.Id;
                    registerResponseModel.Message = "User is created successfully";
                    registerResponseModel.Status = true;
                    registerResponseModel.Role = user.UserRole;


                    return new SuccessDataResult<RegisterResponseModel>(registerResponseModel);
                }
                else
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = $"An error occured while user creating. {result.Errors.FirstOrDefault().Description}";
                    return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
                }

            }
            catch (Exception ex)
            {
                registerResponseModel.Status = false;
                registerResponseModel.Message = ex.Message;

                return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
            }
        }

        public async Task<IDataResult<RegisterResponseModel>> RegisterUser(RegisterModel registerModel)
        {
            RegisterResponseModel registerResponseModel = new RegisterResponseModel();

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    registerResponseModel.Status = false;
                //    registerResponseModel.Message = "Try Again.";

                //    return BadRequest(registerResponseModel);
                //}

                User existsUser = await _userManager.FindByEmailAsync(registerModel.Email);

                if (existsUser != null)
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = "This user already exists.";

                    return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
                }
                
                User user = new User();

                user.UserName = registerModel.UserName;
                user.Email = registerModel.Email;
                user.UserRole = "User";


                var result = await _userManager.CreateAsync(user, registerModel.Password.Trim());


                if (result.Succeeded)
                {
                    bool roleExists = await _roleManager.RoleExistsAsync(_config["Roles:User"]);


                    if (!roleExists)
                    {
                        IdentityRole role = new IdentityRole(_config["Roles:User"]);
                        role.NormalizedName = _config["Roles:User"];

                        await _roleManager.CreateAsync(role);
                    }

                    //foreach (var role in _roleManager.Roles)
                    //{
                    //    await Console.Out.WriteLineAsync(role.Name);
                    //}

                    //await Console.Out.WriteLineAsync(user.UserName);

                    //await _userManager.AddToRoleAsync(user,"User");

                    var currentUser = await _userManager.FindByEmailAsync(user.Email);

                    currentUser = await _userManager.FindByEmailAsync(user.Email);
                    //var authRoles = (await _userManager.GetRolesAsync(currentUser));

                    registerResponseModel.Email = user.Email;
                    registerResponseModel.UserName = user.UserName;
                    registerResponseModel.UserId = user.Id;
                    registerResponseModel.Message = "User is created successfully";
                    registerResponseModel.Status = true;
                    registerResponseModel.Role = user.UserRole;


                    return new SuccessDataResult<RegisterResponseModel>(registerResponseModel);
                }
                else
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = $"An error occured while user creating. {result.Errors.FirstOrDefault().Description}";
                    return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
                }

            }
            catch (Exception ex)
            {
                registerResponseModel.Status = false;
                registerResponseModel.Message = ex.Message;

                return new ErrorDataResult<RegisterResponseModel>(registerResponseModel);
            }
        }

        IDataResult<AuthMeResponseModel> IAuthorizationService.AuthMe(string token)
        {
            AuthMeResponseModel authMeResponseModel = new AuthMeResponseModel();

            var user = from tkn in _context.UserTokens
                        join usr in _userManager.Users
                        on tkn.UserId equals usr.Id
                        where tkn.Value == token
                        select usr ;

            List<string> roleList = new List<string>();

            foreach (var item in _userManager.GetRolesAsync(user.First()).Result.ToList())
            {
                roleList.Add(item);
            }

            roleList = roleList.Distinct().ToList();

            var model = 
                       from u in _context.Users
                       join t in _context.UserTokens
                       on u.Id equals t.UserId
                       where t.Value == token
                       select new AuthMeResponseModel()
                       {
                           UserId = u.Id,
                           Email = u.Email,
                           UserName = u.UserName,
                           Roles = roleList
                       };

            authMeResponseModel = model.First();

            return new SuccessDataResult<AuthMeResponseModel>(authMeResponseModel);
        }
    }
}
