using System.Runtime.InteropServices;

namespace EMT.Extensions.Hosting.WinUI3;
internal static class PInvokeFunctions
{
    [DllImport("Microsoft.ui.xaml.dll")]
    internal static extern void XamlCheckProcessRequirements();
}
