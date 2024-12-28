using Food.Services.EmailAPI.Data;
using Food.Services.EmailAPI.Message;
using Food.Services.EmailAPI.Models;
using Food.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Food.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;


        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task EmailShoppingCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Shopping Cart Email Requested");
            message.AppendLine("<br/> Total " + cartDto.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("<li>");

            }
            message.AppendLine("<ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);

        }

        public async Task LogOrderPlaced(RewardMessage rewardsDto)
        {
            string message = "New Order Placed. <br/> OrderId : " + rewardsDto.OrderId;
            await LogAndEmail(message, "shri@gmail.com");
        }

        public async Task RegisterUserAndLog(string email)
        {
          string message = "User Registered. <br/> Email : " + email;
            await LogAndEmail(message, "shri@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new EmailLogger()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
               await using var _db = new AppDbContext(_dbOptions);
               await _db.EmailLoggers.AddAsync(emailLogger);
               await _db.SaveChangesAsync();
               return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
