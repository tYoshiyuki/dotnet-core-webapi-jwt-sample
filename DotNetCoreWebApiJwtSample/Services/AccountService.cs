using System;
using System.Linq;
using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DotNetCoreWebApiJwtSample.Services.Interfaces;
using System.Collections.Generic;

namespace DotNetCoreWebApiJwtSample.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        public AccountService(UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// ユーザを作成します
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(UserRequestModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await _userManager.CreateAsync(new IdentityUser { UserName = user.UserName, Email = user.Email }, user.Password);
        }

        /// <summary>
        /// ユーザを作成します
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(string email)
        {
            return await _userManager.CreateAsync(new IdentityUser { UserName = email, Email = email }, "password");
        }

        /// <summary>
        /// ユーザの一覧を取得します
        /// </summary>
        /// <returns></returns>
        public List<IdentityUser> GetList()
        {
            return _userManager.Users.ToList();
        }

        /// <summary>
        /// ユーザにロールを追加します
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> AddToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        /// <summary>
        /// ログインを実施します
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LoginResponseModel> Login(LoginRequestModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new LoginResponseModel();

            // ユーザの存在チェック
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return response;

            // パスワードのチェック
            var isPasswordOk = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordOk) return response;

            var roles = await _userManager.GetRolesAsync(user);
            // JWTトークンの設定
            response.Token = _jwtService.GenerateEncodedToken(user.UserName, roles);
            return response;
        }

        /// <summary>
        /// 外部サービスによるログインを実施します
        /// </summary>
        /// <param name="email"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<LoginResponseModel> ExternalLogin(string email, ExternalLoginInfo info)
        {
            var response = new LoginResponseModel();

            // ユーザの存在チェック
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return response;

            await _userManager.AddLoginAsync(user, info);

            var roles = await _userManager.GetRolesAsync(user);
            // JWTトークンの設定
            response.Token = _jwtService.GenerateEncodedToken(user.UserName, roles);
            return response;
        }
    }
}
