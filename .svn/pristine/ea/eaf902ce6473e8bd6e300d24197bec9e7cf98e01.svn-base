using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Api.Provider
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRender(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> Render<T>(string viewName, T model)
        {
            DefaultHttpContext httpcontext = new()
            {
                RequestServices = _serviceProvider
            };

            ActionContext context = new(httpcontext, new RouteData(), new ActionDescriptor());

            IView view = FindView(context, viewName);

            using StringWriter output = new();
            ViewContext viewContext = new(context, view,
                new ViewDataDictionary<T>(metadataProvider: new EmptyModelMetadataProvider(), modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(context.HttpContext, _tempDataProvider),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        #region private methods

        private IView FindView(ActionContext context, string viewName)
        {
            ViewEngineResult getViewResult = _viewEngine.GetView(null, viewName, true);
            if (getViewResult.Success)
                return getViewResult.View;

            ViewEngineResult findViewResult = _viewEngine.FindView(context, viewName, true);
            if (findViewResult.Success)
                return findViewResult.View;

            throw new InvalidOperationException(
                $"Unable to find view '{viewName}'. The following locations were searched:"
                + string.Join(Environment.NewLine, getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations)));
        }

        #endregion private methods
    }
}