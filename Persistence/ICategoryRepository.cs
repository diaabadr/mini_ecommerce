namespace Persistence
{
    public interface ICategoryRepository
    {
        Task<bool> IsExistAsync(string addingCategoryId);
    }
}
