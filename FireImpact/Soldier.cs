using MathNet.Spatial.Euclidean;

namespace FireImpact
{
    /// <summary>
    /// <c>Soldier</c> class   . Soldiers are represented as point.
    /// </summary>
    public class Soldier
    {
        public int Id { get; set; }
        public Vector3D Positon { get; set; }

        public Soldier(int id, Vector3D positon)
        {
            Id = id;
            Positon = positon;
        }

    }
}
