using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PawnArt.Application;
using PawnArt.Logic;
using PawnArt.Logic.DataAccessInterface;

namespace PawnArt.DataAccess
{
    public static class ServiceCollectionExtenstions
    {
        public static IServiceCollection AddEFCoreDataAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<GameContext>(
                options =>
                    options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUnitOfWorkFactory, EFCoreUnitOfWorkFactory>();
            return services;
        }
    }
}
