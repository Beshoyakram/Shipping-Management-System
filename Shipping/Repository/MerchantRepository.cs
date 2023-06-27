using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        MyContext _myContext;
        public MerchantRepository(UserManager<ApplicationUser> userManager, MyContext myContext)
        {
            _userManager = userManager;
            _myContext = myContext;
        }

        
        #region addMerchant
        public async Task<bool> AddMechant(MerchantViewModel merchantViewModel)
        {
            var merchant = new Merchant
            {
                Address = merchantViewModel.Address,
                City = merchantViewModel.City,
                Government = merchantViewModel.Government,
                PickUpSpecialCost = merchantViewModel.PickUpPrice,
                RefusedOrderPercent = merchantViewModel.RefuseCostPercent,
                BranchId = merchantViewModel.BranchId,



            };
            var user = new ApplicationUser()
            {
                Email = merchantViewModel.Email,
                PhoneNumber = merchantViewModel.Phone,
                Name = merchantViewModel.Name,
                UserName = merchantViewModel.Name
                //Type = TypeOfUser.Merchant


            };
            var result = await _userManager.CreateAsync(user, merchantViewModel.Password);
            if (result.Succeeded)
            {
                var _user = await _userManager.FindByEmailAsync(user.Email);
                if (_user != null)
                    await _userManager.AddToRoleAsync(_user, "التجار");

                merchant.UserId = _user.Id;
                _myContext.Add(merchant);
                _myContext.SaveChanges();

                return true;
            }
            return false;
        }



        #endregion


        #region getAll

        public async Task<List<MerchantViewModel>> GetAll(string Name)
        {

            List<Merchant> merchants = new List<Merchant>();

            if (Name == null)
            {
                merchants = await _myContext.Merchants
                .Include(m => m.User)
                .ToListAsync();
            }
            else
            {
                merchants = await _myContext.Merchants
                .Include(m => m.User)
                .Where(m => m.User.Name.Contains(Name))
                .ToListAsync();
            }
            List<MerchantViewModel> merchantsViewModel = new List<MerchantViewModel>();
            foreach (var merchant in merchants)
            {
                merchantsViewModel.Add(new MerchantViewModel
                {
                    MerchantId = merchant.User.Id,
                    Address = merchant.Address,
                    BranchId = merchant.BranchId,
                    City = merchant.City,
                    Email = merchant.User.Email,
                    Government = merchant.Government,
                    Name = merchant.User.Name,
                    Phone = merchant.User.PhoneNumber,
                    Password = merchant.User.PasswordHash,
                    PickUpPrice = merchant.PickUpSpecialCost,
                    RefuseCostPercent = merchant.RefusedOrderPercent,
                    //Type = merchant.User.Type.ToString(),
                    Status = merchant.User.Status


                });
            }
            return merchantsViewModel;
        }

        #endregion


        #region GetById
        public async Task<MerchantViewModel> GetById(string id)
        {
            var merchant = await _myContext.Merchants
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.User.Id == id);


            if (merchant == null)
            {
                return null;
            }

            var merchantViewModel = new MerchantViewModel
            {
                MerchantId = merchant.User.Id,
                Address = merchant.Address,
                BranchId = merchant.BranchId,
                City = merchant.City,
                Email = merchant.User.Email,
                Government = merchant.Government,
                Name = merchant.User.Name,
                Phone = merchant.User.PhoneNumber,
                Password = merchant.User.PasswordHash,
                PickUpPrice = merchant.PickUpSpecialCost,
                RefuseCostPercent = merchant.RefusedOrderPercent,
                //Type = merchant.User.Type.ToString()
            };

            return merchantViewModel;
        }


        #endregion


        #region EditUser
        public async void EditMerchant(Merchant merchant, MerchantViewModel merchantViewModel)
        {

            if (merchant != null)
            {
                merchant.Address = merchantViewModel.Address;
                merchant.City = merchantViewModel.City;
                merchant.Government = merchantViewModel.Government;
                merchant.PickUpSpecialCost = merchantViewModel.PickUpPrice;
                merchant.RefusedOrderPercent = merchantViewModel.RefuseCostPercent;
                merchant.BranchId = merchantViewModel.BranchId;

                merchant.User.Email = merchantViewModel.Email;
                merchant.User.PhoneNumber = merchantViewModel.Phone;
                merchant.User.Name = merchantViewModel.Name;


                _myContext.SaveChanges();
            }
        }



        #endregion


        #region updateStatus
        public void UpdateStatus(Merchant merchant, bool status)
        {
            if (merchant != null)
            {
                merchant.User.Status = status;
                _myContext.SaveChanges();
            }
        }
        #endregion




    }
}
