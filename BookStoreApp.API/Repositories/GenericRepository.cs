namespace BookStoreApp.API.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly BookStoreDbContext db;

    public GenericRepository(BookStoreDbContext db)
    {
        this.db = db;
    }
    public async Task<T> AddAsync(T entity)
    {
        await db.AddAsync(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync() =>
        await db.Set<T>().ToListAsync();


    public async Task<T> GetAsync(int? id)
    {
        if (id == null)
            return null;
        return await db.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        db.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id) =>
     await GetAsync(id) != null;
   
}