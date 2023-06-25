using Shipping.Models;
using Shipping.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Constants;
using Shipping.ViewModels.ClaimsPermissions;
using System.Security.Claims;
using System.Data;

namespace Shipping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManager<ApplicationUser> _userManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Permissions.Controls.View)]
        [HttpGet]
        public IActionResult Index()
        { return View(); }
        /*------------------------------- Roles --------------------------------------------------*/
        #region ListRoles
        [Authorize(Permissions.Controls.View)]
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }
        #endregion

        #region Add Role
        [Authorize(Permissions.Controls.Create)]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel RoleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = RoleVM.RoleName;
                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return View("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }
        #endregion

        #region Edit Roles
        [Authorize(Permissions.Controls.Edit)]
        [HttpGet]
        public async Task<IActionResult> EditRole(string ID)
        {
            var role = await _roleManager.FindByIdAsync(ID);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id : {ID} can't be found.";
                return View("Notfound");
            }
            else
            {
                var model = new EditRoleViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id : {model.RoleId} can't be found.";
                return View("Notfound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(model);
            }
        }

        #endregion

        #region Delete Role
        [Authorize(Permissions.Controls.Delete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id : {RoleId} can't be found.";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return RedirectToAction("ListRoles");
                }
            }
        }
        #endregion

        /*------------------------------- Users Role --------------------------------------------------*/
        #region ListUsers
        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            List<UserRoleViewModel> model = new List<UserRoleViewModel>();
            List<ApplicationUser> users = _userManager.Users.ToList();
            for (int i = 0; i < users.Count; i++)
            {
                UserRoleViewModel user = new UserRoleViewModel()
                {
                    Id = users[i].Id,
                    Address = users[i].Address,
                    Email = users[i].Email,
                    UserName = users[i].UserName,
                    PhoneNumber = users[i].PhoneNumber,
                };
                //get Role Name
                var userRole = await _userManager.GetRolesAsync(users[i]);
                user.RoleName = userRole[0];

                //get Role Id
                var RoleData = await _roleManager.FindByNameAsync(user.RoleName);
                user.RoleId = RoleData.Id;

                //add to model
                model.Add(user);
            }

            List<IdentityRole> roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;

            return View(model);
        }
        #endregion

        #region Manage User Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            //get Role Name
            var userRole = await _userManager.GetRolesAsync(user);
            var oldRole = _roleManager.Roles.FirstOrDefault(r => r.Name == userRole[0]);



            var newRole = _roleManager.Roles.FirstOrDefault(r => r.Id == roleId);

            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
            await _userManager.AddToRoleAsync(user, newRole.Name);

            return Ok();
        }
        #endregion

        #region Search role

        public async Task <IActionResult> Search(string query)
        {
            List<IdentityRole> roles;
            if (string.IsNullOrWhiteSpace(query))
            {  roles = await _roleManager.Roles.ToListAsync(); }
            else
            {
                 roles= await _roleManager.Roles.Where(r => r.Name.Contains(query)).ToListAsync();
                

            }
            return View("ListRoles", roles);
        }

        #endregion

        /*------------------------------- Cliams --------------------------------------------------*/
        #region Manage Permissions
        [HttpGet]
        [Authorize(Permissions.Controls.View)]
        public async Task<IActionResult> ManagePermissions(string roleId) 
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
             return NotFound(); 


            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();

            var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = allClaims.Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue)) 
                permission.IsSelected = true;
            }
            var viewModel = new PermissionsFormViewModel
            {
                RoleId=roleId,
                RoleName=role.Name,
                RoleCalims=allPermissions
               

            };
            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Controls.View)]
        public async Task<IActionResult> ManagePermissions(PermissionsFormViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction("Index");
        }

        #endregion

    }
}
