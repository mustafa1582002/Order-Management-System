using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Dtos;
using OrderManagement.Api.Errors;
using OrderManagement.Core.Services;
using OrderManagement.Repository.Data;
using Customer = OrderManagement.Core.Entities.Identity.Customer;

namespace OrderManagement.Api.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly OrderManagementDbContext _dbContext;

        public AccountsController(UserManager<IdentityUser> userManager
            ,SignInManager<IdentityUser> signInManager
            ,ITokenService tokenService
            ,OrderManagementDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<ReturnUserDto>> CreateCustomer(RegisterUserDto model)
        {
            var CheckedEmail = await _userManager.FindByEmailAsync(model.Email) is not null;
            if (CheckedEmail)
                return BadRequest(new ApiResponse(400, "Email Is Already in Use"));


            var appUser = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(appUser, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            await _userManager.AddToRoleAsync(appUser, "Customer");

            var user = await _userManager.FindByEmailAsync(model.Email);
            ////////////////////////
            var customer = new Customer()
            {
                Id=user.Id,
                Email = model.Email,
                Name=model.UserName,
            };
            await _dbContext.Set<Customer>().AddAsync(customer);
            //var ResultSuccedd= check if the Result Succeed
                await _dbContext.SaveChangesAsync();
            //////////////////////
            var ReturnedUser = new ReturnUserDto()
            {
                UserName = model.UserName,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(appUser, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ReturnUserDto>> Register(RegisterUserDto model)
        {
            var CheckedEmail= await _userManager.FindByEmailAsync(model.Email) is not null;
            if (CheckedEmail)
                return BadRequest(new ApiResponse(400, "Email Is Already in Use"));

            var appUser = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(appUser, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            await _userManager.AddToRoleAsync(appUser, "Customer");
            var user = await _userManager.FindByEmailAsync(model.Email);

            var customer = new Customer()
            {
                Id = user.Id,
                Email = model.Email,
                Name = model.UserName,
            };
            await _dbContext.Set<Customer>().AddAsync(customer);
            //var ResultSuccedd= check if the Result Succeed
            await _dbContext.SaveChangesAsync();

            var ReturnedUser = new ReturnUserDto()
            {
                UserName = model.UserName,
                Email = model.Email,
                Token =await _tokenService.CreateTokenAsync(appUser, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ReturnUserDto>> login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new ReturnUserDto()
            {
                UserName = User.UserName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }

        
    }
}
