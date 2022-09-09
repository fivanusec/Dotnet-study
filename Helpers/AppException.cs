using System.Globalization;

namespace Dotnetrest.Helper
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string msg) : base(msg) { }

        public AppException(string msg, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, msg, args)) { }
    }
}