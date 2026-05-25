namespace _Project.Scripts
{
    public class Bunny : Animal
    {
        protected override Entity GetTarget()
        {
            return G.Instance.EntityContainer.GetNearestPlant(transform.position);
        }
    }
}