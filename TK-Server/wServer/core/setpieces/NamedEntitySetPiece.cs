using wServer.core.objects;
using wServer.core.worlds;

namespace wServer.core.setpieces
{
    public sealed class NamedEntitySetPiece : ISetPiece
    {
        private string EntityName;

        public NamedEntitySetPiece(string entityName) => EntityName = entityName;

        public int Size => 5;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var entity = Entity.Resolve(world.Manager, EntityName);

            if (entity == null)
                return;

            entity.Move(pos.X + (Size / 2f), pos.Y + (Size / 2f));
            world.EnterWorld(entity);
        }
    }
}
