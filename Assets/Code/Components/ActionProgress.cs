namespace Kk.BusyEcs
{
    public struct ActionProgress
    {
        public float spent;
        public float mainDuration;
        public float cooldownDuration;

        public ActionProgress(float mainDuration, float cooldownDuration) : this()
        {
            this.mainDuration = mainDuration;
            this.cooldownDuration = cooldownDuration;
            spent = 0;
        }

        public bool DoesNothing() => spent >= mainDuration + cooldownDuration;
    }
}