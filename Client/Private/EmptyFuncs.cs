namespace PT
{
    partial class Plane
    {
        public void DrawBullet(Scene.FindTargetResult t) { }
        public void DrawDamage(Plane.HurtResult r) { }
        public void ClientInit() { }
        public void ClientRestore() { }
        public void DrawLocked(int id, bool b) { }
        public void DrawStartFire(float a) { }
        public void DrawStopFire(float a) { }
    }
    partial class Plane_Laser
    {
        public new void ClientInit() { }
        public new void ClientRestore() { }
    }
    partial class Plane0
    {
        public new void ClientInit() { }
        public new void ClientRestore() { }
    }
    partial class Plane1
    {
        public new void ClientInit() { }
        public new void ClientRestore() { }
    }
    partial class Plane2
    {
        public new void ClientInit() { }
        public new void ClientRestore() { }
    }

    partial class Gas
    {
        public Plane eater;
    }

    partial class Player
    {
        public void DrawEvolution() { }
    }
}
