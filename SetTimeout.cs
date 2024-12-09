

namespace Minecraft_Server
{
    internal class SetTimeout
    {
        private bool stop = false;

        private Action action;
        private int time;
        private bool loop;

        public SetTimeout(Action action, int time, bool loop = false)
        {
            this.action = action;
            this.time = time;
            this.loop = loop;

            Loop();
        }

        private async void Loop()
        {
            if (stop) return;

            await Task.Delay(time);

            action();

            if (!loop) return;
            Loop();
        }

        public void Stop()
        {

        }
    }
}
