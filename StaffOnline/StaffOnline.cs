using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using TShockAPI.Hooks;
using Terraria;
using TerrariaApi.Server;

namespace StaffOnline
{
    [ApiVersion(1, 16)]
    public class StaffOnline : TerrariaPlugin
    {
        #region Plugin Info
        public override Version Version
        {
            get { return new Version("1.0"); }
        }
        public override string Name
        {
            get { return "StaffOnline"; }
        }
        public override string Author
        {
            get { return "Bippity"; }
        }
        public override string Description
        {
            get { return "Shows staffs online"; }
        }
        public StaffOnline(Main game)
            : base(game)
        {
            Order = 1;
        }
        #endregion


        public override void Initialize()
        {
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreet);
            TShockAPI.Hooks.PlayerHooks.PlayerCommand += OnCommand;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreet);
                TShockAPI.Hooks.PlayerHooks.PlayerCommand -= OnCommand;
            }
            base.Dispose(disposing);
        }

        public void OnGreet(GreetPlayerEventArgs args)
        {
            if (TShock.Players[args.Who] == null)
                return;
            TSPlayer player = TShock.Players[args.Who];

            var sb = new StringBuilder();
            foreach (TSPlayer user in TShock.Players)
            {
                if (user != null && user.Active && user.Group.HasPermission("staffonline"))
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(user.Name);
                }
            }
            player.SendMessage("Staff Online: " + sb.ToString(), Color.Cyan);
        }

        public void OnCommand(PlayerCommandEventArgs args)
        {
            if (args.Handled)
                return;
            TSPlayer player = args.Player;

            if (args.CommandName.Equals("who") || args.CommandName.Equals("online") || args.CommandName.Equals("playing"))
            {
                var sb = new StringBuilder();
                foreach (TSPlayer user in TShock.Players)
                {
                    if(user != null && user.Active && user.Group.HasPermission("staffonline"))
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(user.Name);
                    }
                }
                player.SendMessage("Staff Online: " + sb.ToString(), Color.Cyan);
                return;
            }
        }
    }
}
