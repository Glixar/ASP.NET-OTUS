using PromoCodeFactory.DataAccess.DataContext;

namespace PromoCodeFactory.DataAccess.Data;

public class EfDbInitialization : IDbInitialization
{
    private readonly DataBaseContext _context;
 
    public EfDbInitialization(DataBaseContext databaseContext)
    {
        _context = databaseContext;
    }

    public void Init()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
}