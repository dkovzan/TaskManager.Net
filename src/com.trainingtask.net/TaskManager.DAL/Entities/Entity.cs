namespace TaskManager.DAL.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public int IsDeleted { get; set; }
    }
}