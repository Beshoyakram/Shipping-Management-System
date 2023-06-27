using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        public DeliveryRepository(MyContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
        }
        #region GetAllDeliveriesWithNameOrNon
        public async Task<List<DeliveryViewModel>> GetAll(string Name)
        {
            List<Delivery> deliveryList = new List<Delivery>();
            
            if (Name == null)
            {
                deliveryList = await _context.Deliveries
                .Include(m => m.User).Where(m => m.User.IsDeleted==false)
                .ToListAsync();
            }
            else
            {
                deliveryList = await _context.Deliveries
                .Include(m => m.User)
                .Where(m => m.User.Name.Contains(Name)&&m.User.IsDeleted==false)
                .ToListAsync();
            }
            //List<ApplicationUser> users = _context.Users.ToList();
            //List<Branch> branch = _context.Branches.ToList();

            List<DeliveryViewModel> deliveriesViewModel = new List<DeliveryViewModel>();
                foreach (var delivery in deliveryList)
                {
                    deliveriesViewModel.Add(new DeliveryViewModel
                    { 
                        DeliveryId = delivery.User.Id,
                        Address = delivery.Address,
                        BranchId = delivery.BranchId,
                        Email = delivery.User.Email,
                        Government = delivery.Governement,
                        Name = delivery.User.Name,
                        Password = delivery.User.PasswordHash,
                        Phone = delivery.User.PhoneNumber,
                        //BranchName = branch.Where(p => p.Id == delivery.BranchId).Select(p=>p.Name).SingleOrDefault(),
                        status = delivery.User.Status,
                        //Type = delivery.User.Type.ToString(),
                        DiscountType = delivery.DiscountType,
                        CompanyPercentage =delivery.CompanyPercent

                    });
                }
            return deliveriesViewModel;
        }
        #endregion

        #region AddingNewDelivery
        public async void AddDelivery(DeliveryViewModel deliveryViewModel)
        {
           var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Name = deliveryViewModel.Name,
                Email = deliveryViewModel.Email,
                PhoneNumber = deliveryViewModel.Phone,
                //Type = TypeOfUser.Merchant
                
            };
            user.PasswordHash = hasher.HashPassword(user, deliveryViewModel.Password);
            // Create a new Delivery object and set its properties
            var delivery = new Delivery
            {
                Governement = deliveryViewModel.Government,
                Address = deliveryViewModel.Address,
                DiscountType = deliveryViewModel.DiscountType,
                CompanyPercent = deliveryViewModel.CompanyPercentage,
                BranchId = deliveryViewModel.BranchId
            };
            delivery.User = user;

            // Add the delivery to the database
            _context.Add(delivery);
            _context.SaveChanges();
        }
        #endregion

        #region SearchForDeliveryById

        public async Task<Delivery> GetDeliveryById(string id)
        {
            var delivery = await _context.Deliveries
            .Include(m => m.User).Where(m => m.User.IsDeleted==false)
            .FirstOrDefaultAsync(m => m.User.Id == id);
            if (delivery == null)
            {
                return null;
            }
            return delivery ;
        }
        public async Task<DeliveryViewModel> GetById(string id)
        {
            var delivery = await _context.Deliveries
            .Include(m => m.User).Where(m => m.User.IsDeleted==false)
            .FirstOrDefaultAsync(m => m.User.Id == id);


            if (delivery == null)
            {
                return null;
            }

            var deliveryViewModel = new DeliveryViewModel
            {
                DeliveryId = delivery.User.Id,
                Address = delivery.Address,
                BranchId = delivery.BranchId,
                Email = delivery.User.Email,
                Government = delivery.Governement,
                Name = delivery.User.Name,
                Phone = delivery.User.PhoneNumber,
                Password = delivery.User.PasswordHash,
                status = delivery.User.Status,
                DiscountType= delivery.DiscountType,
                CompanyPercentage = delivery.CompanyPercent,
                //Type = delivery.User.Type.ToString()
            };

            return deliveryViewModel;
        }
        #endregion

        #region EditMyDelivery
        public void EditDelivery(Delivery delivery,DeliveryViewModel deliveryViewModel)
        {
            if (delivery != null)
            {
                delivery.Address = deliveryViewModel.Address;
                delivery.Governement = deliveryViewModel.Government;
                delivery.BranchId = deliveryViewModel.BranchId;
                delivery.DiscountType= deliveryViewModel.DiscountType;
                delivery.CompanyPercent = deliveryViewModel.CompanyPercentage;

                delivery.User.Status = deliveryViewModel.status; 
                delivery.User.Email = deliveryViewModel.Email;
                delivery.User.PhoneNumber = deliveryViewModel.Phone;
                delivery.User.Name = deliveryViewModel.Name;


                _context.SaveChanges();
            }
        }

        #endregion
        public void Delete(Delivery delivery)
        {
            if(delivery != null) {
                delivery.User.IsDeleted = true;
                }
             _context.SaveChanges();
        }

        public List<Branch> GetAllBranches() {
            var Branchs = _context.Branches.ToList();
            return Branchs;
            }

        public void UpdateStatus(Delivery delivery,bool status)
        {
             if(delivery != null) {
                delivery.User.Status =status;
                 _context.SaveChanges();
                }
        }
    }
}