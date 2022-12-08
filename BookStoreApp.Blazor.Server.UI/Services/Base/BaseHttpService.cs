using Blazored.LocalStorage;

namespace BookStoreApp.Blazor.Server.UI.Services.Base;

public class BaseHttpService
{
    private readonly IClient client;
    private readonly ILocalStorageService localStorage;

    public BaseHttpService(IClient client, ILocalStorageService localStorage)
    {
        this.client = client;
        this.localStorage = localStorage;
    }

    protected Response<Guid> ConvertApiException<Guid>(ApiException apiException)
    {
        if (apiException.StatusCode == 400)
            return new Response<Guid>()
            {
                Message = "Validation errors have occurd.",
                ValidationErrors = apiException.Response,
                Success = false
            };

        if (apiException.StatusCode == 404)
            return new Response<Guid>()
            {
                Message = "The request item could not be found.",
                Success = false
            };
        return new Response<Guid>()
        {
            Message = "Somthing went wrong please try agein.",
            Success = false
        };
    }
}
