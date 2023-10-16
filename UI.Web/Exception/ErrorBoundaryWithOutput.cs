using Microsoft.AspNetCore.Components.Web;

namespace UI.Web.Exception
{
    public class ErrorBoundaryWithOutput : ErrorBoundary
    {
        public new System.Exception? CurrentException => base.CurrentException;
    }
}