using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(PapiroMVC.App_Start.MySuperPackage), "PreStart")]

namespace PapiroMVC.App_Start {
    public static class MySuperPackage {
        public static void PreStart() {
            MVCControlsToolkit.Core.Extensions.Register();
        }
    }
}