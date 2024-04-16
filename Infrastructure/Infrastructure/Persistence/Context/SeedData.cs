using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Infrastructure.Persistence
{

    public static class SeedData
    {

        #region مایگریت کردن و مقدار دهی اولیه دیتابیس ها
        public static void SeedDatabases(this IApplicationBuilder app)
        {
            app.InitApplicationDb();
        }
        #endregion



        #region مقدار دهی اولیه به دیتابیس اپلیکیشن و آیدنتیتی
        public static void InitApplicationDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                #region migrate databases
                var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                applicationContext.Database.Migrate();
                #endregion


                #region seedData
                #region شهر و استان
                long cityId = 0;
                if (!applicationContext.Set<City>().Any())
                {
                    var tehran = new City()
                    {
                        Name = "تهران",
                        Province = new Province
                        {
                            Name = "تهران"
                        }
                    };
                    applicationContext.Set<City>().AddAsync(tehran);
                    applicationContext.SaveChanges();
                    cityId = tehran.Id;
                }
                else
                {
                    cityId = applicationContext.Set<City>()
                        .Select(x => x.Id)
                        .FirstOrDefault();
                }
                #endregion

                #region نقش ادمین
                long roleId = 0;
                if (!applicationContext.Set<Role>().Any())
                {
                    var adminRole = new Role()
                    {
                        Title = "ادمین",
                        Claims = new List<RoleClaim>()
                        {
                            new RoleClaim(){ Claim = "Full"}
                        }
                    };
                    applicationContext.Set<Role>().AddAsync(adminRole);
                    applicationContext.SaveChanges();
                    roleId = adminRole.Id;
                }
                else
                {
                    roleId = applicationContext.Set<Role>()
                        .Select(x => x.Id)
                        .FirstOrDefault();                
                }
                #endregion

                #region کاربر ادمین
                if (!applicationContext.Set<User>().Any())
                {
                    var admin = new User()
                    {
                        FirstName = "ادمین",
                        LastName = "کل",
                        CityId = cityId,
                        Username = "admin",
                        Password = "1qaz@WSX".GetHash(),
                        Mobile = "09359785415",
                        MobileConfirmed = true,
                        PasswordIsChange = true,
                        IsEnabled = true,
                        Roles = new List<UserRole> {
                            new UserRole(){ RoleId = roleId }
                        }
                    };
                    applicationContext.Set<User>().AddAsync(admin);
                }
                //else
                //{
                    //var admin1 = applicationContext.Set<User>().FirstOrDefault(x => x.Username == "admin");
                    //admin1.Password = "1qaz@WSX".GetHash();
                    //admin1.MobileConfirmed = true;
                    //admin1.PasswordIsChange = true;
                    //applicationContext.Update(admin1);
                //}
                #endregion
                #endregion


                applicationContext.SaveChanges();

            }
        }
        #endregion



    }
}
