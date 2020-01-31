using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext: DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts {get; set;}
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
            
        }
    }
}