namespace _Project.Scripts
{
    public class Fox : Animal
    {
        protected override Entity GetTarget()
        {
            return G.Instance.EntityContainer.GetNearestBunny(transform.position);
        }
    }
}