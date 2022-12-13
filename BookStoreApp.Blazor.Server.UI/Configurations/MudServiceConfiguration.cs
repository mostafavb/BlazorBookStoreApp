using MudBlazor;
using MudBlazor.Services;

namespace BookStoreApp.Blazor.Server.UI.Configurations;

public static class MudServiceConfiguration
{
    public static MudServicesConfiguration Config()
    {
        MudServicesConfiguration config = new MudServicesConfiguration();
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

        config.SnackbarConfiguration.PreventDuplicates = false;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 7000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        return config;
    }
}
